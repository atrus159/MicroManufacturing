using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//
public class LayerStackHolder : MonoBehaviour
{
    
    public static int layerCount;

    //an array of all of the layers. each index is a list of deposits at that layer
    public List<GameObject>[] dep_layers;

    //the current highest layer reached by anything in the design. Deposits will start one above here
    public int topLayer;

    //prefabs
    public GameObject meshGenPrefab;
    public GameObject layerStackPrefab;
    public GameObject processGenPrefab;
    public GameObject processEtchPrefab;
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
        dep_layers = new List<GameObject>[layerCount].Select(item => new List<GameObject>()).ToArray();
        topLayer = -1;
        layerHeight = 0.1f;
        curMaterial = control.materialType.chromium;
        deletedLayers = new List<int>();
        deletedFlag = false;
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
        Instantiate(processEtchPrefab, transform.position, transform.rotation).gameObject.name = "New Process";
    }


    //deposits marked for deletion are only cleared at the end of each game step so they don't interfere with other functions
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


    //insterts a deposit of a material into a particular layer
    bool addDeposit(int curlayer, bitMap toDeposit, control.materialType layerMaterial, int newTimeOffset = 0)
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
        dep_layers[curlayer].Add(newMesh);

        return true;
    }

    //sets the bitMap of a deposit to a new value, or destroys it if set to empty
    void updateDeposit(bitMap grid, GameObject deposit, int depLayer)
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
    public void depositLayer(control.materialType layerMaterial, bitMap inputGrid, int newTimeOffset = 0)
    {
        bitMap grid = new bitMap();
        grid.set(inputGrid);
        int curLayer = topLayer + 1;
        while(curLayer > 0)
        {
            bitMap thisDeposit = new bitMap();
            thisDeposit.set(grid);
            bitMap tempDeposit = new bitMap();
            tempDeposit.set(bitMap.zeros());

            foreach (GameObject curDeposit in dep_layers[curLayer-1])
            {
                tempDeposit.set(bitMap.union(tempDeposit, curDeposit.GetComponent<meshGenerator>().grid));
            }
            thisDeposit.set(bitMap.intersect(tempDeposit, thisDeposit));
            if (!thisDeposit.isEmpty())
            {
                addDeposit(curLayer, thisDeposit, layerMaterial, newTimeOffset);
            }
            foreach (GameObject curDeposit in dep_layers[curLayer-1])
            {
                grid.set(bitMap.emptyIntersect(grid, curDeposit.GetComponent<meshGenerator>().grid));
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
        bitMap grid = new bitMap() ;
        grid.set(bitMap.ones());
        int curLayer = topLayer + 1;
        while (curLayer > 0)
        {
            bitMap emptySpots = new bitMap();
            bitMap etchedSpots = new bitMap();
            emptySpots.set(bitMap.zeros());
            etchedSpots.set(bitMap.zeros());
            foreach (GameObject curDeposit in dep_layers[curLayer - 1])
            {
                if(curDeposit.GetComponent<meshMaterial>().timeOffset >= 0 || (curDeposit.GetComponent<meshMaterial>().timeOffset < 0 && curDeposit.GetComponent<meshMaterial>().timeOffset >= newTimeOffset && curDeposit.GetComponent<meshMaterial>().myMaterial != etchMaterial)){
                    emptySpots.set(bitMap.union(emptySpots, curDeposit.GetComponent<meshGenerator>().grid));
                }
            }
            bool anyFlag = false;
            foreach (GameObject curDeposit in dep_layers[curLayer - 1])
            {
                if(curDeposit.GetComponent<meshMaterial>().myMaterial == etchMaterial)
                {
                    if(curDeposit.GetComponent<meshMaterial>().timeOffset >= 0){
                        if(newTimeOffset < 0){
                            etchedSpots.set(bitMap.union(bitMap.intersect(curDeposit.GetComponent<meshGenerator>().grid,grid),etchedSpots));
                            anyFlag = true;
                        }
                        updateDeposit(bitMap.emptyIntersect(curDeposit.GetComponent<meshGenerator>().grid, grid), curDeposit, curLayer-1);

                    }
                }
            }

            if (anyFlag && !etchedSpots.isEmpty()){
                addDeposit(curLayer -1 , etchedSpots, etchMaterial, newTimeOffset);
            }
            grid.set(bitMap.emptyIntersect(grid, emptySpots));
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
        bitMap grid = new bitMap();
        grid.set(bitMap.zeros()); ;
        for (int i = 0; i<=topLayer; i++)
        {
            foreach (GameObject curDeposit in dep_layers[i])
            {
                if (curDeposit.GetComponent<meshMaterial>().myMaterial != control.materialType.photoresist)
                {
                    updateDeposit(bitMap.emptyIntersect(curDeposit.GetComponent<meshGenerator>().grid, grid), curDeposit, i);
                }
            }
            foreach (GameObject curDeposit in dep_layers[i])
            {
                if(curDeposit.GetComponent<meshMaterial>().myMaterial == control.materialType.photoresist)
                {
                    grid.set(bitMap.union(grid, curDeposit.GetComponent<meshGenerator>().grid));
                    updateDeposit(bitMap.zeros(), curDeposit, i);
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


    //when finishing a process, this function lets deletes all the deposits after the selected time step. Called when you click finish on a process
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
                        updateDeposit(bitMap.zeros(), curDeposit, curLayer-1);
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
                        updateDeposit(bitMap.zeros(), curDeposit, curLayer-1);
                    }
                }
            }
            curLayer++;
        }
    }

}
