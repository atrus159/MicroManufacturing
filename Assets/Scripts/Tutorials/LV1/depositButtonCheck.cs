using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depositButtonCheck : checkParent
{
    // Start is called before the first frame update

    public override bool check()
    {
        if(GameObject.Find("New Process"))
        {
            return true;
        }
        return false;
    }
}
