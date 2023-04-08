using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextParent : MonoBehaviour
{

    public TextMeshProUGUI textBoxText;
    public RectTransform textBoxPosition;
    bool initialFlag = false;
    public List<GameObject> activates = new List<GameObject>();
    public string skipOnObj;
    public bool advanceRequirements;

    // Start is called before the first frame update
    virtual public void Initialize()
    {
        foreach (GameObject curActive in activates)
        {
            curActive.GetComponent<Activatable>().activate();
        }
        textBoxText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        textBoxPosition = GameObject.Find("Textbox").GetComponent<RectTransform>();
        initialFlag = true;
        if (advanceRequirements)
        {
            levelRequirementManager lr = GameObject.Find("Level Requirement Manager").GetComponent<levelRequirementManager>();
            lr.addReserve();
            lr.hideDisplay();
        }
    }

        // Update is called once per frame
    virtual public void Display()
    {
        if (!initialFlag)
        {
            Initialize();
        }
    }
}
