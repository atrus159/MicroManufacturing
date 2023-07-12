using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class checkBlue : levelRequirementParent
{
    ProbeScript ps;
    public override void onStart()
    {
        base.onStart();
        name = "Connect the (B1) and (B2) contacts together.";
        description = "Use gold to make the connection";
        ps = GameObject.Find("probesBlue").GetComponent<ProbeScript>();
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
