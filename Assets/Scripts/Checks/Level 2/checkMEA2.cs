using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkMEA2 : levelRequirementParent
{
    CheckStruct photoresist;
    CheckStruct goldEtch;
    CheckStruct chromeEtch;

    public override void onStart()
    {
        base.onStart();
        name = "Use the photomask as a shield while etching away the gold.";
        description = "You will etch gold if it is selected from your materials dropdown.";
        goldEtch = new CheckStruct(0, 1);
        chromeEtch = new CheckStruct(0, 1);



        RectangleStructure gold1 = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(99, 1, 99), new Vector3Int(99, 20, 99), 1, new[] { true, false, false });
        RectangleStructure gold2 = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);
        
        RectangleStructure chrome1 = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure chrome2 = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);



        goldEtch.append(gold1, goldEtch.head);
        goldEtch.append(gold2, gold1);

        chromeEtch.append(chrome1, chromeEtch.head);
        chromeEtch.append(chrome2, chrome1);
    }

    public override void check()
    {
        met = goldEtch.satisfy() || chromeEtch.satisfy();
    }
}
