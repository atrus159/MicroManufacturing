using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGTespy.UI;
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


        Dropdown = GameObject.Find("Dropdown");
        DepositButton = GameObject.Find("Deposit Button");
        EtchButton = GameObject.Find("Etch Button");
        PhotoresistButton = GameObject.Find("Photoresist Button");
        LiftoffButton = GameObject.Find("Liftoff Button");


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
        GameObject.Find("Control").GetComponent<control>().setShowMeasureStick(true);

    }

    virtual public bool CallStep(int i)
    {
        return true;
    }


    virtual public void OnValueChanged(float newValue)
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onCancelButton();
        }
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
        GameObject.Find("Control").GetComponent<control>().setShowMeasureStick(false);
        Destroy(gameObject);
    }
    public void onFinishedButton()
    {
        layerStackHold.cullDeposits(curStep + 1);
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
        layerStackHold.postDeleteCheckFlag = true;
        GameObject.Find("Control").GetComponent<control>().setShowMeasureStick(false);
        Destroy(gameObject);
    }
}
