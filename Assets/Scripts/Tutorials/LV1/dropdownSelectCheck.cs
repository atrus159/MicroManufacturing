using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropdownSelectCheck : checkParent
{
    // Start is called before the first frame update

    public override bool check()
    {
        if(layers.curMaterial == control.materialType.aluminum)
        {
            return true;
        }
        return false;
    }
}
