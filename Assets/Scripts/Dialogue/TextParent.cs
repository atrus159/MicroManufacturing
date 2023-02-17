using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextParent : MonoBehaviour
{

    public TextMeshProUGUI textBoxText;
    public RectTransform textBoxPosition;
    bool initialFlag = false;


    // Start is called before the first frame update
    virtual public void Initialize()
    {
        textBoxText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        textBoxPosition = GameObject.Find("Textbox").GetComponent<RectTransform>();
        initialFlag = true;

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
