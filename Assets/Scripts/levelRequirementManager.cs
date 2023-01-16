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

    //A list of instances of the classes in the scripts provided in requirementScripts. This will be filled out in start
    public List<levelRequirementParent> requirements = new List<levelRequirementParent>();

    //the requirement display, where all the requirements and their completion status can be viewed in game
    public GameObject display;
    //the prefab for one index in the requirement display
    public GameObject requirementPrefab;

    void Start()
    {
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

        //fills out the requirement display with an instance of requirementPrefab for each requirement. Gives them the correct display name, discription and color signifying their completion status.
        //will update this once we have the correct assets for the display tab
        int index = 0;
        float offset = 70.0f;
        foreach(levelRequirementParent curRequirement in requirements)
        {
            GameObject newDisplayObj = Instantiate(requirementPrefab);
            newDisplayObj.transform.SetParent(display.transform);
            newDisplayObj.transform.SetPositionAndRotation(display.transform.position + new Vector3(0,-index*offset,0), Quaternion.identity);
            newDisplayObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = curRequirement.name;
            newDisplayObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = curRequirement.description;
            if (curRequirement.met)
            {
                newDisplayObj.GetComponent<Image>().color = Color.green;
            }
            else
            {
                newDisplayObj.GetComponent<Image>().color = Color.red;
            }
            index++;
        }
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
