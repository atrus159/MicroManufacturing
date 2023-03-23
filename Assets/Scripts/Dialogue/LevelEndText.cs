using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.FilePathAttribute;

public class LevelEndText : TextParent
{
    override public void Initialize()
    {
        base.Initialize();
    }
    override public void Display()
    {
        base.Display();
        SceneManager.LoadScene("Credits");
    }
}
