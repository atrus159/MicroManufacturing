using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialParent : MonoBehaviour
{
    public bool paused;
    public int tutorialStep;
    public bool stepFlag;
    public delegate void tutorialCheck();
    public GameObject textBoxPrefab;
    public List<tutorialCheck> tutorialChecks;
    GameObject tutorialCanvas;

    // Start is called before the first frame update
    void Start()
    {
        tutorialStep = 0;
        paused = false;
        stepFlag = true;
        tutorialChecks = new List<tutorialCheck>();
        initializeChecks();
        tutorialCanvas = GameObject.Find("Canvas - Tutorial");
    }

    virtual public void initializeChecks()
    {
        tutorialChecks.Add(delegate { displayText("Testing Testing 1 2 3", new Vector2(400, 400));});
    }

    // Update is called once per frame
    void Update()
    {
        if (stepFlag)
        {
            stepFlag = false;
            paused = false;
            tutorialChecks[tutorialStep]();
            tutorialStep++;
        }
        
    }

    void displayText(string text, Vector2 pos)
    {
        paused = true;
        GameObject newText = Instantiate(textBoxPrefab, pos, Quaternion.identity, tutorialCanvas.transform);
        newText.GetComponent<tutorialText>().updateText(text);
    }
}
