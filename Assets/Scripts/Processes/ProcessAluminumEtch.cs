using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessAluminumEtch : ProcessParent
{


    public override void CallStep(int i)
    {
        layerStackHold.etchLayer(layerStackHold.curMaterial, -i);
        layerStackHold.etchLayerAround(layerStackHold.curMaterial, -i);
        layerStackHold.clearDeletes();
    }

    public override void OnValueChanged(float newValue)
    {
        curStep = -(int)newValue;
        layerStackHold.sliceDeposits(curStep);
    }


}
