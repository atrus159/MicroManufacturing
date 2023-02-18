using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class FreeText : TextParent
{
    override public void Initialize()
    {
        base.Initialize();
    }
    override public void Display()
    {
        base.Display();
        TextManager.instance.holdFlag = true;
        TextManager.instance.GetTextBox().SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TextManager.instance.holdFlag = false;
            TextManager.instance.GetTextBox().SetActive(true);
        }
    }


}
