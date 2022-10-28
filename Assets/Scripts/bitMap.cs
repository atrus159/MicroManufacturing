using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using System.Diagnostics;


//a class for handling and combining bitmaps. 
public class bitMap
{
    public static int gridWidth = 100;
    public static int gridHeight = 100;


    //a gridWidth x gridHeight grid of ints that are treated as bits. All numbers greater than zero are treated as 1 since some functions use 2 as a tracking flag
    int[,] grid;


    //constructor
    public bitMap()
    {
        grid = new int[bitMap.gridWidth, bitMap.gridHeight];

    }

    //sets a spesific index in the grid to a particular value
    public void setPoint(int i, int j, int val)
    {
        grid[i, j] = val;
    }

    public int getPoint(int i, int j)
    {
        return grid[i, j];
    }

    //sets the grid to that of another bitMap
    public void set(bitMap toSet)
    {
        for(int i = 0; i< bitMap.gridWidth; i++)
        {
            for(int j = 0; j< bitMap.gridHeight; j++)
            {
                grid[i, j] = toSet.getPoint(i,j);
            }
        }
    }

    //returns a bitMap which is the union of two other bitMaps
    public static bitMap union(bitMap ar1, bitMap ar2)
    {
        bitMap toReturn = new bitMap();

        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                if (ar1.getPoint(i,j) != 0 || ar2.getPoint(i, j) != 0)
                {
                    toReturn.setPoint(i,j,1);
                }
                else
                {
                    toReturn.setPoint(i, j, 0);
                }
            }
        }
        return toReturn;
    }

    //returns a bitMap which is the intersection of two other bitMaps
    public static bitMap intersect(bitMap ar1, bitMap ar2)
    {
        bitMap toReturn = new bitMap();

        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                if (ar1.getPoint(i, j) != 0 && ar2.getPoint(i, j) != 0)
                {
                    toReturn.setPoint(i, j, 1);
                }
                else
                {
                    toReturn.setPoint(i, j, 0);
                }
            }
        }
        return toReturn;
    }

    //returns a bitMap which is the intersect of ar1 and the compliment of ar2: in otherwords, the places where ar1 is 1 and ar2 is 0
    public static bitMap emptyIntersect(bitMap ar1, bitMap ar2)
    {
        bitMap toReturn = new bitMap();

        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
  
                if (ar1.getPoint(i, j) != 0 && ar2.getPoint(i, j) == 0)
                {
                    toReturn.setPoint(i, j, 1);
                }
                else
                {
                    toReturn.setPoint(i, j, 0);
                }
            }
        }
        return toReturn;
    }

    //returns a bitmap with the contiguous regions of ones in InterTo reachable by any contiguous regions of ones in interFrom
    public static bitMap getIntersectedRegions(bitMap interFrom, bitMap interTo)
    {
        bitMap toReturn = new bitMap();
        toReturn.set(interTo);

        for(int i = 0; i< bitMap.gridWidth; i++)
        {
            for(int j = 0; j< bitMap.gridHeight; j++)
            {
                if(interFrom.getPoint(i,j) != 0)
                {
                    fill(toReturn, i, j);
                }
            }
        }


        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                if (toReturn.getPoint(i, j) == 3)
                {
                    toReturn.setPoint(i, j, 1);
                }
                else
                {
                    toReturn.setPoint(i, j, 0);
                }
            }
        }
        return toReturn;
    }

    //helper function for getIntersectedRegions, performs a ms paint style fill bucket at the point i,j, filling all 1s with 3s
    static void fill(bitMap toFill, int iInput, int jInput)
    {
        Stack<Vector2> indexes = new Stack<Vector2>();

        indexes.Push(new Vector2(iInput, jInput));


        while(indexes.Count > 0)
        {

            Vector2 curInd = indexes.Pop();
            int i = (int)curInd.x;
            int j = (int)curInd.y;
            if (toFill.getPoint(i, j) == 0 || toFill.getPoint(i, j) == 3)
            {
                continue;
            }

            toFill.setPoint(i, j, 3);

            for (int iInd = i - 1; iInd <= i + 1; iInd++)
            {
                for (int jInd = j - 1; jInd <= j + 1; jInd++)
                {
                    if (iInd < 0 || iInd >= bitMap.gridWidth || jInd < 0 || jInd >= bitMap.gridHeight)
                    {
                        continue;
                    }
                    indexes.Push(new Vector2(iInd, jInd));
                }
            }
        }
    }

    //returns a bitmap with a line of 1s around every spot that had 1s in map
    public static bitMap getBorderRegion(bitMap map)
    {
        bitMap toReturn = bitMap.zeros();
        for(int i = 0; i< bitMap.gridWidth; i++)
        {
            for(int j = 0; j<bitMap.gridHeight; j++)
            {
                if (map.getPoint(i, j) == 0)
                {
                    bool breakLoop = false;
                    for (int iInd = i - 1; iInd <= i + 1; iInd++)
                    {
                        for (int jInd = j - 1; jInd <= j + 1; jInd++)
                        {
                            if (iInd < 0 || iInd >= bitMap.gridWidth || jInd < 0 || jInd >= bitMap.gridHeight)
                            {
                                continue;
                            }
                            if (map.getPoint(iInd, jInd) == 0)
                            {
                                continue;
                            }
                            toReturn.setPoint(i, j, 1);
                            breakLoop = true;
                            break;
                        }
                        if (breakLoop)
                        {
                            break;
                        }
                    }
                }
            }
        }
        return toReturn;
    }

    //returns a bitmap which is the inversion of input
    public static bitMap invert(bitMap input)
    {
        bitMap toReturn = new bitMap();
        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for(int j = 0; j<bitMap.gridHeight; j++)
            {
                if(input.getPoint(i,j) == 0)
                {
                    toReturn.setPoint(i, j, 1);
                }
                else
                {
                    toReturn.setPoint(i, j, 0);
                }
            }
        }
        return toReturn;
    }

    //returns a new bitMap that is all ones
    public static bitMap ones()
    {

        bitMap toReturn = new bitMap();
        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                toReturn.setPoint(i, j, 1);
            }
        }
        return toReturn;
    }

    //returns a new bitMap that is all zeros
    public static bitMap zeros()
    {

        bitMap toReturn = new bitMap();
        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                toReturn.setPoint(i, j, 0);
            }
        }
        return toReturn;
    }


    //returns a new bitMap that is all zeros with a diagonal line of ones
    public static bitMap line()
    {
        bitMap toReturn = new bitMap();
        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                if (i == j)
                {
                    toReturn.setPoint(i, j, 1);
                }
                else
                {
                    toReturn.setPoint(i, j, 0);
                }
            }
        }
        return toReturn;
    }

    //returns a new bitMap that is all zeros with a circle of ones
    public static bitMap circle()
    {
        bitMap toReturn = new bitMap();
        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                if (Mathf.Sqrt(Mathf.Pow(i - bitMap.gridWidth * 0.5f, 2f) + Mathf.Pow(j - bitMap.gridHeight * 0.5f, 2f)) < bitMap.gridWidth * 0.25f)
                {
                    toReturn.setPoint(i, j, 1);
                }
                else
                {
                    toReturn.setPoint(i, j, 0);
                }
            }
        }
        return toReturn;
    }

    //returns if the bitMap is all zeros
    public bool isEmpty()
    {
        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            for (int j = 0; j < bitMap.gridHeight; j++)
            {
                if (grid[i, j] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void printGrid()
    {
        for (int i = 0; i < bitMap.gridWidth; i++)
        {
            System.Diagnostics.Debug.Write("[ ");
            for (int j = 0; j < bitMap.gridHeight; i++)
            {
                System.Diagnostics.Debug.Write(grid[i, j] + ", ");
            }
            System.Diagnostics.Debug.WriteLine("]");
        }
    }
}
