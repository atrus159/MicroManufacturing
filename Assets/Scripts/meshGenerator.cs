using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class meshGenerator : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    public GameObject meshGameObject;
    public float layerHeight = 0.1f;
    public float cellSize = 0.1f;
    public bool toBeDestroyed = false;

    public BitGrid grid;

    enum faces
    {
        North,
        South,
        East,
        West,
        Top,
        Bottom
    }

    // Start is called before the first frame update
    void Awake()
    {
        grid = new BitGrid();
    }

    public void initialize(bool skipTopBottom = false)
    {
        createGrid(skipTopBottom);
        updateMesh();
    }

    void createGrid(bool skipTopBottom = false)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        while (true)
        {
            int startI = 0;
            int startJ = 0;
            int i = 0;
            int j = 0;
            for(i = 0; i < BitGrid.gridWidth; i++)
            {
                int breakflag = 0;
                for(j = 0; j < BitGrid.gridHeight; j++)
                {
                    if(grid.getPoint(i,j) == 1)
                    {
                        breakflag = 1;
                        break;
                    }
                }
                if (breakflag == 1)
                {
                    break;
                }
            }
            startI = i;
            startJ = j;


            if(startI >= BitGrid.gridWidth-1 && startJ >= BitGrid.gridHeight -1)
            {
                break;
            }

            for(j = startJ; j< BitGrid.gridHeight; j++)
             {
                 if(grid.getPoint(i,j) != 1)
                 {
                     break;
                 }

             }
             int endJ = j - 1;


             for (i = startI; i < BitGrid.gridWidth; i++)
             {
                 int breakFlag = 0;
                 for(j = startJ; j<= endJ; j++)
                 {
                     if (grid.getPoint(i,j) != 1)
                     {
                        breakFlag = 1;
                         break;
                     }
                 }
                 if (breakFlag == 1)
                 {
                     break;
                 }

            } 
            int endI = i - 1;


                
             for(i  = startI; i <= endI; i++)
            {
                for(j = startJ; j<= endJ; j++)
                {
                    grid.setPoint(i,j,2);
                }
            }
            if (!skipTopBottom)
            {
                createFace(startI, startJ, endI, endJ, faces.Top);
                createFace(startI, startJ, endI, endJ, faces.Bottom);
            }
             createSideFace(startI, startJ, endI, endJ, faces.North);
             createSideFace(startI, startJ, endI, endJ, faces.South);
             createSideFace(startI, startJ, endI, endJ, faces.East);
             createSideFace(startI, startJ, endI, endJ, faces.West);
        }

    }

    void createSideFace(int startI, int startJ, int endI, int endJ, faces face)
    {
        bool makingFace = false;
        int curFaceStart = 0;
        switch (face)
        {
            case faces.North:
                if (endJ + 1 >= BitGrid.gridHeight)
                {
                    createFace(startI, startJ, endI, endJ, faces.North);
                }
                else
                {
                    for (int i = startI; i <= endI; i++)
                    {
                        if (!makingFace)
                        {
                            if (grid.getPoint(i, endJ + 1) == 0)
                            {
                                curFaceStart = i;
                                makingFace = true;
                            }
                        }
                        if (makingFace)
                        {
                            if (grid.getPoint(i, endJ + 1) != 0 || i == endI)
                            {
                                createFace(curFaceStart, startJ, i, endJ, faces.North);
                                makingFace = false;
                            }
                        }
                    }
                }
                
                break;

            case faces.South:
                if (startJ - 1 <0)
                {
                    createFace(startI, startJ, endI, endJ, faces.South);
                }
                else
                {
                    for (int i = startI; i <= endI; i++)
                    {
                        if (!makingFace)
                        {
                            if (grid.getPoint(i, startJ - 1) == 0)
                            {
                                curFaceStart = i;
                                makingFace = true;
                            }
                        }
                        if (makingFace)
                        {
                            if (grid.getPoint(i, startJ - 1) != 0 || i == endI)
                            {
                                createFace(curFaceStart, startJ, i, endJ, faces.South);
                                makingFace = false;
                            }
                        }
                    }
                }

                break;

            case faces.East:
                if (endI + 1 >= BitGrid.gridWidth)
                {
                    createFace(startI, startJ, endI, endJ, faces.East);
                }
                else
                {
                    for (int j = startJ; j <= endJ; j++)
                    {
                        if (!makingFace)
                        {
                            if (grid.getPoint(endI + 1, j) == 0)
                            {
                                curFaceStart = j;
                                makingFace = true;
                            }
                        }
                        if (makingFace)
                        {
                            if (grid.getPoint(endI + 1, j) != 0 || j == endJ)
                            {
                                createFace(startI, curFaceStart, endI, j, faces.East);
                                makingFace = false;
                            }
                        }
                    }
                }

                break;

            case faces.West:
                if (startI - 1 < 0 )
                {
                    createFace(startI, startJ, endI, endJ, faces.West);
                }
                else
                {
                    for (int j = startJ; j <= endJ; j++)
                    {
                        if (!makingFace)
                        {
                            if (grid.getPoint(startI - 1, j) == 0)
                            {
                                curFaceStart = j;
                                makingFace = true;
                            }
                        }
                        if (makingFace)
                        {
                            if (grid.getPoint(startI - 1, j) != 0 || j == endJ)
                            {
                                createFace(startI, curFaceStart, endI, j, faces.West);
                                makingFace = false;
                            }
                        }
                    }
                }

                break;
        }
    }

    void createFace(int startI, int startJ, int endI, int endJ, faces face)
    {
        int vertStart = vertices.Count;
        float xPos = startI * cellSize;
        float zPos = startJ * cellSize;
        float xEPos = endI * cellSize;
        float zEPos = endJ * cellSize;

        switch (face)
        {
            case faces.Top:
                {
                    vertices.Add(new Vector3(xPos, 0f, zPos));
                    vertices.Add(new Vector3(xEPos + cellSize, 0f, zPos));
                    vertices.Add(new Vector3(xPos, 0f, zEPos + cellSize));
                    vertices.Add(new Vector3(xEPos + cellSize, 0f, zEPos + cellSize));
                    break;
                }
            case faces.Bottom:
                {
                    vertices.Add(new Vector3(xEPos + cellSize, -layerHeight, zEPos + cellSize));
                    vertices.Add(new Vector3(xEPos + cellSize, -layerHeight, zPos));
                    vertices.Add(new Vector3(xPos, -layerHeight, zEPos + cellSize));
                    vertices.Add(new Vector3(xPos, -layerHeight, zPos));
                    break;
                }
            case faces.North:
                {
                    vertices.Add(new Vector3(xPos, 0f, zEPos + cellSize));
                    vertices.Add(new Vector3(xEPos + cellSize, 0f, zEPos + cellSize));
                    vertices.Add(new Vector3(xPos, -layerHeight, zEPos + cellSize));
                    vertices.Add(new Vector3(xEPos + cellSize, -layerHeight, zEPos + cellSize));
                    break;
                }
            case faces.South:
                {
                    vertices.Add(new Vector3(xPos, -layerHeight, zPos));
                    vertices.Add(new Vector3(xEPos + cellSize, -layerHeight, zPos));
                    vertices.Add(new Vector3(xPos, 0f, zPos));
                    vertices.Add(new Vector3(xEPos + cellSize, 0f, zPos));
                    break;
                }
            case faces.East:
                {
                    vertices.Add(new Vector3(xEPos + cellSize, 0f, zPos));
                    vertices.Add(new Vector3(xEPos + cellSize, -layerHeight, zPos));
                    vertices.Add(new Vector3(xEPos + cellSize, 0f, zEPos + cellSize));
                    vertices.Add(new Vector3(xEPos + cellSize, -layerHeight, zEPos + cellSize));
                    break;
                }
            case faces.West:
                {
                    vertices.Add(new Vector3(xPos, -layerHeight, zPos));
                    vertices.Add(new Vector3(xPos, 0f, zPos));
                    vertices.Add(new Vector3(xPos, -layerHeight, zEPos + cellSize));
                    vertices.Add(new Vector3(xPos, 0f, zEPos + cellSize));
                    break;
                }


        }
        triangles.Add(vertStart);
        triangles.Add(vertStart + 2);
        triangles.Add(vertStart + 1);
        triangles.Add(vertStart + 2);
        triangles.Add(vertStart + 3);
        triangles.Add(vertStart + 1);
    }



    public GameObject clone()
    {
        GameObject newMesh = Instantiate(meshGameObject, transform.position, transform.rotation);
        newMesh.GetComponent<meshMaterial>().myMaterial = GetComponent<meshMaterial>().myMaterial;
        newMesh.GetComponent<meshGenerator>().grid.set(grid);
        newMesh.GetComponent<meshGenerator>().vertices = vertices.GetRange(0, vertices.Count);
        newMesh.GetComponent<meshGenerator>().triangles = triangles.GetRange(0, triangles.Count);
        newMesh.GetComponent<meshGenerator>().updateMesh();
        return newMesh;
    }

    void updateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
