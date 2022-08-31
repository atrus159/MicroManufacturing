using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tutorialText : MonoBehaviour
{
    TextMeshProUGUI textBox;
    GameObject myText;
    GameObject myImage;
    GameObject myPanel;
    // Start is called before the first frame update
    void Awake()
    {
        myText = transform.GetChild(2).gameObject;
        myImage = transform.GetChild(1).gameObject;
        myImage = transform.GetChild(0).gameObject;
        textBox = myText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateText(string newText)
    {
        textBox.SetText(newText);
        Vector3 newSize = new Vector3(2.0f, 2.0f, 0.0f);//textBox.GetRenderedValues(true);
        myImage.transform.localScale += newSize;

    }
}
