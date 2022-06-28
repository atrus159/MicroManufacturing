using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class draw_button : MonoBehaviour
{
    public Sprite mainImage;
    public Sprite backupImage;
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
        paintCavas.curTool = 0;
    }

    public void Update()
    {
        if(paintCavas.curTool == 0)
        {
            img.sprite = backupImage;
        }
        else
        {
            img.sprite = mainImage;
        }
    }
}
