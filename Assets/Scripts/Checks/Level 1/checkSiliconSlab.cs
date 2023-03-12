using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkSiliconSlab : levelRequirementParent
{
    CheckStruct slab;
    public checkSiliconSlab(LayerStackHolder layers) : base(layers) {
        name = "Create deposit that's atleast 20um thick.";
        description = "You can use the measuring stick on the side of the waifer.";
        slab = new CheckStruct(0, 1);
        RectangleStructure slabStruct = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] {true, false, false}) ;

        slab.appendAllEnds(slabStruct);
    }

    public override void check()
    {
        met = slab.satisfy();
    }
}
