using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class swap_button : MonoBehaviour
{
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
        for(int i = 0; i< control.gridWidth; i++)
        {
            for(int j = 0; j< control.gridHeight; j++)
            {
                paintCavas.setPixel(i,j,1 - paintCavas.grid[i, j]);
            }
        }
        paintCavas.texture.Apply(); 
    }
}
