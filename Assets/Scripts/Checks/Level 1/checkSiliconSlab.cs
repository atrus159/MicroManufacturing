using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkSiliconSlab : levelRequirementParent
{
    CheckStruct slab;
    public checkSiliconSlab(LayerStackHolder layers) : base(layers) {
        name = "Click the deposit button";
        description = "Use the deposit button to create a slab";
        slab = new CheckStruct(0, 1);
        RectangleStructure slabStruct = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(99, 1, 99), new Vector3Int(99, 100, 99), 1, new[] {true, false, false}) ;

        slab.appendAllEnds(slabStruct);
    }

    public override void check()
    {
        met = slab.satisfy();
    }
}
