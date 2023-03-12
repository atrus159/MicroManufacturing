using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkEPressed : levelRequirementParent
{


    public override void onStart()
    {
        base.onStart();
        name = "Press the E button";
        description = "At the top left corner of your keyboard is a button with 'E' on it. Press that button";
    }


    public override void check()
    {
        if (Input.GetKey(KeyCode.E))
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
