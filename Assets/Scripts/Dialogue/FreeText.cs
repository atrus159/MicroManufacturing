using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class FreeText : TextParent
{
    public bool advanceRequirements;
    override public void Initialize()
    {
        base.Initialize();
    }
    override public void Display()
    {
        base.Display();
        TextManager.instance.holdFlag = true;
        TextManager.instance.GetTextBox().SetActive(false);

        if (advanceRequirements)
        {
            GameObject.Find("Level Requirement Manager").GetComponent<levelRequirementManager>().addReserve();
        }

    }
}
