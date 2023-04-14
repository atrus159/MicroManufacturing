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
        name = "Head to the Photolithography Bench.";
        description = "You can press TAB, or click on the button in the top left to switch to the Photolithography Bench.";
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
