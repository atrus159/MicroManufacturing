using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkWetEtch : levelRequirementParent
{
    public override void onStart()
    {
        base.onStart();
<<<<<<< HEAD
        name = "Switch the etch toggle to “wet-etch” then etch the aluminum.";
        description = "The etch toggle is in the top right";
=======
        name = "Change the current material to Gold, and try to deposit it on the wafer.";
        description = "You can change materials with the drop-down in the bottom left";
>>>>>>> Level3Dasha
        checkOutsideEdits = true;
    }

    public override void check()
    {
<<<<<<< HEAD
        met = false;
        GameObject np = GameObject.Find("New Process");
        if (np)
        {
            if (np.GetComponent<ProcessWetEtch>())
            {
                met = true;
            }
=======
        if (GameObject.Find("New Process") && GameObject.Find("LayerStack").GetComponent<LayerStackHolder>().wetEtch == true)
        {
            met = true;
        }
        else
        {
            met = false;
>>>>>>> Level3Dasha
        }
    }
}
