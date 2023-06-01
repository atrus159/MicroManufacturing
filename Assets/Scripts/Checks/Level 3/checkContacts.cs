using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class checkContacts : levelRequirementParent
{
    ProbeScript ps;
    public override void onStart()
    {
        base.onStart();
        name = "Use gold to connect the two electrical contacts.";
        description = "The red and black boxes show where the electrical signal will be applied";
        ps = GameObject.Find("probes").GetComponent<ProbeScript>();
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
