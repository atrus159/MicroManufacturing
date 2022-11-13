using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkQPressed : levelRequirementParent
{
    public checkQPressed(LayerStackHolder layers) : base(layers) {
        name = "Press the Q button";
        description = "At the top left corner of your keyboard is a button with 'Q' on it. Press that button";
    }

    public override void check()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
