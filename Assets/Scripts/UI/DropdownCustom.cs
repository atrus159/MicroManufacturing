using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Events;

public class DropdownCustom : MonoBehaviour
{
    public bool hasChromium;
    public bool hasGold;
    public bool hasAluminum;
    public bool hasSilicon;
    public bool hasSiliconDioxide;

    public Sprite chromium_menu;
    public Sprite chromium_select;
    public Sprite gold_menu;
    public Sprite gold_select;
    public Sprite aluminum_menu;
    public Sprite aluminum_select;
    public Sprite silicon_menu;
    public Sprite silicon_select;
    public Sprite silicon_dioxide_menu;
    public Sprite silicon_dioxide_select;

    public Sprite[] dropDownSprites = new Sprite[6];

    bool toggled;
    int curElement;
    int numElements;
    List<int> returnElements;
    GameObject tab;

    public int value;

    // Start is called before the first frame update
    void Start()
    {
        tab = gameObject.transform.GetChild(0).gameObject;
        tab.SetActive(false);
        returnElements = new List<int>();
        toggled = false;
        curElement = 0;
        numElements = 0;
        if (hasAluminum)
        {
            numElements++;
            returnElements.Add(2);
        }
        if (hasChromium)
        {
            numElements++;
            returnElements.Add(0);
        }
        if (hasGold)
        {
            numElements++;
            returnElements.Add(1);
        }
        if (hasSilicon)
        {
            numElements++;
            returnElements.Add(3);
        }
        if (hasSiliconDioxide)
        {
            numElements++;
            returnElements.Add(4);
        }
        value = returnElements[0];
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = UnityEngine.Input.mousePosition.x;
        float mouseY = UnityEngine.Input.mousePosition.y;

        float width = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float height = gameObject.GetComponent<RectTransform>().sizeDelta.y;


        float x1 = gameObject.GetComponent<RectTransform>().position.x - width/2;
        float y1 = gameObject.GetComponent<RectTransform>().position.y - height/2;
        float x2 = x1 + width;
        float y2 = y1 + height;

        bool hoverOver = false;
        if(mouseX > x1 && mouseX < x2 && mouseY > y1 && mouseY < y2)
        {
            hoverOver = true;
        }


        switch (curElement)
        {
            case 0:
                if (hoverOver)
                {
                    gameObject.GetComponent<Image>().sprite = aluminum_select;
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = aluminum_menu;
                }
                break;
            case 1:
                if (hoverOver)
                {
                    gameObject.GetComponent<Image>().sprite = chromium_select;
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = chromium_menu;
                }
                break;
            case 2:
                if (hoverOver)
                {
                    gameObject.GetComponent<Image>().sprite = gold_select;
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = gold_menu;
                }
                break;
            case 3:
                if (hoverOver)
                {
                    gameObject.GetComponent<Image>().sprite = silicon_select;
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = silicon_menu;
                }
                break;
            case 4:
                if (hoverOver)
                {
                    gameObject.GetComponent<Image>().sprite = silicon_dioxide_select;
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = silicon_dioxide_menu;
                }
                break;
        }

        bool mouseDown = UnityEngine.Input.GetMouseButtonDown(0);
        if (toggled)
        {
            int index = 0;
            if (mouseX > x1 && mouseX < x2)
            {
                float tabHeight = tab.GetComponent<RectTransform>().sizeDelta.y;
                float mouseRelY = y1 - mouseY;
                float separation = tabHeight / numElements;
                index = (int)Mathf.Floor(mouseRelY / separation) + 1;
                if (index < 0 || index > numElements)
                {
                    index = 0;
                }
                if (mouseDown && index != 0)
                {
                    curElement = index - 1;
                    value = returnElements[curElement];
                    GameObject.Find("Control").GetComponent<control>().onDropDownChanged();
                }
            }
            tab.GetComponent<Image>().sprite = dropDownSprites[index];

        }


        if (mouseDown)
        {
            if(hoverOver && !toggled)
            {
                toggled = true;
            }
            else
            {
                toggled = false;
            }
            tab.SetActive(toggled);
        }

    }
}
