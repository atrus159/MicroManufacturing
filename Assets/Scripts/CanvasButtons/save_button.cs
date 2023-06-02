using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine;
using System.IO;


/* Depending on platform, must implement different file browser. */
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN) && !UNITY_WEBGL
  using AnotherFileBrowser.Windows;
#endif
#if (UNITY_EDITOR_OSX) && !UNITY_WEBGL
    using UnityEditor;
#endif


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
        string path = "";

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN) && !UNITY_WEBGL
        BrowserProperties bp = new BrowserProperties();
        bp.filter = "txt files (*.txt) | *.txt";
        bp.filterIndex = 0;
        new FileBrowser().OpenFileBrowser(bp, filePath => { path = filePath; });
#endif

#if (UNITY_EDITOR_OSX) && !UNITY_WEBGL
        path = EditorUtility.SaveFilePanel("Save bitmap as .txt", "", "bitmap.txt", "txt");
#endif
        if (path.Length == 0)
            return;

        string[] lines = new string[BitGrid.gridHeight];

        for (int j = 0; j < BitGrid.gridHeight; j++)
        {
            for (int i = BitGrid.gridWidth - 1; i >= 0; i--)
            {
                lines[BitGrid.gridHeight - j - 1] = paintCanvas.grid.getPoint(i, j) + lines[BitGrid.gridHeight - j - 1];
            }
        }

        File.WriteAllLines(path, lines);

    }

}
