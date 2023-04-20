using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkMEA3 : levelRequirementParent
{
    CheckStruct photoresist;
    CheckStruct goldEtch;
    CheckStruct chromeEtch;

    public override void onStart()
    {
        base.onStart();
        name = "Remember to etch away the chromium after you finish with the gold!";
        description = "You will etch chromium if it is selected from your materials dropdown.";
        chromeEtch = new CheckStruct(0, 1);

        
        RectangleStructure chrome1 = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure chrome2 = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);



        chromeEtch.append(chrome1, chromeEtch.head);
        chromeEtch.append(chrome2, chrome1);
    }

    public override void check()
    {
        met = chromeEtch.satisfy();
    }
}
