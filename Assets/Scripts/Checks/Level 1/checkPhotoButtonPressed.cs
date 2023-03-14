using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkPhotoButtonPressed : levelRequirementParent
{

    public override void onStart()
    {
        base.onStart();
        name = "Create your photoresist stencil.";
        description = "Click the Photoresist button in the bottom right to activate the process.";
        checkOutsideEdits = true;
    }

    public override void check()
    {
        if (GameObject.Find("Liftoff Button") && ! GameObject.Find("Spinner Container(Clone)"))
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
