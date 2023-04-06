using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkTwoRectangles : levelRequirementParent
{
    CheckStruct cantilever;

    public override void onStart()
    {
        base.onStart();
        name = "Create two boxes from gold";
        description = "A box must be between 10 and 20 cells on a side, and no taller than 30 layers.";
        cantilever = new CheckStruct(0, 1);
        AmorphousStructure amorphBase = new AmorphousStructure(control.materialType.chromium, 1, new Vector3Int(5, 10, 5), new Vector3Int(40, 40, 40), 1, new[] { true, true, false }, control.materialType.empty);
        /*RectangleStructure siliconBasePart = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(60, 5, 20), new Vector3Int(90, 15, 50), 1, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure hingePart = new RectangleStructure(control.materialType.silicondioxide, 1, new Vector3Int(10, 10, 10), new Vector3Int(20, 20, 20), 1, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure siliconTopPart = new RectangleStructure(control.materialType.silicon, -1, new Vector3Int(60, 5, 20), new Vector3Int(90, 15, 50), 1, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure plateGoldBottom = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(5, 5, 5), new Vector3Int(20, 10, 20), 1, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure emptyTop = new RectangleStructure(control.materialType.empty, 1, new Vector3Int(90, 1, 90), new Vector3Int(100, 100, 100), 1, new[] { false, false, true });
        RectangleStructure plateGoldTop = plateGoldBottom.clone() as RectangleStructure;
        cantilever.append(siliconBasePart, cantilever.head);
        cantilever.append(hingePart, siliconBasePart);
        cantilever.append(plateGoldBottom, siliconBasePart);
        cantilever.append(siliconTopPart, hingePart);
        cantilever.append(plateGoldTop, siliconTopPart);*/
        cantilever.appendAllEnds(amorphBase);
    }

    public override void check()
    {
        met = cantilever.satisfy();
    }
}
