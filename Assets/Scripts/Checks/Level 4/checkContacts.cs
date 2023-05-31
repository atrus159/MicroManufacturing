using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class checkContacts : levelRequirementParent
{
    public override void onStart()
    {
        base.onStart();
        name = "Use gold or aluminum to connect the two electrical contacts.";
        description = "Connect the red, blue and green contacts together, without mixing colors.";
        checkOutsideEdits = true;
    }

    public override void check()
    {
        // check
        if (GameObject.Find("New Process") && GameObject.Find("LayerStack").GetComponent<LayerStackHolder>().wetEtch == true)
        {
            met = true;
        }
        else
        {
            met = false;
        }
    }
}
