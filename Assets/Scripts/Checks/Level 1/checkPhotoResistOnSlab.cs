using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkPhotoResistOnSlab : levelRequirementParent
{
    CheckStruct slab;

    public override void onStart()
    {
        base.onStart();
        name = "Press the Photoresist button to lay down a photoresist.";
        description = "The button is in the lower right corner.";
        slab = new CheckStruct(0, 1);
        RectangleStructure slabStruct = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });
        AmorphousStructure photoresist = new AmorphousStructure(control.materialType.photoresist, 1, new Vector3Int(1, 1, 1), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });


        slab.appendAllEnds(slabStruct);
        slab.append(photoresist, slabStruct);
    }

    public override void check()
    {
        met = slab.satisfy();
    }
}
