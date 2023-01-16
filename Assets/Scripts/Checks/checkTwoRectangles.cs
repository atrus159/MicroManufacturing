using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkTwoRectangles : levelRequirementParent
{
    RectangleStructure rectangles;
    public checkTwoRectangles(LayerStackHolder layers) : base(layers) {
        name = "Create two boxes from gold";
        description = "A box must be between 10 and 20 cells on a side, and no taller than 30 layers.";
        rectangles = new RectangleStructure(control.materialType.gold, new Vector3Int(10, 0, 10), new Vector3Int(20, 30, 20), 2, new[] { true, true }, control.materialType.empty);
    }

    public override void check()
    {
        CheckStructComponent.satisfyResult initial = new CheckStructComponent.satisfyResult();
        initial.layer = 0;
        initial.direction = 1;
        initial.satisfied = false;
        CheckStructComponent.satisfyResult result = rectangles.satisfy(initial);

        if (result.satisfied)
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
