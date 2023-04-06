using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//the parent class for a level requirement. Implement this to make a new level requirement
public class levelRequirementParent : MonoBehaviour
{
    //contains the layerStackHolder so your check can see what deposits there are
    LayerStackHolder layers;

    //the name of the requirement. Displayed on the requirement tracker
    public string name;
    //a short description of the requirement. Displayed on the requirement tracker
    public string description;
    //stores whether the requirement has been met or not
    public bool met;

    public bool checkOutsideEdits;

    //set the name and description of your check in the constructor when you implement this class

    private void Start()
    {
        onStart();
    }
    virtual public void onStart()
    {
        this.layers = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
        met = false;
        checkOutsideEdits = false;
    }


    //implement this function with whatever check you are writing
    virtual public void check()
    {
        met = true;
    }

}
