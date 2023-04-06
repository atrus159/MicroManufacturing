using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkReturn : levelRequirementParent
{
    CanvasGroup cGroup;

    public override void onStart()
    {
        base.onStart();
        name = "Draw something for your lab partner, then return to the workbench.";
        description = "You can use the tools on the left to draw whatever you want.";
        checkOutsideEdits = true;
        cGroup = GameObject.Find("Canvas - HUD").GetComponent<CanvasGroup>();
    }

    public override void check()
    {
        if (cGroup.alpha == 0)
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
