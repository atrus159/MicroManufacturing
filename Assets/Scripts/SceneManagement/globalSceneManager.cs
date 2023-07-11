using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class globalSceneManager : MonoBehaviour
{
    public GameObject blueprintPrefab;

    string continueScene;
    string targetSceneFromLevel;
    public bool optionsMenuFlag;

    public struct layerData
    {
        public BitGrid grid;
        public control.materialType materialType;
        public layerData(BitGrid girdNew, control.materialType materialTypeNew)
        {
            grid = girdNew;
            materialType = materialTypeNew;
        }
    }

    public struct levelState
    {
        public List<layerData> layerDatas;
        public int dialogueIndex;
        public int requirementIndex;
        public bool requirementClear;

        public bool[] activateFlags;

        public enum processType
        {
            none,
            deposit,
            etch,
            wetEtch
        }

        public processType curProcess;

        public bool[] buttonsToReactivate;

        public int material;


        public Sprite blueprintSprite;
        public string blueprintName;

        public levelState(int dIndex, int reqIndex, bool reqClear, bool[] acFlags, processType process, bool[] reactivateButtons, int materialInd)
        {
            dialogueIndex= dIndex;
            requirementIndex= reqIndex;
            requirementClear= reqClear;
            activateFlags = new bool[9];
            activateFlags[0] = acFlags[0]; // deposit
            activateFlags[1] = acFlags[1]; // etch
            activateFlags[2] = acFlags[2]; // photoresist
            activateFlags[3] = acFlags[3]; // liftoff
            activateFlags[4] = acFlags[4]; // dropdown
            activateFlags[5] = acFlags[5]; //wet-etch
            activateFlags[6] = acFlags[6]; //draw
            activateFlags[7] = acFlags[7]; //back
            activateFlags[8] = acFlags[8]; //blueprint
            layerDatas = new List<layerData>();
            curProcess = process;
            buttonsToReactivate = new bool[6];
            buttonsToReactivate[0] = reactivateButtons[0]; // dropdown
            buttonsToReactivate[1] = reactivateButtons[1]; //deposit
            buttonsToReactivate[2] = reactivateButtons[2]; //etch
            buttonsToReactivate[3] = reactivateButtons[3]; //photoresist
            buttonsToReactivate[4] = reactivateButtons[4]; //liftoff
            buttonsToReactivate[5] = reactivateButtons[5]; //wet-etch
            material = materialInd;

            blueprintSprite = null;
            blueprintName = "";
        }

    }

    bool loadFlag = false;

    public levelState curState;

    List<List<layerData>> undoStates;
    List<List<layerData>> redoStates;
    List<layerData> curUndoState; 



    private void Update()
    {
        if (loadFlag && GameObject.Find("LayerStack"))
        {
            loadSceneData();
            loadFlag = false;
        }

        if (GameObject.Find("Control") && !GameObject.Find("Control").GetComponent<control>().hudVisible)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                //{
                    undoState();
                //}
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                //if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                //{
                    redoState();
                //}
            }
        }
    }

    public void undoState()
    {
        if(undoStates.Count > 0)
        {
            redoStates.Add(curUndoState);
            curUndoState = undoStates[undoStates.Count - 1];
            undoStates.RemoveAt(undoStates.Count-1);
            applyLayerData(curUndoState);
            
        }
    }

    public void saveState()
    {
        undoStates.Add(curUndoState);
        curUndoState = new List<layerData>();
        redoStates.Clear();
        updateLayerData(curUndoState);
    }

    public void redoState()
    {
        if (redoStates.Count > 0)
        {
            undoStates.Add(curUndoState);
            curUndoState = redoStates[redoStates.Count - 1];
            redoStates.RemoveAt(redoStates.Count - 1);
            applyLayerData(curUndoState);
        }
    }


    private void Start()
    {
        targetSceneFromLevel = "";
        gameObject.name = "Global Scene Manager";
        DontDestroyOnLoad(gameObject);
        continueScene = "Level1";
        curState = new levelState(-1, -1, false, new bool[9], levelState.processType.none, new bool[6],-1);
        undoStates = new List<List<layerData>>();
        redoStates = new List<List<layerData>>();
        curUndoState = new List<layerData>();
    }

    public void gotoMenuFromLevel(string target)
    {
        continueScene = SceneManager.GetActiveScene().name;
        saveSceneData();
        SceneManager.LoadScene(target);
    }

    public void continueFromMenu()
    {
        SceneManager.LoadScene(continueScene);
        continueScene = "Level1";
        loadFlag= true;
    }

    public void gotoFromLevel(string target)
    {
        targetSceneFromLevel = target;
        continueScene = target;
        SceneManager.LoadScene("PostLevel");
        undoStates.Clear();
        redoStates.Clear();
        curUndoState = new List<layerData>();
        curState = new levelState(-1, -1, false, new bool[9], levelState.processType.none, new bool[6], -1);
    }

    public void gotoFromMenu(string targetScene = "_None_")
    {
        undoStates.Clear();
        redoStates.Clear();
        curUndoState = new List<layerData>();
        curState = new levelState(-1, -1, false, new bool[9], levelState.processType.none, new bool[6], -1);
        if(targetScene == "_None_")
        {
            SceneManager.LoadScene(targetSceneFromLevel);
        }
        else
        {
            SceneManager.LoadScene(targetScene);
        }
        continueScene = "Level1";
    }


    public void saveSceneData()
    {
        int dialogueIndex = TextManager.instance.i;
        levelRequirementManager lrm = GameObject.Find("Level Requirement Manager").GetComponent<levelRequirementManager>();
        int requirementIndex = lrm.curIndex;
        bool requirementClear = lrm.curClear;


        bool depositFlag = GameObject.Find("Deposit Button");
        bool etchFlag = GameObject.Find("Etch Button");
        bool photoresistFlag = GameObject.Find("Photoresist Button");
        bool liftoffFlag = GameObject.Find("Liftoff Button");
        bool drawFlag = GameObject.Find("DrawButton");
        bool backFlag = GameObject.Find("DrawButtonBack");
        bool dropdownFlag = GameObject.Find("Canvas - Main").transform.Find("Dropdown").gameObject.GetComponent<DropdownCustom>().visible;
        bool wetEtchFlag = GameObject.Find("WetEtchToggle");
        bool blueprintFlag = GameObject.Find("blueprint(Clone)");

        levelState.processType curProcess = levelState.processType.none;
        GameObject process = GameObject.Find("New Process");
        bool[] buttonsToReactivate = new bool[6];


        if (process)
        {
            if (process.GetComponent<ProcessGen>())
            {
                curProcess = levelState.processType.deposit;
            }else if(process.GetComponent<ProcessEtch>())
            {
                curProcess = levelState.processType.etch;
            }
            else if (process.GetComponent<ProcessWetEtch>())
            {
                curProcess = levelState.processType.wetEtch;
            }
            buttonsToReactivate = process.GetComponent<ProcessParent>().getButtonsToReactivate();
        }

        int material = GameObject.Find("Canvas - Main").transform.Find("Dropdown").gameObject.GetComponent<DropdownCustom>().getCurElement();

        curState = new levelState(dialogueIndex, requirementIndex, requirementClear, new bool[] { depositFlag, etchFlag, photoresistFlag, liftoffFlag, dropdownFlag, wetEtchFlag, drawFlag, backFlag, blueprintFlag }, curProcess, buttonsToReactivate, material);

        if (blueprintFlag)
        {
            GameObject print = GameObject.Find("blueprint(Clone)");
            curState.blueprintName = print.GetComponentInChildren<TextMeshProUGUI>().text;
            curState.blueprintSprite = print.transform.GetChild(0).GetComponent<Image>().sprite;
        }
        else
        {
            curState.blueprintSprite = null;
            curState.blueprintName = "";
        }

        updateLayerData(curState.layerDatas);
    }


    void updateLayerData(List<layerData> toUpdate)
    {
        toUpdate.Clear();
        LayerStackHolder ls = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
        for (int i = 0; i <= ls.topLayer; i++)
        {
            foreach (GameObject curDeposit in ls.depLayers[i])
            {
                if (curDeposit.GetComponent<meshMaterial>().timeOffset == 0)
                {
                    toUpdate.Add(new layerData(curDeposit.GetComponent<meshGenerator>().grid, curDeposit.GetComponent<meshMaterial>().myMaterial));
                }
            }
        }
    }

    void applyLayerData(List<layerData> toApply)
    {
        LayerStackHolder ls = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
        GameObject process = GameObject.Find("New Process");
        if (process)
        {
            process.GetComponent<ProcessParent>().onCancelButton();
        }
        ls.clear();
        foreach (layerData newDeposit in toApply)
        {
            ls.depositLayer(newDeposit.materialType, newDeposit.grid);
        }
    }

    public void loadSceneData()
    {
        applyLayerData(curState.layerDatas);
        GameObject.Find("Level Requirement Manager").GetComponent<levelRequirementManager>().setToIndex(curState.requirementIndex, curState.requirementClear);

        GameObject MainCanvas = GameObject.Find("Canvas - Main");
        GameObject HudCanvas = GameObject.Find("Canvas - HUD");
        GameObject Holder = GameObject.Find("PhotoButtonToggleHolder");


        MainCanvas.transform.Find("Deposit Button").gameObject.SetActive(curState.activateFlags[0]);
        MainCanvas.transform.Find("Etch Button").gameObject.SetActive(curState.activateFlags[1]);
        Holder.transform.Find("Photoresist Button").gameObject.SetActive(curState.activateFlags[2]);
        Holder.transform.Find("Liftoff Button").gameObject.SetActive(curState.activateFlags[3]);
        GameObject.Find("Dropdown").GetComponent<Image>().enabled = curState.activateFlags[4];
        GameObject.Find("Dropdown").GetComponent<DropdownCustom>().visible = curState.activateFlags[4];
        MainCanvas.transform.Find("WetEtchToggle").gameObject.SetActive(curState.activateFlags[5]);
        MainCanvas.transform.Find("DrawButton").gameObject.SetActive(curState.activateFlags[6]);
        HudCanvas.transform.Find("DrawButtonBack").gameObject.SetActive(curState.activateFlags[7]);
        if (curState.activateFlags[8])
        {
            GameObject newPrint = Instantiate(blueprintPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newPrint.transform.parent = GameObject.Find("Canvas - Tutorial Text").transform;
            newPrint.GetComponentInChildren<TextMeshProUGUI>().text = curState.blueprintName;
            newPrint.transform.GetChild(0).GetComponent<Image>().sprite = curState.blueprintSprite;
            newPrint.transform.position = new Vector3(UnityEngine.Screen.width / 2, UnityEngine.Screen.height / 2, 0);
        }

        GameObject.Find("Canvas - Main").transform.Find("Dropdown").gameObject.GetComponent<DropdownCustom>().initialize();


        LayerStackHolder ls = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();

        switch (curState.curProcess)
        {
            case levelState.processType.none:
                break;
            case levelState.processType.deposit:
                ls.startDepositProcess();
                break;
            case levelState.processType.etch:
                ls.startEtchProcess();
                break;
            case levelState.processType.wetEtch:
                ls.wetEtch = true;
                ls.startEtchProcess();
                break;
            default:
                break;
        }
        if(curState.curProcess != levelState.processType.none)
        {
            GameObject.Find("New Process").GetComponent<ProcessParent>().setButtonsToReactivate(curState.buttonsToReactivate);
        }
    }


}
