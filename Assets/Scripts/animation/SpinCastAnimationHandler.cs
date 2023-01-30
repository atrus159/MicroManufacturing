using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCastAnimationHandler : MonoBehaviour
{
    Animator plateSpinAnimator;
    // Start is called before the first frame update
    void Start()
    {
        plateSpinAnimator = GameObject.Find("Play Area").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void plateSpin()
    {      
        plateSpinAnimator.SetTrigger("TriggerSpin");
    }

    public void destroyMe()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
        Destroy(gameObject);
    }

    public void makePhotoResist()
    {
        GameObject layer = GameObject.Find("LayerStack");
        layer.GetComponent<LayerStackHolder>().makePhotoResist();
    }

}
