using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tutorialText : MonoBehaviour
{
    public tutorialParent myTutorial;
    TextMeshProUGUI textBox;
    GameObject myText;
    GameObject myImage;
    GameObject myPanel;
    // Start is called before the first frame update
    void Awake()
    {
        myImage = transform.GetChild(0).gameObject;
        myText = myImage.transform.GetChild(0).gameObject;
        textBox = myText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myTutorial.stepFlag = true;
            GameObject.Find("Control").GetComponent<control>().tutorialBlockerVisible = false;
            Destroy(gameObject);

        }

    }

    public void updateText(string newText)
    {
        textBox.SetText(newText);

    }
}
