using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RectangleStructure : CheckStructComponent
{

    Vector3Int minDims;
    Vector3Int maxDims;
    bool[] flagVector;
    int numRectangles;
    control.materialType surroundingMaterial;


    class rectangle
    {
        Vector2Int lowLeft;
        Vector2Int upRight;
        int curState;

        public Vector2Int getLowLeft() { return lowLeft; }
        public Vector2Int getUpRight() { return upRight; }
        public void setLowLeft(Vector2Int value) { lowLeft = value; }

        public void setUpRight(Vector2Int value) { upRight = value; }

        public int getCurState() { return curState; }

        public void setCurState(int state) { curState = state; }
    }

    public RectangleStructure(control.materialType materialType, int direction, Vector3Int minDims, Vector3Int maxDims, int numRectangles, bool[] flagVector, control.materialType surroundingMaterial = control.materialType.empty) : base(materialType, direction){
        this.minDims= minDims;
        this.maxDims= maxDims;
        this.flagVector= flagVector;
        this.numRectangles= numRectangles;
        //0: are the dimensions absolute (as opposed to general)?
        //1: should the surrounding material matter?
        //2: should the max height be disregarded?
        this.surroundingMaterial= surroundingMaterial;
     }



    override public satisfyResult satisfy(satisfyResult starting, int layerIndex = 0)
    {
        errors = new List<string>();
        int startingLayer = starting.startingLayers[layerIndex];
        int index = startingLayer;
        satisfyResult toReturn = new satisfyResult();
        toReturn.startingLayers = new List<int>();
        toReturn.direction = this.direction;

        BitGrid thisLayer;
        if (this.materialType != control.materialType.empty)
        {
            thisLayer = BitGrid.zeros();
            foreach (GameObject curDeposit in layers.depLayers[index])
            {

                if (curDeposit.GetComponent<meshMaterial>().myMaterial == materialType)
                {
                    thisLayer = BitGrid.union(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
                }
            }
        }
        else
        {
            thisLayer = BitGrid.ones();
            foreach (GameObject curDeposit in layers.depLayers[index])
            {
                thisLayer = BitGrid.emptyIntersect(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
            }
        }

        List<rectangle> features = getRectangles(thisLayer);
        if(features.Count < numRectangles)
        {
            toReturn.satisfied = false;
            errors.Add("Too few boxes found");
            return toReturn;
        }
        features = cullRectangles(features, thisLayer, index);
        if(features.Count < numRectangles)
        {
            toReturn.satisfied = false;
            errors.Add("Not enough boxes with correct size + border.");
            return toReturn;

        }
        List<rectangle> result = new List<rectangle>();
 
        while (index >= 0 && (index <= this.layers.topLayer +1 || (materialType == control.materialType.empty && index < 100)) )
        {
            if (Mathf.Abs(index - startingLayer) > maxDims.y)
            {
                break;
            }

            if (this.materialType != control.materialType.empty)
            {
                thisLayer = BitGrid.zeros();
                foreach (GameObject curDeposit in layers.depLayers[index])
                {

                    if (curDeposit.GetComponent<meshMaterial>().myMaterial == materialType)
                    {
                        thisLayer = BitGrid.union(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
                    }
                }
            }
            else
            {
                thisLayer = BitGrid.ones();
                foreach (GameObject curDeposit in layers.depLayers[index])
                {
                    thisLayer = BitGrid.emptyIntersect(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
                }
            }

            List<rectangle> compareFeatures = getRectangles(thisLayer);
            compareFeatures = cullRectangles(compareFeatures, thisLayer, index);

            for (int i = features.Count - 1; i >= 0; i--)
            {
                bool anyFound = false;
                foreach(rectangle curCompareRect in compareFeatures)
                {
                    if (curCompareRect.getLowLeft().Equals(features[i].getLowLeft()) && curCompareRect.getUpRight().Equals(features[i].getUpRight()))
                    {
                        anyFound = true;
                        break;
                    }
                }
                if(!anyFound)
                {
                    if (Mathf.Abs(index - startingLayer) >= minDims.y)
                    {
                        result.Add(features[i]);
                        if(direction == 1)
                        {
                            toReturn.startingLayers.Add(index);
                        }
                        else
                        {
                            toReturn.startingLayers.Add(startingLayer - 1);
                        }

                    }
                    else
                    {
                        errors.Add("Found box too short.");
                    }
                    features.RemoveAt(i);
                    continue;
                }
            }

            if (flagVector[2] && Mathf.Abs(index - startingLayer) >= minDims.y)
            {
                break;
            }

            index += starting.direction;
        }

        if(result.Count >= numRectangles || (flagVector[2] && features.Count >= numRectangles))
        {
            toReturn.satisfied = true;
        }
        else
        {
            errors.Add("Not enough boxes with correct height.");
            toReturn.satisfied = false;
        }
        return toReturn;
    }


    List<rectangle> cullRectangles(List<rectangle> curRects, BitGrid grid, int layerIndex)
    {
        
        for(int i = curRects.Count-1; i >= 0; i--)
        {
            rectangle rect = curRects[i];
            int width = rect.getUpRight().x - rect.getLowLeft().x;
            int height = rect.getUpRight().y - rect.getLowLeft().y;
            if(width < minDims.x || width > maxDims.x || height < minDims.z || height > maxDims.z)
            {
                if (flagVector[0])
                {
                    curRects.RemoveAt(i);
                    errors.Add("Found rectangle wrong size:" + "(" + width + ", " + height + ")");
                    goto NextRectangle;
                }
                else if(height < minDims.x || height > maxDims.x || width < minDims.z || width > maxDims.z)
                {
                    errors.Add("Found rectangle wrong size:" + "(" + width + ", " + height + ")");
                    curRects.RemoveAt(i);
                    goto NextRectangle;

                }
            }

            if (flagVector[1])
            {
                for(int iInd = rect.getLowLeft().x -1; iInd <= rect.getUpRight().x + 1; iInd++) { 
                    for(int jInd = rect.getLowLeft().y-1; jInd <= rect.getUpRight().y+1; jInd++)
                    {
                        if (iInd < 0 || iInd >= BitGrid.gridWidth || jInd < 0 || jInd >= BitGrid.gridHeight)
                        {
                            break;
                        }

                        if (iInd >= rect.getLowLeft().x && iInd <= rect.getUpRight().x && jInd >= rect.getLowLeft().y && jInd <= rect.getUpRight().y)
                        {
                            break;
                        }


                        bool anyFailed = false;
                        if (surroundingMaterial == control.materialType.empty)
                        {
                            foreach (GameObject curDeposit in layers.depLayers[layerIndex])
                            {
                                if (curDeposit.GetComponent<meshGenerator>().grid.getPoint(iInd, jInd) != 0)
                                {
                                    anyFailed = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (GameObject curDeposit in layers.depLayers[layerIndex])
                            {
                                if (curDeposit.GetComponent<meshGenerator>().grid.getPoint(iInd, jInd) != 0)
                                {
                                    if(curDeposit.GetComponent<meshMaterial>().myMaterial != surroundingMaterial)
                                    {
                                        anyFailed = true;
                                        break;
                                    }
                                }
                            }

                        }

                        if (anyFailed)
                        {
                            errors.Add("Found rectangle wrong border.");
                            curRects.RemoveAt(i);
                            goto NextRectangle;
                        }
                    }
                }
            }
        NextRectangle:;
        }
        return curRects;
    }


    List<rectangle> getRectangles(BitGrid grid)
    {
        List<rectangle> toReturn = new List<rectangle>();
        int state = 0;
        for(int i = 0; i< BitGrid.gridWidth; i++)
        {
            for(int j = 0; j<=BitGrid.gridHeight; j++)
            {
                switch (state)
                {
                    case 0:
                        if(j != BitGrid.gridHeight && grid.getPoint(i,j) == 1)
                        {
                            bool anyFound = false;
                            foreach(rectangle curRectangle in toReturn)
                            {
                                if(curRectangle.getCurState() == 1 && j <= curRectangle.getUpRight().y && j >= curRectangle.getLowLeft().y )
                                {
                                    curRectangle.setCurState(2);
                                    anyFound = true;
                                    break;
                                }
                            }
                            if (!anyFound)
                            {
                                rectangle newRect = new rectangle();
                                newRect.setCurState(2);
                                newRect.setLowLeft(new Vector2Int(i, j));
                                newRect.setUpRight(new Vector2Int(-1, -1));
                                toReturn.Add(newRect);
                            }
                            state = 1;
                        }
                        break;
                   case 1:
                        if (j == BitGrid.gridHeight || grid.getPoint(i, j) == 0 )
                        {
                            rectangle activeRect = new rectangle();
                            foreach(rectangle curRectangle in toReturn)
                            {
                                if (curRectangle.getCurState() == 2)
                                {
                                    activeRect = curRectangle;
                                    break;
                                }
                            }
                            if(activeRect.getUpRight().y == -1)
                            {
                                activeRect.setUpRight(new Vector2Int(i, j-1));
                                activeRect.setCurState(1);
                            }else if(activeRect.getUpRight().y == j-1)
                            {
                                if(i +1 >= BitGrid.gridWidth || grid.getPoint(i+1,j-1) == 0)
                                {
                                    activeRect.setCurState(0);
                                }
                                else
                                {
                                    activeRect.setCurState(1);
                                }
                                activeRect.setUpRight(new Vector2Int(i, j-1));
                            }else
                            {
                                activeRect.setCurState(0);
                            }
                            state = 0;
                        }
                        break;
                }
            }
        }

        return toReturn;
    }


    override public CheckStructComponent clone()
    {
        return new RectangleStructure(materialType, direction, minDims, maxDims, numRectangles, flagVector, surroundingMaterial);
    }

}
