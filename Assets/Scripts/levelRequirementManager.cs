using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor; //THE PROBLEM
using UnityEngine;
using UnityEngine.UI;

public class levelRequirementManager : MonoBehaviour
{
    //A list of scripts for each of the level requirements
    public List<MonoScript> requirementScripts = new List<MonoScript>();
    public List<MonoScript> requirementReserveScripts = new List<MonoScript>();
    public List<int> requirementReserveBlocks;
    int reserveIndex;



    //A list of instances of the classes in the scripts provided in requirementScripts. This will be filled out in start
    public List<levelRequirementParent> requirements = new List<levelRequirementParent>();

    List<GameObject> requirementDisplayInstances;

    //the requirement display, where all the requirements and their completion status can be viewed in game
    public GameObject display;
    //the prefab for one index in the requirement display
    public GameObject requirementPrefab;


    float displayOffset = 70.0f;

    void Start()
    {
        requirementDisplayInstances = new List<GameObject>();
        reserveIndex = 0;
        //instantiates all of the classes in requirementScripts and puts them in requirements
        foreach(MonoScript curScript in requirementScripts)
        {
            System.Type[] lsType = { typeof(LayerStackHolder) };
            System.Type curType = curScript.GetClass();
            ConstructorInfo curConstructor = curType.GetConstructor(lsType);
            object[] constructorArguments = { GameObject.Find("LayerStack").GetComponent<LayerStackHolder>() };
            object newRequirement = curConstructor.Invoke(constructorArguments);
            requirements.Add((levelRequirementParent) newRequirement);
        }

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

        int index = 0;

        foreach (levelRequirementParent curRequirement in requirements)
        {
            makePrefab(-index * displayOffset, curRequirement.name, curRequirement.description);
            index++;
        }
    }

    void makePrefab(float height, string name, string description)
    {
        GameObject newDisplayObj = Instantiate(requirementPrefab);
        newDisplayObj.transform.SetParent(display.transform);
        newDisplayObj.transform.SetPositionAndRotation(display.transform.position + new Vector3(0, height, 0), Quaternion.identity);
        newDisplayObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = name;
        newDisplayObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = description;
        newDisplayObj.GetComponent<Image>().color = Color.red;
        requirementDisplayInstances.Add(newDisplayObj);

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
            if(requirementReserveScripts.Count == 0)
            {
                break;
            }
            MonoScript curScript = requirementReserveScripts[0];
            System.Type[] lsType = { typeof(LayerStackHolder) };
            System.Type curType = curScript.GetClass();
            ConstructorInfo curConstructor = curType.GetConstructor(lsType);
            object[] constructorArguments = { GameObject.Find("LayerStack").GetComponent<LayerStackHolder>() };
            object newRequirement = curConstructor.Invoke(constructorArguments);
            requirements.Add((levelRequirementParent)newRequirement);
            requirementReserveScripts.RemoveAt(0);

        }
        reserveIndex++;

        updateDisplay();

    }

    // Update is called once per frame
    void Update()
    {
        //temporarily checking when spacebar is pressed, until we decide when the correct time to call the checks is
        if (Input.GetKeyDown(KeyCode.L))
        {
            checkRequirements();
        }
        
    }

    //checks all of the requirements for completion and updates the display accordingly. This should be called whenever a new change occures to the level
    //will update this once we have the correct assets for the display tab
    public void checkRequirements()
    {
        int index = 0;
        foreach(levelRequirementParent curRequirement in requirements)
        {
            curRequirement.check();
            GameObject curDisplayObj = display.transform.GetChild(index).gameObject;
            if (curRequirement.met)
            {
                curDisplayObj.GetComponent<Image>().color = Color.green;
            }
            else
            {
                curDisplayObj.GetComponent<Image>().color = Color.red;
            }
            index++;
        }
    }

}
