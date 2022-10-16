using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rectangleTool : toolParent
{
    public rectangleTool() : base()
    {
        callOffCanvasFlag = true;
    }

    public override void onMouseDown(int i, int j)
    {
        paintCanvas.clickCoords.Set(i, j);
        paintCanvas.saveGrid();
    }

    public override void onClick(int i, int j)
    {
        if (paintCanvas.clickCoords.x != -1)
        {
            paintCanvas.loadGrid();
            int color = paintCanvas.curColor;
            int startI = i;
            int endI = paintCanvas.clickCoords.x;
            if (paintCanvas.clickCoords.x < startI)
            {
                startI = paintCanvas.clickCoords.x;
                endI = i;
            }
            int startJ = j;
            int endJ = paintCanvas.clickCoords.y;
            if (paintCanvas.clickCoords.y < startJ)
            {
                startJ = paintCanvas.clickCoords.y;
                endJ = j;
            }
            for (int iInd = startI; iInd <= endI; iInd++)
            {
                for (int jInd = startJ; jInd <= endJ; jInd++)
                {
                    if(paintCanvas.fillMode == 1)
                    {
                        paintCanvas.setPixel(iInd, jInd, color);
                    }
                    else if(iInd == startI || iInd == endI || jInd == startJ || jInd == endJ)
                    {
                        paintCanvas.setPixel(iInd, jInd, color);
                    }
                }
            }
            paintCanvas.texture.Apply();
        }

    }

    public override void onRelease(int i, int j)
    {
        paintCanvas.clickCoords.Set(-1, -1);
    }

}
