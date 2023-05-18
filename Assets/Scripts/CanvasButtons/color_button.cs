using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class color_button : MonoBehaviour
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
        img = transform.GetChild(0).GetComponent<Image>();
        btn.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick()
    {
        paintCavas.curColor = 1 - paintCavas.curColor;
    }

    public void Update()
    {
        if (paintCavas.curColor == 1)
        {
            img.sprite = backupImage;
        }
        else
        {
            img.sprite = mainImage;
        }
    }
}
