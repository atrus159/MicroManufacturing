using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPanCheck : checkParent
{
    int t;
    Quaternion initialRotation;
    bool moved;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        t = 0;
        moved = false;
        initialRotation = GameObject.Find("Main Camera").transform.rotation;
    }

    override public bool check()
    {
        if (!moved)
        {
            Quaternion currentRotation = GameObject.Find("Main Camera").transform.rotation;
            if (Mathf.Abs(Quaternion.Angle(initialRotation, currentRotation)) > 30)
            {
                moved = true;
            }
            return false;
        }
        else
        {
            t++;
            if(t > 7.0f / Time.deltaTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
