using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkTwoRectangles : levelRequirementParent
{
    CheckStruct ChromeGoldAlum;
    public checkTwoRectangles(LayerStackHolder layers) : base(layers) {
        name = "Create two boxes from gold";
        description = "A box must be between 10 and 20 cells on a side, and no taller than 30 layers.";
        ChromeGoldAlum = new CheckStruct(0, 1);
        RectangleStructure chromePart = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(60, 10, 60), new Vector3Int(90, 20, 90), 1, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure goldPart = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(30, 10, 30), new Vector3Int(60, 20, 60), 1, new[] { true, true, false }, control.materialType.empty); ;
        RectangleStructure alumPart = new RectangleStructure(control.materialType.aluminum, 1, new Vector3Int(7, 10, 7), new Vector3Int(35, 20, 35), 1, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure emptyTop = new RectangleStructure(control.materialType.empty, 1, new Vector3Int(90, 1, 90), new Vector3Int(100, 100, 100), 1, new[] { false, false, true });
        ChromeGoldAlum.appendAllEnds(chromePart);
        ChromeGoldAlum.appendAllEnds(goldPart);
        ChromeGoldAlum.appendAllEnds(alumPart);
        ChromeGoldAlum.appendAllEnds(emptyTop);
    }

    public override void check()
    {
        met = ChromeGoldAlum.satisfy();
    }
}
