using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkPatternWindow : levelRequirementParent
{
    CanvasGroup cGroup;

    public override void onStart()
    {
        base.onStart();
        name = "Click the pattern button, or press TAB";
        description = "The pattern button is on the left side.";
        checkOutsideEdits = true;
        cGroup = GameObject.Find("Canvas - HUD").GetComponent<CanvasGroup>();
    }

    public override void check()
    {
        if (cGroup.alpha == 1)
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
