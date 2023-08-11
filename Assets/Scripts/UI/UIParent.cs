using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParent : MonoBehaviour
{

    CanvasGroup blocker;
    public Image img;
    public bool visible;


    // Start is called before the first frame update
    public virtual void Start()
    {
        img = gameObject.GetComponent<Image>();
        blocker = GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>();
        if (!visible)
        {
            img.enabled = false;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }


    public bool getHoverOver()
    {
        if (blocker.blocksRaycasts || !visible)
        {
            return false;
        }
        float mouseX = UnityEngine.Input.mousePosition.x;
        float mouseY = UnityEngine.Input.mousePosition.y;
        RectTransform trans = gameObject.GetComponent<RectTransform>();
        float width = trans.sizeDelta.x * trans.lossyScale.x;
        float height = trans.sizeDelta.y * trans.lossyScale.y;


        float x1 = trans.position.x - width / 2;
        float y1 = trans.position.y - height / 2;
        float x2 = x1 + width;
        float y2 = y1 + height;

        bool hoverOver = false;
        if (mouseX > x1 && mouseX < x2 && mouseY > y1 && mouseY < y2)
        {
            hoverOver = true;
        }

        return hoverOver;
    }

}
