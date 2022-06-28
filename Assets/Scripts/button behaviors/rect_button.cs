using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class rect_button : MonoBehaviour
{
    public Sprite mainImage;
    public Sprite backupImage;
    public Sprite backupImage2;
    public paint paintCavas;
    public Image img;
    public Button btn;
    // Start is called before the first frame update
    public void Start()
    {
        paintCavas = GameObject.Find("drawing_panel").GetComponent<paint>();
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        btn.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick()
    {
        if(paintCavas.curTool != 3)
        {
            paintCavas.fillMode = 0;
        }
        else
        {
            paintCavas.fillMode = 1 - paintCavas.fillMode;
        }
        paintCavas.curTool = 3;

    }

    public void Update()
    {
        if (paintCavas.curTool == 3)
        {
            if (paintCavas.fillMode == 0)
            {
                img.sprite = backupImage;
            }
            else
            {
                img.sprite = backupImage2;
            }
        }
        else
        {
            img.sprite = mainImage;
        }
    }
}
