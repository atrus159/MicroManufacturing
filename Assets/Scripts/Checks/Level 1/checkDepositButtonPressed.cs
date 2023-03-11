using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class checkDepositButtonPressed : levelRequirementParent
{
    public checkDepositButtonPressed(LayerStackHolder layers) : base(layers) {
        name = "Press the Deposit button to start sputtering";
        description = "The Deposit button is in the bottom right.";
        checkOutsideEdits= true;
    }

    public override void check()
    {
        if (GameObject.Find("New Process"))
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
