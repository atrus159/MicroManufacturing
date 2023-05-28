using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkBox : levelRequirementParent
{
    CheckStruct boxChrome;
    CheckStruct boxAlum;
    CheckStruct boxSilicon;
    CheckStruct boxSiO2;


    public override void onStart()
    {
        base.onStart();
        name = "Create a small box of aluminum with silicon on top.";
        description = "You know how to do this!";
        boxChrome = new CheckStruct(0, 1);
        boxAlum = new CheckStruct(0, 1);
        boxSilicon = new CheckStruct(0, 1);
        boxSiO2 = new CheckStruct(0, 1);
        RectangleStructure boxChromeSt= new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(5, 5, 5), new Vector3Int(90, 90, 90), 1, new[] { true, false, false });
        RectangleStructure boxAlumSt = new RectangleStructure(control.materialType.aluminum, 1, new Vector3Int(5, 5, 5), new Vector3Int(90, 90, 90), 1, new[] { true, false, false });
        RectangleStructure boxSiliconSt = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(5, 5, 5), new Vector3Int(90, 90, 90), 1, new[] { true, false, false });
        RectangleStructure boxSiO2St = new RectangleStructure(control.materialType.silicondioxide, 1, new Vector3Int(5, 5, 5), new Vector3Int(90, 90, 90), 1, new[] { true, false, false });

        boxChrome.append(boxChromeSt, boxChrome.head);
        boxChrome.append(boxAlumSt, boxAlum.head);
        boxChrome.append(boxSiliconSt, boxSilicon.head);
        boxChrome.append(boxSiO2St, boxSiO2.head);
    }

    public override void check()
    {
        met = boxChrome.satisfy() || boxAlum.satisfy() || boxSilicon.satisfy() || boxSiO2.satisfy();
    }
}
