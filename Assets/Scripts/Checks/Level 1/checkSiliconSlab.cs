using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkSiliconSlab : levelRequirementParent
{
    CheckStruct slab;

    public override void onStart()
    {
        base.onStart();
        name = "Deposit a coating that's at least 2 µm thick.";
        description = "You can use the measuring stick on the side of the wafer to see how much that is. Click the “Finish” button when you’re done.";
        slab = new CheckStruct(0, 1);
        RectangleStructure slabStruct = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });

        slab.appendAllEnds(slabStruct);
    }

    public override void check()
    {
        met = slab.satisfy();
    }
}
