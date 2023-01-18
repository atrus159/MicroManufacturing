using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clear_button : MonoBehaviour
{
    // Start is called before the first frame update
    public Button clearButton;
    paint paintCanvas;
    void Start()
    {
        Button clearButton = GetComponent<Button>();
        clearButton.onClick.AddListener(TaskOnClick);

        paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();   
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        for (int j = 0; j < bitMap.gridHeight; j++)
        {
            for (int i = bitMap.gridWidth - 1; i >= 0; i--)
            {
                paintCanvas.setPixel(i, j, 0);
            }
        }
        paintCanvas.texture.Apply();
    }
}
