using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class swap_button : MonoBehaviour
{
    public paint paintCanvas;
    public Image img;
    public Button btn;
    // Start is called before the first frame update
    public void Start()
    {
        paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        btn.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick()
    {
        for(int i = 0; i< BitGrid.gridWidth; i++)
        {
            for(int j = 0; j< BitGrid.gridHeight; j++)
            {
                paintCanvas.setPixel(i,j,1 - paintCanvas.grid.getPoint(i,j));
            }
        }
        paintCanvas.texture.Apply();
        paintCanvas.addState();
    }
}
