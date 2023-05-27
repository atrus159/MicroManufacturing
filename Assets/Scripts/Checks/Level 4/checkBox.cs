using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkBox : levelRequirementParent
{
    CheckStruct boxChrome;
    CheckStruct boxSilicon;
    CheckStruct boxSiO2;


    public override void onStart()
    {
        base.onStart();
        name = "Create a small box of aluminum with a different material on top.";
        description = "You can pick whatever other material you want.";
        boxChrome = new CheckStruct(0, 1);
        boxSilicon = new CheckStruct(0, 1);
        boxSiO2 = new CheckStruct(0, 1);
        RectangleStructure boxAlumCr = new RectangleStructure(control.materialType.aluminum, 1, new Vector3Int(5, 5, 5), new Vector3Int(90, 90, 90), 1, new[] { true, true, false });
        RectangleStructure boxAlumSi = new RectangleStructure(control.materialType.aluminum, 1, new Vector3Int(5, 5, 5), new Vector3Int(90, 90, 90), 1, new[] { true, true, false });
        RectangleStructure boxAlumSo = new RectangleStructure(control.materialType.aluminum, 1, new Vector3Int(5, 5, 5), new Vector3Int(90, 90, 90), 1, new[] { true, true, false });


        RectangleStructure topChrome= new RectangleStructure(control.materialType.chromium, 1, new Vector3Int(5, 1, 5), new Vector3Int(90, 90, 90), 1, new[] { true, true, false });
        RectangleStructure topSilicon = new RectangleStructure(control.materialType.silicon, 1, new Vector3Int(5, 1, 5), new Vector3Int(90, 90, 90), 1, new[] { true, true, false });
        RectangleStructure topSiO2 = new RectangleStructure(control.materialType.silicondioxide, 1, new Vector3Int(5, 1, 5), new Vector3Int(90, 90, 90), 1, new[] { true, true, false });

        boxChrome.append(boxAlumCr, boxChrome.head);
        boxChrome.append(topChrome, boxAlumCr);

        boxSilicon.append(boxAlumSi, boxSilicon.head);
        boxSilicon.append(topSilicon, boxAlumSi);

        boxSiO2.append(boxAlumSo, boxSiO2.head);
        boxSiO2.append(topSiO2, boxAlumSo);
    }

    public override void check()
    {
        met = boxChrome.satisfy() || boxSilicon.satisfy() || boxSiO2.satisfy();
    }
}
