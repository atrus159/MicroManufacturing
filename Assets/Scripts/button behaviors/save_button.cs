using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using System.IO;

public class save_button : MonoBehaviour
{
    public Button loadButton;
    public paint paintCanvas;

    void Start()
    {
        // Debug.Log("Start!");
        Button loadButton = GetComponent<Button>();
        loadButton.onClick.AddListener(TaskOnClick);

        paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();

    }
    void TaskOnClick()
    {
        //Debug.Log("You have clicked the button!");

        string path = EditorUtility.SaveFilePanel("Save bitmap as .txt", "", "bitmap.txt", "txt");

        if (path.Length == 0)
            return;

        string[] lines = new string[bitMap.gridHeight];

        for (int j = 0; j < bitMap.gridHeight ; j++)
        {
            for (int i = bitMap.gridWidth - 1; i >= 0; i--)
            {
                lines[bitMap.gridHeight - j - 1] = paintCanvas.grid.getPoint(i, j) + lines[bitMap.gridHeight - j - 1];
            }
        }

        File.WriteAllLines(path, lines);

    }

}
