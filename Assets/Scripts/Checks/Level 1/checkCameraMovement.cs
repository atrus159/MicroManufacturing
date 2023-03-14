using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkCameraMovement : levelRequirementParent
{
    GameObject Camera;
    float startAngle;


    public override void onStart()
    {
        base.onStart();
        name = "Take a look around the wafer";
        description = "You can move the camera with the arrow keys.";
        Camera = GameObject.Find("Main Camera");
        startAngle = Camera.transform.rotation.eulerAngles.y;
        checkOutsideEdits = true;
    }

    public override void check()
    {
        float curAngle = Camera.transform.rotation.eulerAngles.y;
        if (Mathf.DeltaAngle(startAngle,curAngle) >= 160)
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
