using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerStackHolder : MonoBehaviour
{
    public static int layerCount;
    public List<GameObject>[] dep_layers;
    public int topLayer;
    public GameObject meshGenPrefab;
    public GameObject layerStackPrefab;
    public GameObject processGenPrefab;
    public GameObject processEtchPrefab;
    public GameObject processIonEtchPrefab;
    public float layerHeight;
    public control.materialType curMaterial;
    public bool deletedFlag;
    public List<int> deletedLayers;
    // Start is called before the first frame update
    void Start()
    {
        layerCount = 100;
        dep_layers = new List<GameObject>[layerCount].Select(item => new List<GameObject>()).ToArray();
        topLayer = -1;
        layerHeight = 0.1f;
        curMaterial = control.materialType.chromium;
        deletedLayers = new List<int>();
        deletedFlag = false;
    }


    public void onValueChange(int num)
    {
        switch (num)
        {
            case 0:
                curMaterial = control.materialType.chromium;
                break;
            case 1:
                curMaterial = control.materialType.gold;
                break;
            case 2:
                curMaterial = control.materialType.aluminum;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            depositLayer(curMaterial,ones());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            liftOff();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            depositLayer(control.materialType.photoresist, GameObject.Find("drawing_panel").GetComponent<paint>().grid);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
             Instantiate(processEtchPrefab, transform.position, transform.rotation).gameObject.name = "New Process";
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(processGenPrefab, transform.position, transform.rotation).gameObject.name = "New Process";
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Instantiate(processIonEtchPrefab, transform.position, transform.rotation).gameObject.name = "New Process";
        }
    }

    void LateUpdate()
    {
        clearDeletes();
    }
    
    public void clearDeletes(){
        if (deletedFlag)
        {
            foreach(int i in deletedLayers)
            {
                List<GameObject> curList = dep_layers[i];
                for (int j = 0; j < curList.Count; j++)
                {
                    if (!curList[j] || curList[j].GetComponent<meshGenerator>().toBeDestroyed)
                    {
                        curList.RemoveAt(j);
                    }
                }
                if(curList.Count() == 0 && i >= topLayer)
                {
                    topLayer--;
                }

            }
            deletedFlag = false;
            deletedLayers.Clear();

        }
    }

    public bool addDeposit(int curlayer, int[,] grid, control.materialType layerMaterial, int newTimeOffset = 0)
    {
        if(curlayer >= layerCount)
        {
            return false;
        }
        if(curlayer > topLayer)
        {
            curlayer = topLayer + 1;
            topLayer++;
        }
        GameObject newMesh = Instantiate(meshGenPrefab ,transform.position + new Vector3(0, layerHeight * (float) curlayer,0) , transform.rotation);
        newMesh.GetComponent<meshGenerator>().layerHeight = layerHeight;
        int[,] newgrid = newMesh.GetComponent<meshGenerator>().grid;
        System.Array.Copy(grid, newgrid, control.gridWidth * control.gridHeight);
        newMesh.GetComponent<meshGenerator>().initialize();
        newMesh.GetComponent<meshMaterial>().myMaterial = layerMaterial;
        newMesh.GetComponent<meshMaterial>().initialize(newTimeOffset);
        dep_layers[curlayer].Add(newMesh);

        return true;
    }


    public void depositLayer(control.materialType layerMaterial, int[,] inputGrid, int newTimeOffset = 0)
    {
        int[,] grid = new int[control.gridWidth, control.gridHeight];
        System.Array.Copy(inputGrid, grid, control.gridWidth * control.gridHeight);
        int curLayer = topLayer + 1;
        while(curLayer > 0)
        {
            int[,] thisDeposit = new int[control.gridWidth, control.gridHeight];
            System.Array.Copy(grid, thisDeposit, control.gridWidth * control.gridHeight);
            int[,] tempDeposit = new int[control.gridWidth, control.gridHeight];
            System.Array.Copy(zeros(), tempDeposit, control.gridWidth * control.gridHeight);
            foreach (GameObject curDeposit in dep_layers[curLayer-1])
            {
                tempDeposit = union(tempDeposit, curDeposit.GetComponent<meshGenerator>().grid);
            }
            thisDeposit = intersect(tempDeposit, thisDeposit);
            if (!isEmpty(thisDeposit))
            {
                addDeposit(curLayer, thisDeposit, layerMaterial, newTimeOffset);
            }
            foreach (GameObject curDeposit in dep_layers[curLayer-1])
            {
                grid = emptyIntersect(grid, curDeposit.GetComponent<meshGenerator>().grid);
                if (isEmpty(grid))
                {
                    return;
                }
            }
            curLayer--;
        }
        addDeposit(0, grid, layerMaterial, newTimeOffset);
    }

    //if: spray removal 
    //then: remove lotsa things
    public void etchLayer(control.materialType etchMaterial, int newTimeOffset = 0)
    {
        int[,] grid = new int[control.gridWidth, control.gridHeight];
        System.Array.Copy(ones(), grid, control.gridWidth * control.gridHeight);
        int curLayer = topLayer + 1;
        while (curLayer > 0)
        {
            int[,] emptySpots = new int[control.gridWidth, control.gridHeight];
            int[,] etchedSpots = new int[control.gridWidth, control.gridHeight];
            System.Array.Copy(zeros(), emptySpots, control.gridWidth * control.gridHeight);
            System.Array.Copy(zeros(), etchedSpots, control.gridWidth * control.gridHeight);
            foreach (GameObject curDeposit in dep_layers[curLayer - 1])
            {
                if(curDeposit.GetComponent<meshMaterial>().timeOffset >= 0 || (curDeposit.GetComponent<meshMaterial>().timeOffset < 0 && curDeposit.GetComponent<meshMaterial>().timeOffset >= newTimeOffset && curDeposit.GetComponent<meshMaterial>().myMaterial != etchMaterial)){
                    emptySpots = union(emptySpots, curDeposit.GetComponent<meshGenerator>().grid);
                }
            }
            bool anyFlag = false;
            foreach (GameObject curDeposit in dep_layers[curLayer - 1])
            {
                if(curDeposit.GetComponent<meshMaterial>().myMaterial == etchMaterial)
                {
                    if(curDeposit.GetComponent<meshMaterial>().timeOffset >= 0){
                        if(newTimeOffset < 0){
                            System.Array.Copy(union(intersect(curDeposit.GetComponent<meshGenerator>().grid,grid),etchedSpots), etchedSpots, control.gridWidth * control.gridHeight);
                            anyFlag = true;
                        }
                        updateDeposit(emptyIntersect(curDeposit.GetComponent<meshGenerator>().grid, grid), curDeposit, curLayer-1);

                    }
                }
            }

            if (anyFlag && !isEmpty(etchedSpots)){
                addDeposit(curLayer -1 , etchedSpots, etchMaterial, newTimeOffset);
            }
            System.Array.Copy(emptyIntersect(grid, emptySpots), grid, control.gridWidth * control.gridHeight);
            if (isEmpty(grid))
            {
                return;
            }
            curLayer--;
        }
    }

    public void liftOff()
    {
        int[,] grid = new int[control.gridWidth, control.gridHeight];
        System.Array.Copy(zeros(), grid, control.gridWidth * control.gridHeight);
        for (int i = 0; i<=topLayer; i++)
        {
            foreach (GameObject curDeposit in dep_layers[i])
            {
                if (curDeposit.GetComponent<meshMaterial>().myMaterial != control.materialType.photoresist)
                {
                    updateDeposit(emptyIntersect(curDeposit.GetComponent<meshGenerator>().grid, grid), curDeposit, i);
                }
            }
            foreach (GameObject curDeposit in dep_layers[i])
            {
                if(curDeposit.GetComponent<meshMaterial>().myMaterial == control.materialType.photoresist)
                {
                    grid = union(grid, curDeposit.GetComponent<meshGenerator>().grid);
                    updateDeposit(zeros(), curDeposit, i);
                }
            }
        }
    }


    int[,] union(int[,] ar1, int[,] ar2)
    {
        int[,] grid = new int[ar1.GetLength(0), ar1.GetLength(1)];

        for (int i = 0; i < ar1.GetLength(0); i++)
        {
            for (int j = 0; j < ar1.GetLength(1); j++)
            {
                if(ar1[i,j] != 0 || ar2[i,j] != 0)
                {
                    grid[i, j] = 1;
                }
                else
                {
                    grid[i, j] = 0;
                }
            }
        }
        return grid;
    }

    int[,] intersect(int[,] ar1, int[,] ar2)
    {
        int[,] grid = new int[ar1.GetLength(0), ar1.GetLength(1)];

        for (int i = 0; i < ar1.GetLength(0); i++)
        {
            for (int j = 0; j < ar1.GetLength(1); j++)
            {
                if (ar1[i, j] != 0 && ar2[i, j] != 0)
                {
                    grid[i, j] = 1;
                }
                else
                {
                    grid[i, j] = 0;
                }
            }
        }
        return grid;
    }

    int[,] emptyIntersect(int[,] ar1, int[,] ar2)
    {
        int[,] grid = new int[ar1.GetLength(0), ar1.GetLength(1)];

        for (int i = 0; i < ar1.GetLength(0); i++)
        {
            for (int j = 0; j < ar1.GetLength(1); j++)
            {
                int a = ar1[i, j];
                int b = ar2[i, j];
                if (a != 0 && b == 0)
                {
                    grid[i, j] = 1;
                }
                else
                {
                    grid[i, j] = 0;
                }
            }
        }
        return grid;
    }

    public int[,] ones()
    {
        int[,] grid = new int[control.gridWidth, control.gridHeight];
        for (int i = 0; i < control.gridWidth; i++)
        {
            for (int j = 0; j < control.gridHeight; j++)
            {
                grid[i, j] = 1;
            }
        }
        return grid;
    }

    int[,] zeros()
    {
        int[,] grid = new int[control.gridWidth, control.gridHeight];
        for (int i = 0; i < control.gridWidth; i++)
        {
            for (int j = 0; j < control.gridHeight; j++)
            {
                grid[i, j] = 0;
            }
        }
        return grid;
    }

    int[,] line()
    {
        int[,] grid = new int[control.gridWidth, control.gridHeight];
        for (int i = 0; i < control.gridWidth; i++)
        {
            for (int j = 0; j < control.gridHeight; j++)
            {
                if (i == j)
                {
                    grid[i, j] = 1;
                }
                else
                {
                    grid[i, j] = 0;
                }
            }
        }
        return grid;
    }

    int[,] circle()
    {
        int[,] grid = new int[control.gridWidth, control.gridHeight];
        for (int i = 0; i < control.gridWidth; i++)
        {
            for (int j = 0; j < control.gridHeight; j++)
            {
                if (Mathf.Sqrt(Mathf.Pow(i - control.gridWidth * 0.5f, 2f) + Mathf.Pow(j - control.gridHeight * 0.5f, 2f)) < control.gridWidth * 0.25f)
                {
                    grid[i, j] = 1;
                }
                else
                {
                    grid[i, j] = 0;
                }
            }
        }
        return grid;
    }


    bool isEmpty(int[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public GameObject clone()
    {
        GameObject newLayerStack = Instantiate(layerStackPrefab, transform.position, transform.rotation);
        for(int i = 0; i<=topLayer; i++)
        {
            foreach(GameObject curDeposit in dep_layers[i])
            {
                newLayerStack.GetComponent<LayerStackHolder>().dep_layers[i].Add(curDeposit.GetComponent<meshGenerator>().clone());
            }
        }
        newLayerStack.GetComponent<LayerStackHolder>().topLayer = topLayer;

        return newLayerStack;
    }

    public void copy(GameObject toCopy)
    {
        contentDestroy();
        dep_layers = new List<GameObject>[layerCount].Select(item => new List<GameObject>()).ToArray();
        LayerStackHolder copyLayers = toCopy.GetComponent<LayerStackHolder>();
        for (int i = 0; i <= copyLayers.topLayer; i++)
        {
            foreach (GameObject curDeposit in copyLayers.dep_layers[i])
            {
                dep_layers[i].Add(curDeposit.GetComponent<meshGenerator>().clone());
            }
        }
        topLayer = copyLayers.topLayer;
    }

    public void contentSetActive(bool act)
    {
        for (int i = 0; i <=topLayer; i++)
        {
            foreach (GameObject curDeposit in dep_layers[i])
            {
                curDeposit.SetActive(act);
            }
        }
        gameObject.SetActive(act);
    }

    public void contentDestroy()
    {
        for(int i = 0; i<=topLayer; i++)
        {
            foreach(GameObject curDeposit in dep_layers[i])
            {
                Destroy(curDeposit);
            }
        }
    }

    void updateDeposit(int[,] grid, GameObject deposit, int depLayer)
    {
        if (!isEmpty(grid))
        {
            System.Array.Copy(grid, deposit.GetComponent<meshGenerator>().grid, control.gridWidth * control.gridHeight);
            deposit.GetComponent<meshGenerator>().initialize();
        }
        else
        {
            //dep_layers[depLayer].Remove(deposit);
            deletedLayers.Add(depLayer);
            deletedFlag = true;
            deposit.GetComponent<meshGenerator>().toBeDestroyed = true;
            Destroy(deposit);
        }
        
    }


    public void sliceDeposits(int n)
    {
        int curLayer = 1;
        while (curLayer <= layerCount)
        {
            foreach (GameObject curDeposit in dep_layers[curLayer - 1])
            {
                meshMaterial mat = curDeposit.GetComponent<meshMaterial>();
                if(mat.timeOffset > 0)
                {
                    if (mat.timeOffset <= n)
                    {
                        curDeposit.SetActive(true);
                    }
                    else
                    {
                        curDeposit.SetActive(false);
                    }

                } else if(mat.timeOffset <0)
                {
                    if(mat.timeOffset <= n)
                    {
                        curDeposit.SetActive(true);
                    }
                    else
                    {
                        curDeposit.SetActive(false);
                    }
                }

            }
            curLayer++;
        }
    }

    public void cullDeposits(int n)
    {
        int curLayer = 1;
        while (curLayer <= layerCount)
        {
            foreach (GameObject curDeposit in dep_layers[curLayer - 1])
            {
                curDeposit.SetActive(true);
                meshMaterial mat = curDeposit.GetComponent<meshMaterial>();
                if (mat.timeOffset > 0)
                {
                    if (mat.timeOffset < n)
                    {
                        curDeposit.GetComponent<meshMaterial>().initialize();
                    }
                    else
                    {
                        updateDeposit(zeros(), curDeposit, curLayer-1);
                    }

                }
                else if (mat.timeOffset < 0)
                {
                    if (mat.timeOffset < n)
                    {
                        curDeposit.GetComponent<meshMaterial>().initialize();
                    }
                    else
                    {
                        updateDeposit(zeros(), curDeposit, curLayer-1);
                    }
                }
            }
            curLayer++;
        }
    }

}
