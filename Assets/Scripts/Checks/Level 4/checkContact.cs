using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkContact : levelRequirementParent
{
    CheckStruct contact;

    public override void onStart()
    {
        base.onStart();
        name = "Create the contact electrode";
        description = "It can be between 0.2 and 1 µm tall.";
        contact = new CheckStruct(0, 1);
        RectangleStructure chrome = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(3, 1, 3), new Vector3Int(60, 10, 60), 1, new[] { true, false, false }) ;
        RectangleStructure gold = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(3, 2, 3), new Vector3Int(60, 10, 60), 1, new[] { true, false, false });
        
        contact.append(chrome, contact.head);
        contact.append(gold,chrome);
    }

    public override void check()
    {
        met = contact.satisfy();
    }
}
