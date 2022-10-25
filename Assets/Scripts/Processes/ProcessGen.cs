using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessGen : ProcessParent
{


    public override void CallStep(int i)
    {
        layerStackHold.depositLayer(layerStackHold.curMaterial, bitMap.ones(), i + 1);
    }

    public override void OnValueChanged(float newValue)
    {
        curStep = (int) newValue;
        layerStackHold.sliceDeposits(curStep);
    }


}
