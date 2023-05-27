using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkCantileverScaffold : levelRequirementParent
{
    CheckStruct cantilever;

    public override void onStart()
    {
        base.onStart();
        name = "Create the body and top of the cantilever, using a different material as the scaffold";
        description = "Aluminum would work, since it’s not used for any part of the design.";
        cantilever = new CheckStruct(0, 1);
        RectangleStructure siHinge = new RectangleStructure(control.materialType.silicondioxide, 1, new Vector3Int(10, 30, 10), new Vector3Int(60, 50, 60), 1, new[] { true, false, false });
        AmorphousStructure chTop = new AmorphousStructure(control.materialType.chromium, 1, new Vector3Int(10, 1, 50), new Vector3Int(60, 10, 90), 1, new[] { false, false, false }) ;
        AmorphousStructure goldTop = new AmorphousStructure(control.materialType.gold, 1, new Vector3Int(10, 1, 50), new Vector3Int(60, 10, 90), 1, new[] { false, false, false });
        
        cantilever.append(siHinge, cantilever.head);
        cantilever.append(chTop,siHinge);
        cantilever.append(goldTop, chTop);
    }

    public override void check()
    {
        met = cantilever.satisfy();
    }
}
