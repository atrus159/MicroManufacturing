using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickMeDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") || TextManager.instance.skipFlag)
        {
            Destroy(gameObject);
        }
    }

    public void destroyMe()
    {
        Destroy(gameObject);
    }
}
