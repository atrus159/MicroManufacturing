using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkWetEtch : levelRequirementParent
{
    public override void onStart()
    {
        base.onStart();
        name = "Change the current material to Gold, and try to deposit it on the wafer.";
        description = "You can change materials with the drop-down in the bottom left";
        checkOutsideEdits = true;
    }

    public override void check()
    {
        if (GameObject.Find("New Process") && GameObject.Find("LayerStack").GetComponent<LayerStackHolder>().wetEtch == true)
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
