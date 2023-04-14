using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Events;

public class Blueprint : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openSprite;
    public Sprite drawingSprite;

    bool open;

    Image frameImage;
    Image drawingImage;
    GameObject drawing;

    CanvasGroup blocker;

    // Start is called before the first frame update
    void Start()
    {
        frameImage = gameObject.GetComponent<Image>();
        frameImage.sprite = closedSprite;
        drawing = gameObject.transform.GetChild(0).gameObject;
        drawingImage = drawing.GetComponent<Image>();
        drawingImage.sprite = drawingSprite;
        drawing.SetActive(false);
        blocker = GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(blocker.blocksRaycasts)
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
        

    }
}
