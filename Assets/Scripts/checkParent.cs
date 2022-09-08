using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkParent : MonoBehaviour
{
    public tutorialParent myTutorial;
    LayerStackHolder layers;
    // Start is called before the first frame update
    void Start()
    {
        layers = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool result = check();
        if (result)
        {
            myTutorial.stepFlag = true;
            Destroy(gameObject);
        }
    }

    virtual public bool check()
    {
        return true;
    }
}
