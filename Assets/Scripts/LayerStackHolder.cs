using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//
public class LayerStackHolder : MonoBehaviour
{
    
    public static int layerCount;

    //an array of all of the layers. each index is a list of deposits at that layer
    public List<GameObject>[] depLayers;

    //the current highest layer reached by anything in the design. Deposits will start one above here
    public int topLayer;

    //prefabs
    public GameObject meshGenPrefab;
    public GameObject layerStackPrefab;
    public GameObject processGenPrefab;
    public GameObject processEtchPrefab;
    public GameObject processAluminumEtchPrefab;
    public GameObject processIonEtchPrefab;

    //constant, the height in pixels of a layer
    public float layerHeight;

    //the current material to be etched or deposited. Selected with dropdown
    public control.materialType curMaterial;

    //for deleting deposits
    public bool deletedFlag;
    public List<int> deletedLayers;

    // Start is called before the first frame update
    void Start()
    {
        layerCount = 100;
        depLayers = new List<GameObject>[layerCount].Select(item => new List<GameObject>()).ToArray();
        topLayer = -1;
        layerHeight = 0.1f;
        deletedLayers = new List<int>();
        deletedFlag = false;

        //initially sets the curMaterial to whatever the top option on the materials dropdown is
        onValueChange(GameObject.Find("Dropdown").GetComponent<DropdownCustom>().value);
    }


