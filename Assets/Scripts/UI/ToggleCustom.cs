using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Events;

public class ToggleCustom : MonoBehaviour
{
    public bool visible;

    bool toggled;

    CanvasGroup blocker;

    Image img;

    public Sprite offToggleSprite;
    public Sprite onToggleSprite;

    LayerStackHolder layerStack;

    // Start is called before the first frame update
    void Start()
    {
        
        toggled = false;
        img = gameObject.GetComponent<Image>();
        blocker = GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>();
        if (!visible)
        {
            img.enabled = false;
        }
        img.sprite = offToggleSprite;
        layerStack = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        if(blocker.blocksRaycasts || !visible)
        {
            return;
        }
        float mouseX = UnityEngine.Input.mousePosition.x;
        float mouseY = UnityEngine.Input.mousePosition.y;
        RectTransform trans = gameObject.GetComponent<RectTransform>();
        float width = trans.sizeDelta.x*trans.lossyScale.x;
        float height = trans.sizeDelta.y * trans.lossyScale.y;


        float x1 = trans.position.x - width/2;
        float y1 = trans.position.y - height/2;
        float x2 = x1 + width;
        float y2 = y1 + height;

        bool hoverOver = false;
        if(mouseX > x1 && mouseX < x2 && mouseY > y1 && mouseY < y2)
        {
            hoverOver = true;
        }

        bool mouseDown = UnityEngine.Input.GetMouseButtonDown(0);


        if (mouseDown && hoverOver && GameObject.Find("showSchematicGrid"))
        {
            if (toggled)
            {
                toggled = false;
                img.sprite = offToggleSprite;
            }
            else
            {
                toggled = true;
                img.sprite = onToggleSprite;
            }
            layerStack.wetEtch = toggled;
        }

    }
}
