using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkDepositOnPhotoResist : levelRequirementParent
{
    CheckStruct slab;
    CheckStruct slabLiftoff;
    public override void onStart()
    {
        base.onStart();
        name = "Deposit material on top of the photoresist.";
        description = "You can use the Deposit button, the same as you did before.";
        slab = new CheckStruct(0, 1);
        RectangleStructure slabStruct = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });
        AmorphousStructure photoresist = new AmorphousStructure(control.materialType.photoresist, 1, new Vector3Int(1, 1, 1), new Vector3Int(99, 100, 99), 1, new[] { true, true, false }, control.materialType.silicon);


        slab.append(slabStruct, slab.head);
        slab.append(photoresist, slabStruct);

        slabLiftoff = new CheckStruct(0, 1);
        RectangleStructure slabLiftoffStruct = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });
        AmorphousStructure outlineStruct = new AmorphousStructure(control.materialType.silicon, 1, new Vector3Int(1, 1, 1), new Vector3Int(99, 100, 99), 1, new[] { true, true, false }, control.materialType.empty);
        slabLiftoff.append(slabLiftoffStruct, slabLiftoff.head);
        slabLiftoff.append(outlineStruct, slabLiftoffStruct);
    }


    public override void check()
    {
        met = slab.satisfy() || slabLiftoff.satisfy();
    }
}
