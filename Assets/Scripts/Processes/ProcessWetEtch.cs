using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessWetEtch : ProcessParent
{


    public override bool CallStep(int i)
    {
        bool toReturn = false;
        toReturn |= layerStackHold.etchLayer(layerStackHold.curMaterial, -i-1);
        toReturn |= layerStackHold.etchLayerAround(layerStackHold.curMaterial, -i-1);
        layerStackHold.clearDeletes();
        if (!toReturn)
        {
            ErrorMessage = "No accessable material to etch!";
        }
        return toReturn;
    }

    public override void OnValueChanged(float newValue)
    {
        curStep = -(int)newValue;
        layerStackHold.sliceDeposits(curStep);
    }

    public override void updateSchematics() {

        GameObject schematicManagerObject = GameObject.Find("schematicManager");

        if (schematicManagerObject) {
            schematicManagerObject.GetComponent<schematicManager>().updateSchematic();
            schematicManagerObject.GetComponent<schematicManager>().updateText("Wet Etch");
        }

    }


}
