using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elipseTool : toolParent
{
    public elipseTool() : base()
    {

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


                    if (Mathf.Abs(endI - startI) < 1 || Mathf.Abs(endJ - startJ) < 1)
                    {
                        paintCanvas.setPixel(iInd, jInd, color);
                        continue;
                    }

                    float len = (endI - startI)/2;
                    float height = (endJ - startJ)/2;

                    float fx = height * Mathf.Sqrt(1 - Mathf.Pow((iInd - len - startI) / len, 2)) + height + startJ;
                    float nfx = -height * Mathf.Sqrt(1 - Mathf.Pow((iInd - len - startI) / len, 2)) + height + startJ;
                    float fy = len * Mathf.Sqrt(1 - Mathf.Pow((jInd - height - startJ) / height, 2)) + len + startI;
                    float nfy = -len * Mathf.Sqrt(1 - Mathf.Pow((jInd - height - startJ) / height, 2)) + len + startI;

                    if (paintCanvas.fillMode == 1)
                    {
                        if (jInd+1 <= fx && jInd-1 >= nfx && iInd +1 <= fy && iInd - 1 >= nfy)
                        {
                            paintCanvas.setPixel(iInd, jInd, color);
                        }
                    }
                    else
                    {
                        if (jInd == fx || (jInd + 1 >= fx && jInd - 1 <= fx) || jInd == nfx || (jInd + 1 >= nfx && jInd - 1 <= nfx))
                        {
                            paintCanvas.setPixel(iInd, jInd, color);
                        }
                        if (iInd == fy || (iInd + 1 >= fy && iInd - 1 <= fy) || iInd == nfy || (iInd + 1 >= nfy && iInd - 1 <= nfy))
                        {
                            paintCanvas.setPixel(iInd, jInd, color);
                        }

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
