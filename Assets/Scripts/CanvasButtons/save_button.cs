using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using UnityEngine;
using System.IO;

public class save_button : MonoBehaviour
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

        string[] lines = new string[BitGrid.gridHeight];

        for (int j = 0; j < BitGrid.gridHeight ; j++)
        {
            for (int i = BitGrid.gridWidth - 1; i >= 0; i--)
            {
                lines[BitGrid.gridHeight - j - 1] = paintCanvas.grid.getPoint(i, j) + lines[BitGrid.gridHeight - j - 1];
            }
        }

        File.WriteAllLines(path, lines);

    }

}
