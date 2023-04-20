using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkLiftOff : levelRequirementParent
{
    CheckStruct slab;

    public override void onStart()
    {
        base.onStart();
        name = "Once you deposit the material, use lift-off to remove the coating on top to reveal your pattern.";
        description = "Note that the Photoresist button has been replaced with the Lift-Off button after you pressed it.";
        slab = new CheckStruct(0, 1);
        RectangleStructure slabStruct = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });
        AmorphousStructure outlineStruct = new AmorphousStructure(control.materialType.silicon, 1, new Vector3Int(1, 1, 1), new Vector3Int(99, 100, 99), 1, new[] { true, true, false }, control.materialType.empty);


        slab.append(slabStruct, slab.head);
        slab.append(outlineStruct, slabStruct);
    }

    public override void check()
    {
        met = slab.satisfy();
    }
}
