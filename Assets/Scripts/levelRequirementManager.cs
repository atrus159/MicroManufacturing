using CGTespy.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class levelRequirementManager : MonoBehaviour
{
    //A list of scripts for each of the level requirements
    //public List<MonoScript> requirementScripts = new List<MonoScript>();
    //public List<MonoScript> requirementReserveScripts = new List<MonoScript>();
    public List<int> requirementReserveBlocks;
    int reserveIndex;

    //A list of instances of the classes in the scripts provided in requirementScripts. This will be filled out in start
    public List<levelRequirementParent> requirements = new List<levelRequirementParent>();
    public List<levelRequirementParent> requirementReserves = new List<levelRequirementParent>();

    List<GameObject> requirementDisplayInstances;

    //the requirement display, where all the requirements and their completion status can be viewed in game
    public GameObject display;
    //the prefab for one index in the requirement display
    public GameObject requirementPrefab;


    float displayOffset = 105.0f;

    int count;

    void Start()
    {
        requirementDisplayInstances = new List<GameObject>();
        reserveIndex = 0;
        //instantiates all of the classes in requirementScripts and puts them in requirements
        /*foreach(MonoScript curScript in requirementScripts)
        {
            System.Type[] lsType = { typeof(LayerStackHolder) };
            System.Type curType = curScript.GetClass();
            ConstructorInfo curConstructor = curType.GetConstructor(lsType);
            object[] constructorArguments = { GameObject.Find("LayerStack").GetComponent<LayerStackHolder>() };
            object newRequirement = curConstructor.Invoke(constructorArguments);
            requirements.Add((levelRequirementParent) newRequirement);
        }*/
        updateDisplay();
        
    }


    //fills out the requirement display with an instance of requirementPrefab for each requirement. Gives them the correct display name, discription and color signifying their completion status.
    void updateDisplay()
    {
        foreach (GameObject curDisplayObj in requirementDisplayInstances)
        {
            Destroy(curDisplayObj);
        }
        requirementDisplayInstances.Clear();
        count = 0;

        int index = 0;

        foreach (levelRequirementParent curRequirement in requirements)
        {
            makePrefab(-index * displayOffset, curRequirement.name, curRequirement.description);
            index++;
        }
    }

    public void hideDisplay()
    {
        display.SetActive(false);
    }

    public void startDisplay()
    {
        display.SetActive(true);
    }

    void makePrefab(float height, string name, string description)
    {
        count++;
        GameObject newDisplayObj = Instantiate(requirementPrefab);
        newDisplayObj.transform.SetParent(display.transform);
        newDisplayObj.transform.SetPositionAndRotation(display.transform.position + new Vector3(0, height, 0), Quaternion.identity);
        newDisplayObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = name;
        newDisplayObj.transform.GetComponent<requirementIcon>().description = description;
        newDisplayObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = count.ToString();
        newDisplayObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
        requirementDisplayInstances.Add(newDisplayObj);
        newDisplayObj.transform.SetAsFirstSibling();
    }


    public void addReserve()
    {
        if(reserveIndex >= requirementReserveBlocks.Count)
        {
            return;
        }
        int amount = requirementReserveBlocks[reserveIndex];

        for(int i = 0; i<amount; i++)
        {   
            if(requirementReserves.Count == 0)
            {
                break;
            }
            levelRequirementParent newRequirement = requirementReserves[0];
            requirements.Add(newRequirement);
            requirementReserves.RemoveAt(0);

        }
        reserveIndex++;

        updateDisplay();

    }

    // Update is called once per frame
    void Update()
    {
        checkRequirements(true);
        if (display && Input.GetMouseButtonDown(0) && requirements.Count >0)
        {
            float mouseX = UnityEngine.Input.mousePosition.x;
            float mouseY = UnityEngine.Input.mousePosition.y;
            RectTransform trans = display.GetComponent<RectTransform>();




            float width = requirementPrefab.GetComponent<RectTransform>().Width()/2;
            float height = requirementPrefab.GetComponent<RectTransform>().Height()/2;

            float x1 = trans.position.x;
            float y1 = trans.position.y;
            float x2 = x1 - width;
            float y2 = y1 - displayOffset*(requirements.Count);

            bool hoverOver = false;
            if (mouseX < x1 && mouseX > x2 && mouseY < y1 && mouseY > y2)
            {
                hoverOver = true;
            }
            if (hoverOver)
            {
                int reqIndex = (int)MathF.Floor((y1 - mouseY) / displayOffset);
                for(int i = 0; i < requirements.Count; i++)
                {
                    if(i == reqIndex)
                    {

                        requirementDisplayInstances[i].GetComponent<requirementIcon>().toggleOpen();
                    }
                    else
                    {
                        requirementDisplayInstances[i].GetComponent<requirementIcon>().setOpen(false);
                    }
                }
            }
        }
    }

    //checks all of the requirements for completion and updates the display accordingly. This should be called whenever a new change occures to the level
    //will update this once we have the correct assets for the display tab


    
    public void checkRequirements(bool outsideEdits = false)
    {
        int index = 0;
        bool allMet = true;
        bool anyMet = false;
        foreach(levelRequirementParent curRequirement in requirements)
        {
            if(outsideEdits == true && !curRequirement.checkOutsideEdits)
            {
                if (curRequirement.met)
                {
                    anyMet = true;
                }
                else
                {
                    allMet = false;
                }
                index++;
                continue;
            }

            curRequirement.check();
            GameObject curDisplayObj = display.transform.GetChild(display.transform.childCount - index - 1).gameObject;
            if (curRequirement.met)
            {
                anyMet = true;
                curDisplayObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            }
            else
            {
                allMet = false;
                curDisplayObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            index++;
        }

        if (allMet && anyMet)
        {
            TextManager.instance.endHold();
            TextManager.instance.GetTextBox().SetActive(true);
            requirements.Clear();
            updateDisplay();
        }

    }

}
