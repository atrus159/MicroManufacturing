using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.FilePathAttribute;

public class LevelEndText : TextParent
{

    public string destination;
    override public void Initialize()
    {
        base.Initialize();
    }
    override public void Display()
    {
        base.Display();
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(destination);
    }
}
