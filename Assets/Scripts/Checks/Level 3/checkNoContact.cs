using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class checkNoContact : levelRequirementParent
{
    ProbeScript red;
    ProbeScript blue;
    ProbeScript green;
    public override void onStart()
    {
        base.onStart();
        name = "Make sure there's no connection between differently colored electrodes!";
        description = "You can use silicon-dioxide to insulate your connections";
        red = GameObject.Find("probesRed").GetComponent<ProbeScript>();
        blue = GameObject.Find("probesBlue").GetComponent<ProbeScript>();
        green = GameObject.Find("probesGreen").GetComponent<ProbeScript>();
    }

    public override void check()
    {
        // check
        if (!red.getCrossConectionStatus(blue) && !red.getCrossConectionStatus(green) && !blue.getCrossConectionStatus(green))
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
