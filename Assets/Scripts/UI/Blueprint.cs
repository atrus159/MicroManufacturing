using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Events;
using Unity.Mathematics;

public class Blueprint : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openSprite;
    public Sprite drawingSprite;

    bool open;

    Image frameImage;
    GameObject drawing;

    CanvasGroup blocker;

    float originalX;
    float originalY;


    float mouseRelX;
    float mouseRelY;

    enum dragStates
    {
        waiting,
        clicked
    }


    Vector2 closedScale = new Vector2(250,48);
    Vector2 openScale = new Vector2(250,400);

    dragStates dragState;

    // Start is called before the first frame update
    void Start()
    {
        frameImage = gameObject.GetComponent<Image>();
        drawing = gameObject.transform.GetChild(0).gameObject;
        blocker = GameObject.Find("Canvas - Tutorial Blocker").GetComponent<CanvasGroup>();
        dragState = dragStates.waiting;
        originalX = 0.0f;
        originalY = 0.0f;
        mouseRelX = 0.0f;
        mouseRelY= 0.0f;
        setOpen(true);
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


        float x1 = trans.position.x;
        float y1 = trans.position.y;
        float x2 = x1 + width;
        float y2 = y1 - height;

        bool hoverOver = false;
        if(mouseX > x1 && mouseX < x2 && mouseY < y1 && mouseY > y2)
        {
            hoverOver = true;
        }

        bool mouseDown = UnityEngine.Input.GetMouseButtonDown(0);
        bool mouseUp = UnityEngine.Input.GetMouseButtonUp(0);

        if (dragState == dragStates.waiting)
        {
            if (hoverOver && mouseDown)
            {
                dragState = dragStates.clicked;
                mouseRelX = trans.position.x - mouseX;
                mouseRelY = trans.position.y - mouseY;
                originalX = trans.position.x;
                originalY = trans.position.y;
            }
        } else if (dragState == dragStates.clicked)
        {
            GameObject.Find("Main Camera").GetComponent<OrbitCamera>().LockOut();
            trans.SetPositionAndRotation(new Vector2(mouseRelX + mouseX, mouseRelY + mouseY), Quaternion.identity);

            if (mouseUp || !GameObject.Find("showSchematicGrid"))
            {
                dragState = dragStates.waiting;
                GameObject.Find("Main Camera").GetComponent<OrbitCamera>().UnlockOut();
                Debug.Log("bp trigger");
                float dist = math.sqrt(math.pow(trans.position.y - originalY,2) - math.pow(trans.position.x - originalX,2));
                if (dist <= 2.0f && GameObject.Find("showSchematicGrid"))
                {
                    if (open)
                    {
                        setOpen(false);
                    }
                    else
                    {
                        setOpen(true);
                    }
                }
            }

        }
    }


    void setOpen(bool toSet)
    {
        RectTransform trans = gameObject.GetComponent<RectTransform>();
        if (toSet)
        {
            frameImage.sprite = openSprite;
            drawing.SetActive(true);
            open = true;
            trans.sizeDelta = openScale;
            //float offset = openScale.y / 2 - closedScale.y / 2;
            //trans.SetPositionAndRotation(new Vector2(trans.position.x, trans.position.y - offset), Quaternion.identity);
        }
        else
        {
            frameImage.sprite = closedSprite;
            drawing.SetActive(false);
            open = false;
            trans.sizeDelta = closedScale;
            //float offset = openScale.y / 2 - closedScale.y / 2;
            //trans.SetPositionAndRotation(new Vector2(trans.position.x, trans.position.y + offset), Quaternion.identity);
        }
    }

}
