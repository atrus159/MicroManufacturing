using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_creator : MonoBehaviour
{

    public GameObject SpinCaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            makeSpinCaster();
        }
    }


    public void makeSpinCaster()
    {
        GameObject newSpin = Instantiate(SpinCaster);
        newSpin.transform.position = new Vector3(-9, -27, 19);
        newSpin.transform.rotation = Quaternion.Euler(-90, 0, -90);
    }
}
