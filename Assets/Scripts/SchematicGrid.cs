using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * Similar to SchematicGrid, but allows multiple colors.
 */
public class SchematicGrid
{

    public static int gridWidth = 100;
    public static int gridHeight = 100;

    int[,] grid;

    public SchematicGrid()
    {

        grid = new int[SchematicGrid.gridWidth, SchematicGrid.gridHeight];

    }

    public void setPoint(int i, int j, int val)
    {
        grid[i, j] = val;
    }

    public int getPoint(int i, int j)
    {
        return grid[i, j];
    }

    //sets the grid to that of another SchematicGrid
    public void set(SchematicGrid toSet)
    {
        for (int i = 0; i < SchematicGrid.gridWidth; i++)
        {
            for (int j = 0; j < SchematicGrid.gridHeight; j++)
            {
                grid[i, j] = toSet.getPoint(i, j);
            }
        }
    }

    //returns a new SchematicGrid that is all zeros
    public static SchematicGrid zeros()
    {

        SchematicGrid toReturn = new SchematicGrid();
        for (int i = 0; i < SchematicGrid.gridWidth; i++)
        {
            for (int j = 0; j < SchematicGrid.gridHeight; j++)
            {
                toReturn.setPoint(i, j, 0);
            }
        }
        return toReturn;
    }

    public Texture2D gridToTexture(int textureWidth, int textureHeight)
    {
        Texture2D newTexture = new Texture2D(textureWidth, textureHeight);
        // new Color(201, 110, 55, 1)
        Color silicon = new Color(0.7f, 0.7f, 0.7f);
        Color silicondioxide = new Color(0.8f, 0.55f, 0.4f);

        Color[] colorArray = new Color[] { Color.white, Color.cyan, Color.yellow, Color.gray, silicon, silicondioxide };

        for (int i = 0; i < textureWidth; i++)
            for (int j = 0; j < textureHeight; j++)
            {
                int pointVal = this.getPoint(i / (textureWidth / 100), j / (textureHeight / 100));

                Color toSet = Color.white;

                if (pointVal >= 0 && pointVal < colorArray.Length)
                    toSet = colorArray[pointVal];

                newTexture.SetPixel(i, j, toSet);
            }

        return newTexture;
    }
}

