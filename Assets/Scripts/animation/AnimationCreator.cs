using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCreator : MonoBehaviour
{

    public GameObject SpinCaster;
    public Material photoresist_normal;
    public Material photoresist_comp;
    public GameObject mainCamera;
    public enum states
    {
        standby,
        maskDecending,
        lightOn,
        effectBreak,
        maskAcending
    }

    public states curLightBeamState;
    public GameObject Photomask;
    public GameObject Lightblock;
    public GameObject LightblockTop;
    float lightTime;
    Vector3 startMaskPosition;
    // Start is called before the first frame update
    void Start()
    {
        curLightBeamState = states.standby;
    }

    // Update is called once per frame
    void Update()
    {

        if (curLightBeamState != states.standby)
        {
            lightTime += Time.deltaTime;
            switch (curLightBeamState)
            {
                case states.maskDecending:
                    if (lightTime > 1)
                    {
                        Lightblock.SetActive(true) ;
                        LightblockTop.SetActive(true) ;
                        curLightBeamState = states.lightOn;
                        lightTime = 0;
                    }
                    break;
                case states.lightOn:
                    Color newColor = photoresist_comp.color;
                    newColor.b -= 0.75f*Time.deltaTime;
                    newColor.r -= 0.75f * Time.deltaTime;
                    newColor.g -= 0.75f * Time.deltaTime;
                    photoresist_comp.SetColor("_Color", newColor);
                    if(lightTime > 1.5) {
                        Lightblock.SetActive(false);
                        LightblockTop.SetActive(false);
                        curLightBeamState = states.effectBreak;
                        lightTime = 0;
                    }
                    break;
                case states.effectBreak:
                    if (lightTime > 0.5f)
                    {
                        GameObject.Find("LayerStack").GetComponent<LayerStackHolder>().etchLayer(control.materialType.photoresistComplement);
                        curLightBeamState = states.maskAcending;
                        lightTime = 0;
                        startMaskPosition = Photomask.transform.localPosition;
                        StartCoroutine(zoomIn(0.5f));
                    }
                    break;
                case states.maskAcending:
                    Photomask.transform.SetLocalPositionAndRotation(Photomask.transform.localPosition + new Vector3(0.0f, 20.0f * Time.deltaTime, 0.0f), Photomask.transform.localRotation);
                    if(lightTime > 1.0f)
                    {
                        Photomask.transform.SetLocalPositionAndRotation(startMaskPosition, Photomask.transform.localRotation);
                        Photomask.SetActive(false);
                        curLightBeamState = states.standby;
                        photoresist_comp.SetColor("_Color", photoresist_normal.color);
                    }
                    break;
            }
        }

    }


    public void makeSpinCaster()
    {
        GameObject newSpin = Instantiate(SpinCaster);
        newSpin.transform.position = new Vector3(-9, -27, 19);
        newSpin.transform.rotation = Quaternion.Euler(-90, 0, -90);
        photoresist_comp.color = photoresist_normal.color;
        StartCoroutine(zoomOut(0.25f));
    }



    public void startBeams()
    {
        Photomask.SetActive(true);
        Lightblock.SetActive(true);
        LightblockTop.SetActive(true);
        Photomask.GetComponent<meshGenerator>().grid.set(GameObject.Find("drawing_panel").GetComponent<paint>().grid);
        Photomask.GetComponent<meshGenerator>().initialize();
        Lightblock.GetComponent<meshGenerator>().grid.set(BitGrid.invert(GameObject.Find("drawing_panel").GetComponent<paint>().grid));
        Lightblock.GetComponent<meshGenerator>().initialize(true);
        LightblockTop.GetComponent<meshGenerator>().grid.set(BitGrid.ones());
        LightblockTop.GetComponent<meshGenerator>().initialize(true);
        Lightblock.SetActive(false);
        LightblockTop.SetActive(false);
        lightTime = 0.0f;
        curLightBeamState = states.maskDecending;

    }

    IEnumerator zoomOut(float seconds)
    {
        float time = 0.0f;
        while(time < seconds)
        {
            time += Time.deltaTime;
            mainCamera.GetComponent<OrbitCamera>().distance += (10 / seconds) * Time.deltaTime;
            yield return null;
        }
        mainCamera.GetComponent<OrbitCamera>().distance = 30;
    }

    IEnumerator zoomIn(float seconds)
    {
        float time = 0.0f;
        while (time < seconds)
        {
            time += Time.deltaTime;
            mainCamera.GetComponent<OrbitCamera>().distance -= (10 / seconds) * Time.deltaTime;
            yield return null;
        }
        mainCamera.GetComponent<OrbitCamera>().distance = 20;
    }

}
