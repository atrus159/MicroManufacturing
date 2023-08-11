using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkWetEtch : levelRequirementParent
{
    public override void onStart()
    {
        base.onStart();
        name = "Switch the etch toggle to ?wet-etch? then etch the aluminum.";
        description = "The etch toggle is in the bottom left";
        checkOutsideEdits = true;
    }

    public override void check()
    {
        met = false;
        GameObject np = GameObject.Find("New Process");
        if (np)
        {
            if (np.GetComponent<ProcessWetEtch>())
            {
                met = true;
            }
        }
    }
}
