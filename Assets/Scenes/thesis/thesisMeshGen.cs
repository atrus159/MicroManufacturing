using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thesisMeshGen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject mg = GameObject.Find("MeshGenerator");
        mg.GetComponent<meshGenerator>().grid.set(BitGrid.circle());
        mg.GetComponent<meshGenerator>().layerHeight = 0.1f;
        mg.GetComponent<meshGenerator>().initialize();
        //mg.GetComponent<meshMaterial>().initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
