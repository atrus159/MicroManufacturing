using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessGen : ProcessParent
{


    public override void CallStep(int i)
    {
        if(layerStackHold.curMaterial == control.materialType.gold)
        {
            layerStackHold.depositGoldLayer(layerStackHold.curMaterial, BitGrid.ones(), i + 1);
        }
        else
        {
            layerStackHold.depositLayer(layerStackHold.curMaterial, BitGrid.ones(), i + 1);
        }
    }

    public override void OnValueChanged(float newValue)
    {
        curStep = (int) newValue;
        layerStackHold.sliceDeposits(curStep);
    }

    public override void updateSchematics() {
        GameObject schematicManagerObject = GameObject.Find("schematicManager");

        if (schematicManagerObject)
        {
            schematicManagerObject.GetComponent<schematicManager>().updateSchematic();
            schematicManagerObject.GetComponent<schematicManager>().updateText("Deposit");
        }
    }

}
