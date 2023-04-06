using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;

public class load_button : MonoBehaviour
{
    public Button loadButton;
    public paint paintCanvas;
    void Start()
    {
        Button loadButton = GetComponent<Button>();
        loadButton.onClick.AddListener(TaskOnClick);

        paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();

    }
    void TaskOnClick()
    {

        BrowserProperties bp = new BrowserProperties();
        bp.filter = "txt files (*.txt) | *.txt";
        bp.filterIndex = 0;

        string path = "";

        new FileBrowser().OpenFileBrowser(bp, filePath => { path = filePath; });


        if (path.Length == 0)
            return;

        string[] lines = System.IO.File.ReadAllLines(path);

        if (lines.Length < BitGrid.gridHeight || lines[0].Length < BitGrid.gridWidth)
        {
            Debug.Log("Improper file size!");
            return;
        }

        for (int j = 0; j < BitGrid.gridHeight; j++)
        {
            for (int i = BitGrid.gridWidth - 1; i >= 0; i--)
            {
                paintCanvas.setPixel(i, j, lines[BitGrid.gridHeight - j - 1][i] - '0');
            }
        }
        paintCanvas.texture.Apply();
    }
}
