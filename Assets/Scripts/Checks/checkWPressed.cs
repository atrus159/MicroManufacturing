using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkWPressed : levelRequirementParent
{

    public override void onStart()
    {
        base.onStart();
        name = "Press the W button";
        description = "At the top left corner of your keyboard is a button with 'W' on it. Press that button";
    }

    public override void check()
    {
        if (Input.GetKey(KeyCode.W))
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
