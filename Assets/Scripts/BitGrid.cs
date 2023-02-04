using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using System.Diagnostics;


//a class for handling and combining bitmaps. 
public class BitGrid
{
    public static int gridWidth = 100;
    public static int gridHeight = 100;


    //a gridWidth x gridHeight grid of ints that are treated as bits. All numbers greater than zero are treated as 1 since some functions use 2 as a tracking flag
    int[,] grid;


    //constructor
    public BitGrid()
    {
        grid = new int[BitGrid.gridWidth, BitGrid.gridHeight];

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

    //sets the grid to that of another BitGrid
    public void set(BitGrid toSet)
    {
        for(int i = 0; i< BitGrid.gridWidth; i++)
        {
            for(int j = 0; j< BitGrid.gridHeight; j++)
            {
                grid[i, j] = toSet.getPoint(i,j);
            }
        }
    }

    //returns a BitGrid which is the union of two other BitGrids
    public static BitGrid union(BitGrid ar1, BitGrid ar2)
    {
        BitGrid toReturn = new BitGrid();

        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
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

    //returns a BitGrid which is the intersection of two other BitGrids
    public static BitGrid intersect(BitGrid ar1, BitGrid ar2)
    {
        BitGrid toReturn = new BitGrid();

        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
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

    //returns a BitGrid which is the intersect of ar1 and the compliment of ar2: in otherwords, the places where ar1 is 1 and ar2 is 0
    public static BitGrid emptyIntersect(BitGrid ar1, BitGrid ar2)
    {
        BitGrid toReturn = new BitGrid();

        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
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
    public static BitGrid getIntersectedRegions(BitGrid interFrom, BitGrid interTo)
    {
        BitGrid toReturn = new BitGrid();
        toReturn.set(interTo);

        for(int i = 0; i< BitGrid.gridWidth; i++)
        {
            for(int j = 0; j< BitGrid.gridHeight; j++)
            {
                if(interFrom.getPoint(i,j) != 0)
                {
                    fill(toReturn, i, j);
                }
            }
        }


        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
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
    static void fill(BitGrid toFill, int iInput, int jInput)
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

            if (i-1 >= 0)
            {
                indexes.Push(new Vector2(i-1, j));
            }
            if (i + 1 < BitGrid.gridWidth)
            {
                indexes.Push(new Vector2(i + 1, j));
            }
            if (j - 1 >= 0)
            {
                indexes.Push(new Vector2(i, j - 1));
            }
            if (j + 1 < BitGrid.gridHeight)
            {
                indexes.Push(new Vector2(i, j + 1));
            }

            /*for (int iInd = i - 1; iInd <= i + 1; iInd++)
            {
                for (int jInd = j - 1; jInd <= j + 1; jInd++)
                {
                    if (iInd < 0 || iInd >= BitGrid.gridWidth || jInd < 0 || jInd >= BitGrid.gridHeight)
                    {
                        continue;
                    }
                    indexes.Push(new Vector2(iInd, jInd));
                }
            }*/
        }
    }

    //returns a bitmap with a line of 1s around every spot that had 1s in map
    public static BitGrid getBorderRegion(BitGrid map)
    {
        BitGrid toReturn = BitGrid.zeros();
        for(int i = 0; i< BitGrid.gridWidth; i++)
        {
            for(int j = 0; j<BitGrid.gridHeight; j++)
            {
                if (map.getPoint(i, j) == 0)
                {
                    bool breakLoop = false;
                    for (int iInd = i - 1; iInd <= i + 1; iInd++)
                    {
                        for (int jInd = j - 1; jInd <= j + 1; jInd++)
                        {
                            if (iInd < 0 || iInd >= BitGrid.gridWidth || jInd < 0 || jInd >= BitGrid.gridHeight)
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
    public static BitGrid invert(BitGrid input)
    {
        BitGrid toReturn = new BitGrid();
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for(int j = 0; j<BitGrid.gridHeight; j++)
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

    //returns a new BitGrid that is all ones
    public static BitGrid ones()
    {

        BitGrid toReturn = new BitGrid();
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
            {
                toReturn.setPoint(i, j, 1);
            }
        }
        return toReturn;
    }

    //returns a new BitGrid that is all zeros
    public static BitGrid zeros()
    {

        BitGrid toReturn = new BitGrid();
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
            {
                toReturn.setPoint(i, j, 0);
            }
        }
        return toReturn;
    }


    //returns a new BitGrid that is all zeros with a diagonal line of ones
    public static BitGrid line()
    {
        BitGrid toReturn = new BitGrid();
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
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

    //returns a new BitGrid that is all zeros with a circle of ones
    public static BitGrid circle()
    {
        BitGrid toReturn = new BitGrid();
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
            {
                if (Mathf.Sqrt(Mathf.Pow(i - BitGrid.gridWidth * 0.5f, 2f) + Mathf.Pow(j - BitGrid.gridHeight * 0.5f, 2f)) < BitGrid.gridWidth * 0.25f)
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

    //returns if the BitGrid is all zeros
    public bool isEmpty()
    {
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
            {
                if (grid[i, j] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public String printGrid()
    {
        string output = "";
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            output += "|";
            for (int j = 0; j < BitGrid.gridHeight; j++)
            {
                if (grid[i,j] != 0)
                {
                    output += "█";
                }
                else
                {
                    output += "⠀";
                }
            }
            output += "|\n";
        }
        return output;
    }
}
