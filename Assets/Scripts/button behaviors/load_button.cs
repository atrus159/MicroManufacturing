using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class load_button : MonoBehaviour
{
    public Button loadButton;
    public paint paintCanvas;
    void Start()
    {
        Debug.Log("Start!");
        Button loadButton = GetComponent<Button>();
        loadButton.onClick.AddListener(TaskOnClick);

        paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();

    }
    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        string path = EditorUtility.OpenFilePanel("Overwrite with .txt", "", ".txt");

        if (path.Length == 0)
            return;

        string[] lines = System.IO.File.ReadAllLines(path);

        if (lines.Length < bitMap.gridHeight || lines[0].Length < bitMap.gridWidth)
        {
            Debug.Log("Improper file size!");
            return;
        }

        for (int j = 0; j < bitMap.gridHeight; j++)
        {
            Debug.Log(lines[j][0]);
            for (int i = bitMap.gridWidth - 1; i >= 0; i--)
            {
                paintCanvas.setPixel(i, j, lines[bitMap.gridHeight - j - 1][i] - '0');
            }
        }
        paintCanvas.texture.Apply();
        bitMap myBitmap = new bitMap();

    }
}
