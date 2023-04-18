using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkGoldOnChrome : levelRequirementParent
{
    CheckStruct slab;

    public override void onStart()
    {
        base.onStart();
        name = "Deposit 20 µm of gold on top of the chromium.";
        description = "You can change materials with the drop-down in the bottom left";
        slab = new CheckStruct(0, 1);
        RectangleStructure slabChrome = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(99, 1, 99), new Vector3Int(99, 20, 99), 1, new[] { true, false, false });
        RectangleStructure slabGold = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });


        

        slab.append(slabChrome, slab.head);
        slab.append(slabGold, slabChrome);
    }

    public override void check()
    {
        met = slab.satisfy();
    }
}
