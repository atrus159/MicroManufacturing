using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brushTool : toolParent
{
    public brushTool() : base()
    {

    }


    override public void onClick(int i, int j)
    {
        paintCanvas.setPixel(i, j, paintCanvas.curColor);
        paintCanvas.texture.Apply();
    }

    public override void onRelease(int i, int j)
    {
        paintCanvas.addState();
    }

}
