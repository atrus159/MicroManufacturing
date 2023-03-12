using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class control : MonoBehaviour
{
    public enum pauseStates 
    {
        unPaused,
        tutorialPaused,
        menuPaused

    }

    public pauseStates paused;


    public bool hudVisible;
    public bool tutorialBlockerVisible;
    bool prevTutorialBlockerVisible;
    int displayDelayTime;

    bool showMeasureSticks;
    GameObject ms1;
    GameObject ms2;
    GameObject ms3;
    GameObject ms4;
    int curRegion;
    int offset;
    public struct materialData
    {
        public materialData(Material m, int ef)
        {
            material = m;
            etchFlag = ef;
        }

        public Material material { get; }
        public int etchFlag { get; }

    }

    public Material m_gold;
    public Material m_chromium;
    public Material m_aluminum;
    public Material m_photoresist;
    public Material m_silicon;
    public Material m_silicondioxide;

    public enum materialType
    {
        gold,
        chromium,
        aluminum,
        photoresist,
        silicon,
        silicondioxide,
        empty,
        photoresistComplement
    }

    public GameObject PhotoResistEdge;
    pauseStates prevPaused;

    public static Dictionary<materialType, materialData> materialsList = new Dictionary<materialType, materialData>();
    // Start is called before the first frame update
    void Start()
    {
        paused = pauseStates.unPaused;
        materialsList.Add(materialType.gold, new materialData(m_gold, 0));
        materialsList.Add(materialType.chromium, new materialData(m_chromium, 0));
        materialsList.Add(materialType.aluminum, new materialData(m_aluminum, 0));
        materialsList.Add(materialType.photoresist, new materialData(m_photoresist, 0));
        materialsList.Add(materialType.silicon, new materialData(m_silicon, 0));
        materialsList.Add(materialType.silicondioxide, new materialData(m_silicondioxide, 0));
        materialsList.Add(materialType.photoresistComplement, new materialData(m_photoresist, 0));
        hudVisible = false;
        tutorialBlockerVisible = false;
        prevTutorialBlockerVisible = false;
        displayDelayTime = 0;
        if (control.tutorialExists())
        {
            tutorialBlockerVisible = true;
            displayDelayTime = 10;
        }
        if (!control.tutorialExists())
        {
            GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        showMeasureSticks = false;
        ms1 = GameObject.Find("measure stick 1");
        ms2 = GameObject.Find("measure stick 2");
        ms3 = GameObject.Find("measure stick 3");
        ms4 = GameObject.Find("measure stick 4");
        ms1.SetActive(false);
        ms2.SetActive(false);
        ms3.SetActive(false);
        ms4.SetActive(false);
        curRegion = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && paused == pauseStates.unPaused) // prevents overlapping tab menu and pause menu
        {
            hudVisible = !hudVisible;
            if (!hudVisible)
            {
                setMainActive(true);
                setHudActive(false);
            }
            else
            {
               setMainActive(false);
               setHudActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // if the escape is pressed

        {
            if(paused != pauseStates.menuPaused) // if it is not paused, paused the game
            {
                setHudActive(false);
                setMainActive(false);
                setPauseMenuActive(true);
                GameObject.Find("Substrate").GetComponent<substrateControl>().mainCam.SetActive(true); // sets main camera
            }
            else // if it is paused, unpause the game
            {
                if (!hudVisible)
                {
                    setMainActive(true);
                    setHudActive(false);
                }
                else
                {
                    setMainActive(false);
                    setHudActive(true);
                }
                setPauseMenuActive(false);
            }
      

        }


        if (showMeasureSticks)
        {

            float cameraAngle = GameObject.Find("Main Camera").transform.rotation.eulerAngles.y;

            switch (curRegion)
            {
                case 0:
                    checkRegion(1, cameraAngle);
                    checkRegion(3, cameraAngle);
                    break;
                case 1:
                    checkRegion(0, cameraAngle);
                    checkRegion(2, cameraAngle);
                    break;
                case 2:
                    checkRegion(1, cameraAngle);
                    checkRegion(3, cameraAngle);
                    break;
                case 3:
                    checkRegion(0, cameraAngle);
                    checkRegion(2, cameraAngle);
                    break;
            }
        }
    }

    void checkRegion(int region, float cameraAngle)
    {
        switch (region)
        {
            case 0:
                if (cameraAngle < offset || cameraAngle > 270 + offset)
                {
                    ms1.SetActive(true);
                    ms2.SetActive(false);
                    ms3.SetActive(false);
                    ms4.SetActive(false);
                    curRegion = 0;
                }
            break;
            case 1:
                if (cameraAngle < 270 + offset && cameraAngle > 180 + offset)
                {
                    ms1.SetActive(false);
                    ms2.SetActive(true);
                    ms3.SetActive(false);
                    ms4.SetActive(false);
                    curRegion = 1;
                }
            break;
            case 2:
                if (cameraAngle < 180 + offset && cameraAngle > 90 + offset)
                {
                    ms1.SetActive(false);
                    ms2.SetActive(false);
                    ms3.SetActive(false);
                    ms4.SetActive(true);
                    curRegion = 2;
                }
            break;
            case 3:
                if (cameraAngle < 90 + offset && cameraAngle > offset)
                {
                    ms1.SetActive(false);
                    ms2.SetActive(false);
                    ms3.SetActive(true);
                    ms4.SetActive(false);
                    curRegion = 3;
                }
            break;
        }
    }


    public void setShowMeasureStick(bool val)
    {
        if (val)
        {
            showMeasureSticks = true;
            float cameraAngle = GameObject.Find("Main Camera").transform.rotation.eulerAngles.y;
            checkRegion(1, cameraAngle);
            checkRegion(2, cameraAngle);
            checkRegion(3, cameraAngle);
            checkRegion(4, cameraAngle);
        }
        else
        {
            showMeasureSticks = false;
        }
    }
    private void setHudActive(bool status)
    {
        if (status) 
        {
            GameObject.Find("Canvas - HUD").GetComponent<CanvasGroup>().blocksRaycasts = true;
            GameObject.Find("Canvas - HUD").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Substrate").GetComponent<substrateControl>().subCam.SetActive(true);
        }
        else 
        {
            GameObject.Find("Canvas - HUD").GetComponent<CanvasGroup>().blocksRaycasts = false;
            GameObject.Find("Canvas - HUD").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Substrate").GetComponent<substrateControl>().subCam.SetActive(false);
        }
    }

    private void setMainActive(bool status)
    {
        if (status)
        {
            GameObject.Find("Canvas - Main").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Substrate").GetComponent<substrateControl>().mainCam.SetActive(true);
        }
        else 
        {
            GameObject.Find("Canvas - Main").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Substrate").GetComponent<substrateControl>().mainCam.SetActive(false);
        }
    }

    private void setPauseMenuActive(bool status)
    {
        if (status)
        {
            GameObject.Find("Canvas - Pause Menu").GetComponent<CanvasGroup>().alpha = 1; // make it appear
            GameObject.Find("Canvas - Pause Menu").GetComponent<CanvasGroup>().blocksRaycasts = true; // when you click on it, it will click on the first it touches
            prevPaused = paused;
            setPaused(pauseStates.menuPaused);
        }
        else
        {
            GameObject.Find("Canvas - Pause Menu").GetComponent<CanvasGroup>().alpha = 0; // make it disappear 
            GameObject.Find("Canvas - Pause Menu").GetComponent<CanvasGroup>().blocksRaycasts = false; // cannot click on it

            setPaused(prevPaused);
        }
    }


    private void LateUpdate()
    {
        if (prevTutorialBlockerVisible != tutorialBlockerVisible)
        {
            displayDelayTime++;
            if(displayDelayTime >= 5)
            {
                if(displayDelayTime < 10)
                {
                    if (tutorialBlockerVisible)
                    {
                        GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().alpha = (displayDelayTime - 5.0f)/10.0f;
                    }
                    else
                    {
                        GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().alpha = (10.0f - displayDelayTime) / 10.0f;
                    }

                }
                else
                {
                    if (tutorialBlockerVisible)
                    {
                        GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().alpha = 0.5f;
                        GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    else
                    {
                        GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().alpha = 0;
                        GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>().blocksRaycasts = false;
                    }
                    displayDelayTime = 0;
                    prevTutorialBlockerVisible = tutorialBlockerVisible;
                }
            }
        }
    }

    public void OnValueChanged(float newValue)
    {
        GameObject proc = GameObject.Find("New Process");
        proc.GetComponent<ProcessParent>().OnValueChanged(newValue);
    }
    public void onDropDownChanged()
    {
        int num = GameObject.Find("Dropdown").GetComponent<DropdownCustom>().value;
        GameObject layer = GameObject.Find("LayerStack");
        layer.GetComponent<LayerStackHolder>().onValueChange(num);
    }

    public void onDepositButton()
    {
        GameObject layer = GameObject.Find("LayerStack");
        layer.GetComponent<LayerStackHolder>().startDepositProcess();
    }

    public void onEtchButton()
    {
        GameObject layer = GameObject.Find("LayerStack");
        layer.GetComponent<LayerStackHolder>().startEtchProcess();
    }

    public void onPhotoResistButton()
    {
        GameObject layer = GameObject.Find("Animation Creator");
        layer.GetComponent<AnimationCreator>().makeSpinCaster();
    }

    public void onFinishedButton()
    {
        GameObject proc = GameObject.Find("New Process");
        proc.GetComponent<ProcessParent>().onFinishedButton();
    }

    public void onCancelButton()
    {
        GameObject proc = GameObject.Find("New Process");
        proc.GetComponent<ProcessParent>().onCancelButton();
    }

    public static bool tutorialExists()
    {
        return GameObject.Find("Tutorial");
    }
    
    public static pauseStates isPaused()
    {
        GameObject myself = GameObject.Find("Control");
        return myself.GetComponent<control>().paused;
    }

    public static void setPaused(pauseStates newPaused)
    {
        GameObject myself = GameObject.Find("Control");
        myself.GetComponent<control>().paused = newPaused;
    }

}
