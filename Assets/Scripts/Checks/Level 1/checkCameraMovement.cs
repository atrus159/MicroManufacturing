using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkCameraMovement : levelRequirementParent
{
    GameObject Camera;
    float startAngle;
    bool activated;
    float activeTime;

    public override void onStart()
    {
        base.onStart();
        name = "Take a look around the wafer";
        description = "You can move the camera with the arrow keys.";
        Camera = GameObject.Find("Main Camera");
        startAngle = Camera.transform.rotation.eulerAngles.y;
        checkOutsideEdits = true;
        activated= false;
        activeTime = 0.0f;
    }

    public override void check()
    {
        if (!activated)
        {
            float curAngle = Camera.transform.rotation.eulerAngles.y;
            if (!startAngle.Equals(curAngle))
            {
                activated = true;
            }
            met = false;
            return;
        }
        activeTime += Time.deltaTime;
        if(activeTime >= 2.75f)
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
