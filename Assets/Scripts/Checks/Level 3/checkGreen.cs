using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class checkGreen : levelRequirementParent
{
    ProbeScript ps;
    public override void onStart()
    {
        base.onStart();
        name = "Connect the green contacts together.";
        description = "Use gold to make the connection";
        ps = GameObject.Find("probesGreen").GetComponent<ProbeScript>();
    }

    public override void check()
    {
        // check
        if (ps.getConectionStatus())
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
