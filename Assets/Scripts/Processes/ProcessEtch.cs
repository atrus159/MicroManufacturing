using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessEtch : ProcessParent
{


    public override void CallStep(int i)
    {
        layerStackHold.etchLayer(layerStackHold.curMaterial, -i);
        layerStackHold.clearDeletes();
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
