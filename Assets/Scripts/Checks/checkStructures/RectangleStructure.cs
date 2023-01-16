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
    control.materialType suroundingMaterial;


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

    public RectangleStructure(control.materialType materialType, Vector3Int minDims, Vector3Int maxDims, int numRectangles, bool[] flagVector, control.materialType surroundingMaterial) : base(materialType){
        this.minDims= minDims;
        this.maxDims= maxDims;
        this.flagVector= flagVector;
        this.numRectangles= numRectangles;
        //0: are the dimensions absolute (as opposed to general)?
        //1: should the surrounding material matter?
        this.suroundingMaterial= surroundingMaterial;
     }



    override public satisfyResult satisfy(satisfyResult starting)
    {
        int index = starting.layer;

        bitMap thisLayer;
        if (this.materialType != control.materialType.empty)
        {
            thisLayer = bitMap.zeros();
            foreach (GameObject curDeposit in layers.depLayers[index])
            {

                if (curDeposit.GetComponent<meshMaterial>().myMaterial == materialType)
                {
                    thisLayer = bitMap.union(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
                }
            }
        }
        else
        {
            thisLayer = bitMap.ones();
            foreach (GameObject curDeposit in layers.depLayers[index])
            {
                thisLayer = bitMap.emptyIntersect(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
            }
        }

        List<rectangle> features = getRectangles(thisLayer);
        features = cullRectangles(features, thisLayer, index);
        List<rectangle> result = new List<rectangle>();
 
        while (index >= 0 && index <= this.layers.topLayer +1 )
        {
            if (Mathf.Abs(index - starting.layer) >= maxDims.y)
            {
                break;
            }

            if (this.materialType != control.materialType.empty)
            {
                thisLayer = bitMap.zeros();
                foreach (GameObject curDeposit in layers.depLayers[index])
                {

                    if (curDeposit.GetComponent<meshMaterial>().myMaterial == materialType)
                    {
                        thisLayer = bitMap.union(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
                    }
                }
            }
            else
            {
                thisLayer = bitMap.ones();
                foreach (GameObject curDeposit in layers.depLayers[index])
                {
                    thisLayer = bitMap.emptyIntersect(thisLayer, curDeposit.GetComponent<meshGenerator>().grid);
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
                    if (Mathf.Abs(index - starting.layer) >= minDims.y)
                    {
                        result.Add(features[i]);
                    }
                    features.RemoveAt(i);
                    continue;
                }
            }

            //if features are terminating in the right range, don't put them on the naughty list
            index += starting.direction;
        }

        satisfyResult toReturn = new satisfyResult();
        toReturn.layer = index;
        if(result.Count >= numRectangles)
        {
            toReturn.satisfied = true;
        }
        else
        {
            toReturn.satisfied = false;
        }
        return toReturn;
    }


    List<rectangle> cullRectangles(List<rectangle> curRects, bitMap grid, int layerIndex)
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
                    break;
                }else if(height < minDims.x || height > maxDims.x || width < minDims.z || width > maxDims.z)
                {
                    curRects.RemoveAt(i);
                    break;

                }
            }

            if (flagVector[1])
            {
                for(int iInd = rect.getLowLeft().x -1; iInd <= rect.getUpRight().x + 1; iInd++) { 
                    for(int jInd = rect.getLowLeft().y-1; jInd <= rect.getUpRight().y+1; jInd++)
                    {
                        if (iInd < 0 || iInd >= bitMap.gridWidth || jInd < 0 || jInd >= bitMap.gridHeight)
                        {
                            break;
                        }

                        if (iInd >= rect.getLowLeft().x && iInd <= rect.getUpRight().x && jInd >= rect.getLowLeft().y && jInd <= rect.getUpRight().y)
                        {
                            break;
                        }


                        bool anyFailed = false;
                        if (suroundingMaterial == control.materialType.empty)
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
                                    if(curDeposit.GetComponent<meshMaterial>().myMaterial != suroundingMaterial)
                                    {
                                        anyFailed = true;
                                        break;
                                    }
                                }
                            }

                        }

                        if (anyFailed)
                        {
                            curRects.RemoveAt(i);
                            goto NextRectangle;
                        }
                    }
                }
            }
        NextRectangle: int placeHolder = 0;
        }
        return curRects;
    }


    List<rectangle> getRectangles(bitMap grid)
    {
        List<rectangle> toReturn = new List<rectangle>();
        int state = 0;
        for(int i = 0; i< bitMap.gridWidth; i++)
        {
            for(int j = 0; j<bitMap.gridHeight; j++)
            {
                switch (state)
                {
                    case 0:
                        if(grid.getPoint(i,j) == 1)
                        {
                            bool anyFound = false;
                            foreach(rectangle curRectangle in toReturn)
                            {
                                if(curRectangle.getCurState() == 1 && j <= curRectangle.getUpRight().y && j >= curRectangle.getLowLeft().y )
                                {
                                    anyFound = true;
                                    break;
                                }
                            }
                            if (!anyFound)
                            {
                                rectangle newRect = new rectangle();
                                newRect.setCurState(1);
                                newRect.setLowLeft(new Vector2Int(i, j));
                                newRect.setUpRight(new Vector2Int(-1, -1));
                                toReturn.Add(newRect);
                            }
                            state = 1;
                        }
                        break;
                   case 1:
                        if (grid.getPoint(i, j) == 0 || j == bitMap.gridHeight-1)
                        {
                            rectangle activeRect = new rectangle();
                            foreach(rectangle curRectangle in toReturn)
                            {
                                if (curRectangle.getCurState() == 1)
                                {
                                    activeRect = curRectangle;
                                    break;
                                }
                            }
                            if(activeRect.getUpRight().y == -1)
                            {
                                activeRect.setUpRight(new Vector2Int(i, j-1));
                            }else if(activeRect.getUpRight().y == j-1)
                            {
                                if(i +1 >= bitMap.gridWidth || grid.getPoint(i+1,j-1) == 0)
                                {
                                    activeRect.setCurState(0);
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

}