    public void onValueChange(int num) //Dropdown selection function
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
            case 3:
                curMaterial = control.materialType.silicon;
                break;
            case 4:
                curMaterial = control.materialType.silicondioxide;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (control.isPaused() == control.pauseStates.unPaused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                liftOff();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                etchLayerAround(curMaterial);
            }
        }
    }

    //external functions called by buttons
    public void makePhotoResist()
    {
        depositLayer(control.materialType.photoresist, GameObject.Find("drawing_panel").GetComponent<paint>().grid);
    }

    public void startDepositProcess()
    {
        Instantiate(processGenPrefab, transform.position, transform.rotation).gameObject.name = "New Process";
    }

    public void startEtchProcess()
    {
        if(curMaterial == control.materialType.aluminum)
        {
            Instantiate(processAluminumEtchPrefab, transform.position, transform.rotation).gameObject.name = "New Process";
        }
        else
        {
            Instantiate(processEtchPrefab, transform.position, transform.rotation).gameObject.name = "New Process";
        }

    }


    //deposits marked for deletion are only cleared at the end of each game step so they don't interfere with other functions
    void LateUpdate()
    {
        clearDeletes();
    }
    
    public void clearDeletes(){
        if (deletedFlag)
        {
            bool topResetFlag = false;
            foreach(int i in deletedLayers)
            {
                List<GameObject> curList = depLayers[i];
                for (int j = 0; j < curList.Count; j++)
                {
                    if (!curList[j] || curList[j].GetComponent<meshGenerator>().toBeDestroyed)
                    {
                        curList.RemoveAt(j);
                    }
                }
                if(i == topLayer)
                {
                    topResetFlag = true;
                }

            }
            if (topResetFlag)
            {
                int i = topLayer;
                while (true)
                {
                    if (i < 0)
                    {
                        break;
                    }
                    if (depLayers[i].Count == 0)
                    {
                        topLayer--;
                        i--;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            deletedFlag = false;
            deletedLayers.Clear();

        }
    }


    //insterts a deposit of a material into a particular layer
    bool addDeposit(int curlayer, BitGrid toDeposit, control.materialType layerMaterial, int newTimeOffset = 0)
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
        newMesh.GetComponent<meshGenerator>().grid.set(toDeposit);
        newMesh.GetComponent<meshGenerator>().initialize();
        newMesh.GetComponent<meshMaterial>().myMaterial = layerMaterial;
        newMesh.GetComponent<meshMaterial>().initialize(newTimeOffset);
        newMesh.transform.parent = gameObject.transform;
        depLayers[curlayer].Add(newMesh);

        return true;
    }

    //sets the BitGrid of a deposit to a new value, or destroys it if set to empty
    void updateDeposit(BitGrid grid, GameObject deposit, int depLayer)
    {
        if (!grid.isEmpty())
        {
            deposit.GetComponent<meshGenerator>().grid.set(grid);
            deposit.GetComponent<meshGenerator>().initialize();
        }
        else
        {
            deletedLayers.Add(depLayer);
            deletedFlag = true;
            deposit.GetComponent<meshGenerator>().toBeDestroyed = true;
            Destroy(deposit);
        }

    }

    //drops a 1 block layer of a material from the top, which cascades over any structures below
    public void depositLayer(control.materialType layerMaterial, BitGrid inputGrid, int newTimeOffset = 0)
    {
        BitGrid grid = new BitGrid();
        grid.set(inputGrid);
        int curLayer = topLayer + 1;
        while(curLayer > 0)
        {
            BitGrid thisDeposit = new BitGrid();
            thisDeposit.set(grid);
            BitGrid tempDeposit = new BitGrid();
            tempDeposit.set(BitGrid.zeros());

            foreach (GameObject curDeposit in depLayers[curLayer-1])
            {
                tempDeposit.set(BitGrid.union(tempDeposit, curDeposit.GetComponent<meshGenerator>().grid));
            }
            thisDeposit.set(BitGrid.intersect(tempDeposit, thisDeposit));
            if (!thisDeposit.isEmpty())
            {
                addDeposit(curLayer, thisDeposit, layerMaterial, newTimeOffset);
            }
            foreach (GameObject curDeposit in depLayers[curLayer-1])
            {
                grid.set(BitGrid.emptyIntersect(grid, curDeposit.GetComponent<meshGenerator>().grid));
                if (grid.isEmpty())
                {
                    return;
                }
            }
            curLayer--;
        }
        addDeposit(0, grid, layerMaterial, newTimeOffset);
    }

    //removes the top-most layer of a particular material from the design
    public void etchLayer(control.materialType etchMaterial, int newTimeOffset = 0)
    {
        BitGrid grid = new BitGrid() ;
        grid.set(BitGrid.ones());
        int curLayer = topLayer + 1;
        while (curLayer > 0)
        {
            BitGrid emptySpots = new BitGrid();
            BitGrid etchedSpots = new BitGrid();
            emptySpots.set(BitGrid.zeros());
            etchedSpots.set(BitGrid.zeros());
            foreach (GameObject curDeposit in depLayers[curLayer - 1])
            {
                if(curDeposit.GetComponent<meshMaterial>().timeOffset >= 0 || (curDeposit.GetComponent<meshMaterial>().timeOffset < 0 && curDeposit.GetComponent<meshMaterial>().timeOffset >= newTimeOffset && curDeposit.GetComponent<meshMaterial>().myMaterial != etchMaterial)){
                    emptySpots.set(BitGrid.union(emptySpots, curDeposit.GetComponent<meshGenerator>().grid));
                }
            }
            bool anyFlag = false;
            foreach (GameObject curDeposit in depLayers[curLayer - 1])
            {
                if(curDeposit.GetComponent<meshMaterial>().myMaterial == etchMaterial)
                {
                    if(curDeposit.GetComponent<meshMaterial>().timeOffset >= 0){
                        if(newTimeOffset < 0){
                            etchedSpots.set(BitGrid.union(BitGrid.intersect(curDeposit.GetComponent<meshGenerator>().grid,grid),etchedSpots));
                            anyFlag = true;
                        }
                        updateDeposit(BitGrid.emptyIntersect(curDeposit.GetComponent<meshGenerator>().grid, grid), curDeposit, curLayer-1);

                    }
                }
            }

            if (anyFlag && !etchedSpots.isEmpty()){
                addDeposit(curLayer -1 , etchedSpots, etchMaterial, newTimeOffset);
            }
            grid.set(BitGrid.emptyIntersect(grid, emptySpots));
            if (grid.isEmpty())
            {
                return;
            }
            curLayer--;
        }
    }


    //removes the material from exposed sides of a particular material from the design
    public void etchLayerAround(control.materialType etchMaterial, int newTimeOffset = 0)
    {
        BitGrid grid = new BitGrid();
        grid.set(BitGrid.ones());
        int curLayer = topLayer + 1;
        while (curLayer > 0)
        {
            BitGrid emptySpots = new BitGrid();
            BitGrid etchedSpots = new BitGrid();
            emptySpots.set(BitGrid.zeros());
            etchedSpots.set(BitGrid.zeros());

            foreach (GameObject curDeposit in depLayers[curLayer - 1])
            {
                if (curDeposit.GetComponent<meshMaterial>().timeOffset >= 0)
                {
                    emptySpots.set(BitGrid.union(emptySpots, curDeposit.GetComponent<meshGenerator>().grid));
                }
            }

            BitGrid emptyContinuation = BitGrid.getIntersectedRegions(grid, BitGrid.invert(emptySpots));
            BitGrid emptyBorder = BitGrid.getBorderRegion(emptyContinuation);

            bool anyFlag = false;
            foreach (GameObject curDeposit in depLayers[curLayer - 1])
            {
                if (curDeposit.GetComponent<meshMaterial>().myMaterial == etchMaterial)
                {
                    if (curDeposit.GetComponent<meshMaterial>().timeOffset >= 0)
                    {
                        if (newTimeOffset < 0)
                        {
                            etchedSpots.set(BitGrid.union(BitGrid.intersect(curDeposit.GetComponent<meshGenerator>().grid, emptyBorder), etchedSpots));
                            anyFlag = true;
                        }
                        updateDeposit(BitGrid.emptyIntersect(curDeposit.GetComponent<meshGenerator>().grid, emptyBorder), curDeposit, curLayer - 1);

                    }
                }
            }

            if (anyFlag && !etchedSpots.isEmpty())
            {
                addDeposit(curLayer - 1, etchedSpots, etchMaterial, newTimeOffset);
            }

            grid.set(BitGrid.emptyIntersect(grid, emptySpots));
            if (grid.isEmpty())
            {
                return;
            }
            curLayer--;
        }
    }



    //triggers a liftOff of the photomask, removing it and all deposits above it
    public void liftOff()
    {
        BitGrid grid = new BitGrid();
        grid.set(BitGrid.zeros()); ;
        for (int i = 0; i<=topLayer; i++)
        {
            foreach (GameObject curDeposit in depLayers[i])
            {
                if (curDeposit.GetComponent<meshMaterial>().myMaterial != control.materialType.photoresist)
                {
                    updateDeposit(BitGrid.emptyIntersect(curDeposit.GetComponent<meshGenerator>().grid, grid), curDeposit, i);
                }
            }
            foreach (GameObject curDeposit in depLayers[i])
            {
                if(curDeposit.GetComponent<meshMaterial>().myMaterial == control.materialType.photoresist)
                {
                    grid.set(BitGrid.union(grid, curDeposit.GetComponent<meshGenerator>().grid));
                    updateDeposit(BitGrid.zeros(), curDeposit, i);
                }
            }
        }
    }


    //when running a process, this function lets you select a particular time-step to show all the layers before. Called by the process slider
    public void sliceDeposits(int n)
    {
        int curLayer = 1;
        while (curLayer <= layerCount)
        {
            foreach (GameObject curDeposit in depLayers[curLayer - 1])
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


    //when finishing a process, this function lets deletes all the deposits after the selected time step. Called when you click finish on a process
    public void cullDeposits(int n)
    {
        int curLayer = 1;
        while (curLayer <= layerCount)
        {
            foreach (GameObject curDeposit in depLayers[curLayer - 1])
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
                        updateDeposit(BitGrid.zeros(), curDeposit, curLayer-1);
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
                        updateDeposit(BitGrid.zeros(), curDeposit, curLayer-1);
                    }
                }
            }
            curLayer++;

        }
    }


}
