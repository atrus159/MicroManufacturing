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
        RectangleStructure chromePart = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(50, 10, 50), new Vector3Int(90, 20, 90), 1, new[] { true, true }, control.materialType.empty);
        RectangleStructure goldPart = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(20, 10, 20), new Vector3Int(30, 20, 30), 1, new[] { true, true }, control.materialType.empty);
        RectangleStructure alumPart = new RectangleStructure(control.materialType.aluminum, 1, new Vector3Int(10, 10, 10), new Vector3Int(20, 20, 20), 1, new[] { true, true }, control.materialType.empty);
        ChromeGoldAlum.appendAllEnds(chromePart);
        ChromeGoldAlum.appendAllEnds(goldPart);
        ChromeGoldAlum.appendAllEnds(alumPart);
    }

    public override void check()
    {
        met = ChromeGoldAlum.satisfy();
    }
}
