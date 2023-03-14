using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class AmorphousStructure : CheckStructComponent
{

    Vector3Int minDims;
    Vector3Int maxDims;
    bool[] flagVector;
    int numAmorphs;
    control.materialType surroundingMaterial;


    class amorphous
    {
        public BitGrid grid;
        public Vector2Int lowLeft;
        public Vector2Int upRight;
        public amorphous()
        {
            grid = BitGrid.zeros();
            lowLeft = new Vector2Int();
            upRight = new Vector2Int();
        }
        public void updateBounds()
        {
            int minX = BitGrid.gridWidth;
            int minY = BitGrid.gridHeight;
            int maxX = 0;
            int maxY = 0;
            for(int i = 0; i< BitGrid.gridWidth; i++)
            {
                for(int j = 0; j< BitGrid.gridHeight; j++)
                {
                    if(grid.getPoint(i,j) != 0)
                    {
                        if(i< minX) { minX = i; }
                        if(i> maxX) { maxX = i; }
                        if(j< minY) { minY = j; }
                        if(j> maxY) { maxY = j; }
                    }
                }
            }
            lowLeft.x = minX;
            lowLeft.y = minY;
            upRight.x = maxX;
            upRight.y = maxY;
        }
    }

    public AmorphousStructure(control.materialType materialType, int direction, Vector3Int minDims, Vector3Int maxDims, int numAmorphs, bool[] flagVector, control.materialType surroundingMaterial = control.materialType.empty) : base(materialType, direction)
    {
        this.minDims = minDims;
        this.maxDims = maxDims;
        this.flagVector = flagVector;
        this.numAmorphs = numAmorphs;
        //0: are the dimensions absolute (as opposed to general)?
        //1: should the surrounding material matter?
        //2: should the max height be disregarded?
        this.surroundingMaterial = surroundingMaterial;
    }

    List<amorphous> getAmorphs(BitGrid grid)
    {
        List<amorphous> toReturn = new List<amorphous>();
        BitGrid curGrid = new BitGrid();
        curGrid.set(grid);
        for(int i = 0; i< BitGrid.gridWidth; i++)
        {
            for(int j = 0; j< BitGrid.gridHeight; j++)
            {
                if(curGrid.getPoint(i,j) == 1)
                {
                    BitGrid newGrid = BitGrid.zeros();
                    newGrid.setPoint(i, j, 1);
                    newGrid = BitGrid.getIntersectedRegions(newGrid, curGrid);
                    amorphous newAmorph = new amorphous();
                    newAmorph.grid.set(newGrid);
                    newAmorph.updateBounds();
                    toReturn.Add(newAmorph);
                    curGrid = BitGrid.emptyIntersect(curGrid, newGrid);
                }
            }
        }

        return toReturn;
    }

    List<amorphous> cullAmorphs(List<amorphous> curAmorphs, BitGrid grid, int layerIndex)
    {

        for (int i = curAmorphs.Count - 1; i >= 0; i--)
        {
            amorphous amorph = curAmorphs[i];
            int width = amorph.upRight.x - amorph.lowLeft.x;
            int height = amorph.upRight.y - amorph.lowLeft.y;
            if (width < minDims.x || width > maxDims.x || height < minDims.z || height > maxDims.z)
            {
                if (flagVector[0])
                {
                    curAmorphs.RemoveAt(i);
                    errors.Add("Found amorph wrong size:" + "(" + width + ", " + height + ")");
                    goto NextAmorph;
                }
                else if (height < minDims.x || height > maxDims.x || width < minDims.z || width > maxDims.z)
                {
                    errors.Add("Found amorph wrong size:" + "(" + width + ", " + height + ")");
                    curAmorphs.RemoveAt(i);
                    goto NextAmorph;

                }
            }

            if (flagVector[1])
            {
                BitGrid surounding = BitGrid.getBorderRegion(amorph.grid);
                BitGrid suroundingFill = BitGrid.zeros();
                foreach (GameObject curDeposit in layers.depLayers[layerIndex])
                {
                    BitGrid intersection = BitGrid.intersect(curDeposit.GetComponent<meshGenerator>().grid, surounding);
                    if (curDeposit.GetComponent<meshMaterial>().myMaterial == surroundingMaterial)
                    {
                        suroundingFill = BitGrid.union(suroundingFill, intersection);
                        continue;
                    }
                    if(!intersection.isEmpty()){
                        errors.Add("Found amorph wrong border");
                        curAmorphs.RemoveAt(i);
                        goto NextAmorph;
                    }
                }
                if(surroundingMaterial != control.materialType.empty)
                {
                    if (!surounding.isEqual(suroundingFill))
                    {
                        errors.Add("Found amorph insufficient border");
                        curAmorphs.RemoveAt(i);
                        goto NextAmorph;
                    }
                }
            }
            NextAmorph:;
        }
        return curAmorphs;
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

            List<amorphous> features = getAmorphs(thisLayer);
            if (features.Count < numAmorphs)
            {
                toReturn.satisfied = false;
                errors.Add("Too few morphs found");
                return toReturn;
            }
            features = cullAmorphs(features, thisLayer, index);
            if (features.Count < numAmorphs)
            {
                toReturn.satisfied = false;
                errors.Add("Not enough morphs with correct size.");
                return toReturn;

            }
            List<amorphous> result = new List<amorphous>();

            while (index >= 0 && (index <= this.layers.topLayer + 1 || (materialType == control.materialType.empty && index < 100)))
            {
                if (Mathf.Abs(index - startingLayer) >= maxDims.y)
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

                List<amorphous> compareFeatures = getAmorphs(thisLayer);
                compareFeatures = cullAmorphs(compareFeatures, thisLayer, index);

                for (int i = features.Count - 1; i >= 0; i--)
                {
                    bool anyFound = false;
                    foreach (amorphous curCompareAmorph in compareFeatures)
                    {
                        if (curCompareAmorph.grid.isEqual(features[i].grid))
                        {
                            anyFound = true;
                            break;
                        }
                    }
                    if (!anyFound)
                    {
                        if (Mathf.Abs(index - startingLayer) >= minDims.y)
                        {
                            result.Add(features[i]);
                            if (direction == 1)
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
                            errors.Add("Found structure too short.");
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

            if (result.Count >= numAmorphs || (flagVector[2] && features.Count >= numAmorphs))
            {
                toReturn.satisfied = true;
            }
            else
            {
                errors.Add("Not enough structures with correct height.");
                toReturn.satisfied = false;
            }
            return toReturn;
        }

    override public CheckStructComponent clone()
    {
        return new AmorphousStructure(materialType, direction, minDims, maxDims, numAmorphs, flagVector, surroundingMaterial);
    }

}
