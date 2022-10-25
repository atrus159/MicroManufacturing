using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
}
