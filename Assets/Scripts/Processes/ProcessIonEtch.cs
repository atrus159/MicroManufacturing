using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessIonEtch : ProcessParent
{


    public override bool CallStep(int i)
    {
        layerStackHold.etchLayer(control.materialType.aluminum, -3*i);
        layerStackHold.etchLayer(control.materialType.aluminum, -3*i+1);
        layerStackHold.etchLayer(control.materialType.aluminum, -3*i+2);
        layerStackHold.etchLayer(control.materialType.chromium, -2*i);
        layerStackHold.etchLayer(control.materialType.chromium, -2*i+1);
        layerStackHold.etchLayer(control.materialType.gold, -i);
        //layerStackHold.etchLayer(control.materialType.aluminum, -i);
        //layerStackHold.etchLayer(control.materialType.chromium, -i);
        layerStackHold.clearDeletes();
        return true;
    }

    public override void OnValueChanged(float newValue)
    {
        curStep = - (int) newValue;
        layerStackHold.sliceDeposits(curStep);
    }


}
