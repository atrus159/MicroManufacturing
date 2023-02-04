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
        empty
    }

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
        if (Input.GetKeyDown(KeyCode.M))
        {
            BitGrid test = BitGrid.circle();
            string output = test.printGrid();
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
            setPaused(pauseStates.menuPaused);
        }
        else
        {
            GameObject.Find("Canvas - Pause Menu").GetComponent<CanvasGroup>().alpha = 0; // make it disappear 
            GameObject.Find("Canvas - Pause Menu").GetComponent<CanvasGroup>().blocksRaycasts = false; // cannot click on it

            setPaused(pauseStates.unPaused);
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
        GameObject layer = GameObject.Find("AnimationCreator");
        layer.GetComponent<AnimationCreator>().makeSpinCaster();
    }

    public void onFinishedButton()
    {
        GameObject proc = GameObject.Find("New Process");
        proc.GetComponent<ProcessParent>().onFinishedButton();
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
