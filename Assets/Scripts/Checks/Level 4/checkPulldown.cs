using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkPulldown : levelRequirementParent
{
    CheckStruct pulldown;

    public override void onStart()
    {
        base.onStart();
        name = "Create the pulldown electrode";
        description = "It must be 0.2 µm tall. This means 1 layer of chromium and one layer of gold";
        pulldown = new CheckStruct(0, 1);
        RectangleStructure chrome = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(10, 1, 10), new Vector3Int(60, 1, 60), 1, new[] { true, false, false }) ;
        RectangleStructure gold = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(10, 1, 10), new Vector3Int(60, 1, 60), 1, new[] { true, false, false });
        
        pulldown.append(chrome, pulldown.head);
        pulldown.append(gold,chrome);
    }

    public override void check()
    {
        met = pulldown.satisfy();
    }
}
