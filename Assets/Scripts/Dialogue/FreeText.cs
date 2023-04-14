using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

public class FreeText : TextParent
{
    override public void Initialize()
    {
        base.Initialize();
    }
    override public void Display()
    {
        base.Display();
        GameObject.Find("Level Requirement Manager").GetComponent<levelRequirementManager>().startDisplay();
        TextManager.instance.startHold();
        TextManager.instance.GetTextBox().SetActive(false);
        //control.setPaused(control.pauseStates.unPaused);

    }
}
