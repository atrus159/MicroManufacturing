using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkMEA1 : levelRequirementParent
{
    CheckStruct photoresist;
    CheckStruct goldEtch;
    CheckStruct chromeEtch;

    public override void onStart()
    {
        base.onStart();
        name = "Create a photomask of the four contacts.";
        description = "This mask should consist of black squares where you want the contacts to go.";
        photoresist = new CheckStruct(0, 1);
        goldEtch = new CheckStruct(0, 1);
        chromeEtch = new CheckStruct(0, 1);


        RectangleStructure photo1 = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(99, 1, 99), new Vector3Int(99, 20, 99), 1, new[] { true, false, false });
        RectangleStructure photo2 = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(99, 20, 99), new Vector3Int(99, 100, 99), 1, new[] { true, false, false });
        RectangleStructure photo3 = new RectangleStructure(control.materialType.photoresist, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);

        RectangleStructure gold1 = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(99, 1, 99), new Vector3Int(99, 20, 99), 1, new[] { true, false, false });
        RectangleStructure gold2 = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);
        
        RectangleStructure chrome1 = new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);
        RectangleStructure chrome2 = new RectangleStructure(control.materialType.gold, 1, new Vector3Int(5, 1, 5), new Vector3Int(40, 100, 40), 4, new[] { true, true, false }, control.materialType.empty);




        photoresist.append(photo1, photoresist.head);
        photoresist.append(photo2, photo1);
        photoresist.append(photo3, photo2);

        goldEtch.append(gold1, goldEtch.head);
        goldEtch.append(gold2, gold1);

        chromeEtch.append(chrome1, chromeEtch.head);
        chromeEtch.append(chrome2, chrome1);
    }

    public override void check()
    {
        met = photoresist.satisfy() || goldEtch.satisfy() || chromeEtch.satisfy();
    }
}
