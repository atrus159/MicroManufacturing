using UnityEngine;
public class ProcessParent : MonoBehaviour
{
    public int nSteps = 50;
    public int curStep;
    public int prevStep;
    public GameObject layerStack;
    public LayerStackHolder layerStackHold;
    public GameObject slider;
    public GameObject sliderPrefab;
    public GameObject finishButton;
    public GameObject finishButtonPrefab;
    public GameObject cancelButton;
    public GameObject cancelButtonPrefab;

    GameObject Dropdown;
    GameObject DepositButton;
    GameObject EtchButton;
    GameObject PhotoresistButton;
    GameObject LiftoffButton;
    GameObject wetEtchToggle;
    GameObject schematicToggle;

    GameObject DropdownPreset;
    GameObject DepositPreset;
    GameObject EtchPreset;
    GameObject PhotoresistPreset;
    GameObject LiftoffPreset;
    GameObject WetEtchPreset;
    GameObject SchematicPreset;


    public bool[] getButtonsToReactivate()
    {
        return new bool[] { Dropdown, DepositButton, EtchButton, PhotoresistButton, LiftoffButton, wetEtchToggle, schematicToggle};
    }

    public void setButtonsToReactivate(bool[] buttonsToReactivate)
    {
        GameObject canvasMain = GameObject.Find("Canvas - Main");
        GameObject holder = GameObject.Find("PhotoButtonToggleHolder");
        if (buttonsToReactivate[0])
        {
            DropdownPreset = canvasMain.transform.Find("Dropdown").gameObject;
        }
        if (buttonsToReactivate[1])
        {
            DepositPreset = canvasMain.transform.Find("Deposit Button").gameObject;
        }
        if (buttonsToReactivate[2])
        {
            EtchPreset = canvasMain.transform.Find("Etch Button").gameObject;
        }
        if (buttonsToReactivate[3])
        {
            PhotoresistPreset = holder.transform.Find("Photoresist Button").gameObject;
        }
        if (buttonsToReactivate[4])
        {
            LiftoffPreset = holder.transform.Find("Liftoff Button").gameObject;
        }
        if (buttonsToReactivate[5])
        {
            WetEtchPreset = canvasMain.transform.Find("WetEtchToggle").gameObject;
        }
        if (buttonsToReactivate[6])
        {
            SchematicPreset = canvasMain.transform.Find("showSchematicGrid").gameObject;
        }
    }

    public string ErrorMessage;

    // Start is called before the first frame update
    void Start()
    {
        curStep = 1;
        prevStep = curStep;
        layerStack = GameObject.Find("LayerStack");
        layerStackHold = layerStack.GetComponent<LayerStackHolder>();
        int i = 0;

        bool any = false;
        while(i < nSteps)
        {
            any |= CallStep(i);
            i++;
        }

        if (!any)
        {
            GameObject em = GameObject.Find("ErrorManager");
            if (em)
            {
                em.GetComponent<errorManager>().createError(ErrorMessage);
            }
            Destroy(gameObject);
            return;
        }

        layerStackHold.sliceDeposits(1);
        slider = Instantiate(sliderPrefab, transform.position, transform.rotation);
        Transform canvTrans = GameObject.Find("Canvas - Main").transform;
        slider.transform.SetParent(canvTrans, false);
        slider.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,50,0);


        finishButton = Instantiate(finishButtonPrefab, transform.position, transform.rotation);
        finishButton.transform.SetParent(canvTrans, false);
        finishButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, 100, 0);

        cancelButton = Instantiate(cancelButtonPrefab, transform.position, transform.rotation);
        cancelButton.transform.SetParent(canvTrans, false);
        cancelButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, 50, 0);

        if (DropdownPreset)
        {
            Dropdown = DropdownPreset;
        }
        else
        {
            Dropdown = GameObject.Find("Dropdown");
        }
        if(DepositPreset)
        {
            DepositButton = DepositPreset;
        }
        else
        {
            DepositButton = GameObject.Find("Deposit Button");
        }
        if (EtchPreset)
        {
            EtchButton = EtchPreset;
        }
        else
        {
            EtchButton = GameObject.Find("Etch Button");
        }
        if (PhotoresistPreset)
        {
            PhotoresistButton = PhotoresistPreset;
        }
        else
        {
            PhotoresistButton = GameObject.Find("Photoresist Button");
        }
        if (LiftoffPreset)
        {
            LiftoffButton = LiftoffPreset;
        }
        else
        {
            LiftoffButton = GameObject.Find("Liftoff Button");
        }
        if (WetEtchPreset)
        {
            wetEtchToggle= WetEtchPreset;
        }
        else
        {
            wetEtchToggle = GameObject.Find("WetEtchToggle");
        }
        if (SchematicPreset)
        {
            schematicToggle = SchematicPreset;
        }
        else {
            schematicToggle = GameObject.Find("showSchematicGrid");
        }


        if (Dropdown)
        {
            Dropdown.SetActive(false);
        }
        if(DepositButton)
        {
            DepositButton.SetActive(false);
        }
        if (EtchButton)
        {
            EtchButton.SetActive(false);
        }
        if (PhotoresistButton)
        {
            PhotoresistButton.SetActive(false);
        }
        if (LiftoffButton)
        {
            LiftoffButton.SetActive(false);
        }
        if (wetEtchToggle)
        {
            wetEtchToggle.SetActive(false);
        }
        if (schematicToggle) {
            schematicToggle.SetActive(false);
        }
        GameObject.Find("Control").GetComponent<control>().setShowMeasureStick(true);

    }

    virtual public bool CallStep(int i)
    {
        return true;
    }
    
    virtual public void UpdateSchematics() {}


    virtual public void OnValueChanged(float newValue)
    {
    }

    public void onCancelButton()
    {
        layerStackHold.cullDeposits(0);
        Destroy(slider);
        Destroy(finishButton);
        Destroy(cancelButton);
        if (Dropdown)
        {
            Dropdown.SetActive(true);
        }
        if (DepositButton)
        {
            DepositButton.SetActive(true);
        }
        if (EtchButton)
        {
            EtchButton.SetActive(true);
        }
        if (PhotoresistButton)
        {
            PhotoresistButton.SetActive(true);
        }
        if (LiftoffButton)
        {
            LiftoffButton.SetActive(true);
        }
        if (wetEtchToggle)
        {
            wetEtchToggle.SetActive(true);
        }
        if (schematicToggle)
        {
            schematicToggle.SetActive(true);
        }
        GameObject.Find("Control").GetComponent<control>().setShowMeasureStick(false);
        Destroy(gameObject);
    }
    public void onFinishedButton()
    {
        layerStackHold.cullDeposits(curStep + 1);
        Destroy(slider);
        Destroy(finishButton);
        Destroy(cancelButton);

        this.UpdateSchematics();

        if (Dropdown)
        {
            Dropdown.SetActive(true);
        }
        if (DepositButton)
        {
            DepositButton.SetActive(true);
        }
        if (EtchButton)
        {
            EtchButton.SetActive(true);
        }
        if (PhotoresistButton)
        {
            PhotoresistButton.SetActive(true);
        }
        if (LiftoffButton)
        {
            LiftoffButton.SetActive(true);
        }
        if (wetEtchToggle)
        {
            wetEtchToggle.SetActive(true);
        }
        if (schematicToggle)
        {
            schematicToggle.SetActive(true);
        }
        layerStackHold.postDeleteCheckFlag = true;
        GameObject.Find("Control").GetComponent<control>().setShowMeasureStick(false);
        Destroy(gameObject);
    }
}
