using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkPhotoButtonPressed : levelRequirementParent
{
    int trueCount;
    public override void onStart()
    {
        base.onStart();
        name = "Create your photoresist stencil.";
        description = "Click the Photoresist button in the bottom right to complete the process.";
        checkOutsideEdits = true;
        trueCount = 0;
    }

    public override void check()
    {
        if (GameObject.Find("Liftoff Button") && !GameObject.Find("Spinner Container(Clone)") && GameObject.Find("Animation Creator").GetComponent<AnimationCreator>().curLightBeamState == AnimationCreator.states.standby)
        {
            met = true;
            return;
        }

        met = false;

    }
}
