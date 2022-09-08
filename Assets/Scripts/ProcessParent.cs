using System.Collections;
using System.Collections.Generic;
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
    public GameObject button;
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        curStep = 1;
        prevStep = curStep;
        layerStack = GameObject.Find("LayerStack");
        layerStackHold = layerStack.GetComponent<LayerStackHolder>();
        int i = 0;
        while(i < nSteps)
        {
            CallStep(i);
            i++;
        }
        layerStackHold.sliceDeposits(1);
        slider = Instantiate(sliderPrefab, transform.position, transform.rotation);
        Transform canvTrans = GameObject.Find("Canvas - Main").transform;
        slider.transform.SetParent(canvTrans, false);
        slider.transform.SetPositionAndRotation(new Vector2(canvTrans.position.x + 250, canvTrans.position.y - 100), transform.rotation);

        button = Instantiate(buttonPrefab, transform.position, transform.rotation);
        button.transform.SetParent(canvTrans, false);
        button.transform.SetPositionAndRotation(new Vector2(canvTrans.position.x + 350, canvTrans.position.y - 150), transform.rotation);

    }

    virtual public void CallStep(int i)
    {

    }


    virtual public void OnValueChanged(float newValue)
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            layerStackHold.cullDeposits(0);
            Destroy(slider);
            Destroy(button);
            Destroy(gameObject);
        }
    }

    public void onFinishedButton()
    {
        layerStackHold.cullDeposits(curStep + 1);
        Destroy(slider);
        Destroy(button);
        Destroy(gameObject);
    }
}
