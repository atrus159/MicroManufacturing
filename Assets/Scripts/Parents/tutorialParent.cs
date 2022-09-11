using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialParent : MonoBehaviour
{
    public int tutorialStep;
    public bool stepFlag;
    public delegate void tutorialCheck();
    public GameObject textBoxPrefab;
    public List<tutorialCheck> tutorialChecks;
    GameObject tutorialCanvas;

    // Start is called before the first frame update
    virtual public void Start()
    {
        tutorialStep = 0;
        stepFlag = true;
        tutorialChecks = new List<tutorialCheck>();
        initializeChecks();
        tutorialCanvas = GameObject.Find("Canvas - Tutorial Text");
}

    virtual public void initializeChecks()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (stepFlag)
        {
            stepFlag = false;
            control.setPaused(control.pauseStates.unPaused);
            tutorialChecks[tutorialStep]();
            tutorialStep++;
        }
        
    }

    public void displayText(string text, Vector2 pos)
    {
        control.setPaused(control.pauseStates.tutorialPaused);
        GameObject.Find("Control").GetComponent<control>().tutorialBlockerVisible = true;
        GameObject newText = Instantiate(textBoxPrefab, pos, Quaternion.identity, tutorialCanvas.transform);
        newText.GetComponent<tutorialText>().myTutorial = this;
        newText.GetComponent<tutorialText>().updateText(text);
    }

    public void displayTextFree(string text, Vector2 pos)
    {
        control.setPaused(control.pauseStates.unPaused);
        GameObject.Find("Control").GetComponent<control>().tutorialBlockerVisible = false;
        GameObject newText = Instantiate(textBoxPrefab, pos, Quaternion.identity, tutorialCanvas.transform);
        newText.GetComponent<tutorialText>().myTutorial = this;
        newText.GetComponent<tutorialText>().updateText(text);
    }

    public void checkSpacer(string checkName)
    {
        if (!GameObject.Find(checkName))
        {
            stepFlag = true;
            Destroy(GameObject.Find("TutorialTextBox(Clone)"));
        }
    }

    public GameObject initializeCheck<T>(string name) where T : UnityEngine.Component
    {
        GameObject toReturn = new GameObject(name);
        toReturn.AddComponent<T>();
        toReturn.GetComponent<checkParent>().myTutorial = this;
        toReturn.SetActive(false);
        return toReturn;
    }
}
