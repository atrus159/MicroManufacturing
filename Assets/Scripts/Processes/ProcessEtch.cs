using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessEtch : ProcessParent
{


    public override bool CallStep(int i)
    {
        bool toReturn = layerStackHold.etchLayer(layerStackHold.curMaterial, -i-1);
        layerStackHold.clearDeletes();
        if(!toReturn)
        {
            ErrorMessage = "No accessable material to etch!";
        }
        return toReturn;
    }

    public override void OnValueChanged(float newValue)
    {
        curStep = - (int) newValue;
        layerStackHold.sliceDeposits(curStep);
    }

    public override void updateSchematics() {

        GameObject schematicManagerObject = GameObject.Find("schematicManager");

        if (schematicManagerObject)
        {
            schematicManagerObject.GetComponent<schematicManager>().updateSchematic();
            schematicManagerObject.GetComponent<schematicManager>().updateText("Etch");
        }
    }

}
