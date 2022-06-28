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
    public static int gridWidth = control.gridWidth;
    public static int gridHeight = control.gridHeight;
    public float layerHeight = 0.1f;
    public float cellSize = 0.1f;
    public bool toBeDestroyed = false;

    public int[,] grid =  new int[gridWidth, gridHeight];

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
    void Start()
    {
    }

    public void initialize()
    {
        createGrid();
        updateMesh();
    }

    void createGrid()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        while (true)
        {
            int startI = 0;
            int startJ = 0;
            int i = 0;
            int j = 0;
            for(i = 0; i < gridWidth; i++)
            {
                int breakflag = 0;
                for(j = 0; j < gridHeight; j++)
                {
                    if(grid[i,j] == 1)
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

            //int southFlag = 0;
            //int northFlag = 0;
            //int eastFlag = 0;
            //int westFlag = 0;
            /*if (startI == 0)
            {
                westFlag = 1;
            }
            if(startJ == 0)
            {
                southFlag = 1;
            }*/

            if(startI >= gridWidth-1 && startJ >= gridHeight-1)
            {
                break;
            }

            for(j = startJ; j< gridHeight; j++)
             {
                 if(grid[startI,j] != 1)
                 {
                     break;
                 }

             }
             int endJ = j - 1;


             for (i = startI; i < gridWidth; i++)
             {
                 int breakFlag = 0;
                 for(j = startJ; j<= endJ; j++)
                 {
                     if (grid[i, j] != 1)
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
            /*if(endI == gridWidth - 1)
            {
                eastFlag = 1;
            }
            if(endJ == gridHeight - 1)
            {
                northFlag = 1;
            }*/

                
             for(i  = startI; i <= endI; i++)
            {
                for(j = startJ; j<= endJ; j++)
                {
                    grid[i, j] = 2;
                }
            }
             createFace(startI, startJ, endI, endJ, faces.Top);
             createFace(startI, startJ, endI, endJ, faces.Bottom);
             createFace(startI, startJ, endI, endJ, faces.North);
             createFace(startI, startJ, endI, endJ, faces.South);
             createFace(startI, startJ, endI, endJ, faces.East);
             createFace(startI, startJ, endI, endJ, faces.West);
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




    /*void createGrid()
    {

        vertices = new List<Vector3>();
        triangles = new List<int>();


        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if(grid[i,j] == 1)
                {
                    float xPos = i * cellSize;
                    float yPos = j * cellSize;
                    createFace(xPos, yPos, faces.Top);
                    //createFace(xPos, yPos, faces.Bottom);
                    if(i == 0 || grid[i-1, j] == 0)
                    {
                        createFace(xPos, yPos, faces.West);
                    }
                    if (i == gridWidth - 1 || grid[i + 1, j] == 0)
                    {
                        createFace(xPos, yPos, faces.East);
                    }
                    if (j == 0 || grid[i, j - 1] == 0)
                    {
                        createFace(xPos, yPos, faces.South);
                    }
                    if (j == gridHeight - 1 || grid[i, j + 1] == 0)
                    {
                        createFace(xPos, yPos, faces.North);
                    }

                }
            }
        }

    }

    void createFace(float xPos, float zPos, faces face)
    {
        int vertStart = vertices.Count;
        switch (face)
        {
            case faces.Top:
            {
                vertices.Add(new Vector3(xPos, 0f, zPos));
                vertices.Add(new Vector3(xPos + cellSize, 0f, zPos));
                vertices.Add(new Vector3(xPos, 0f, zPos + cellSize));
                vertices.Add(new Vector3(xPos + cellSize, 0f, zPos + cellSize));
                break;
            }
            case faces.Bottom:
            {
                vertices.Add(new Vector3(xPos + cellSize, -layerHeight*cellSize, zPos + cellSize));
                vertices.Add(new Vector3(xPos + cellSize, -layerHeight * cellSize, zPos));
                vertices.Add(new Vector3(xPos, -layerHeight * cellSize, zPos + cellSize));
                vertices.Add(new Vector3(xPos, -layerHeight * cellSize, zPos));
                break;
            }
            case faces.North:
            {
                vertices.Add(new Vector3(xPos, 0f, zPos + cellSize));
                vertices.Add(new Vector3(xPos + cellSize, 0f, zPos + cellSize));
                vertices.Add(new Vector3(xPos, -layerHeight * cellSize, zPos + cellSize));
                vertices.Add(new Vector3(xPos + cellSize, -layerHeight * cellSize, zPos + cellSize));
                break;
            }
            case faces.South:
            {
                vertices.Add(new Vector3(xPos, -layerHeight * cellSize, zPos));
                vertices.Add(new Vector3(xPos + cellSize, -layerHeight * cellSize, zPos));
                vertices.Add(new Vector3(xPos, 0f, zPos));
                vertices.Add(new Vector3(xPos + cellSize, 0f, zPos));
                break;
            }
            case faces.East:
            {
                vertices.Add(new Vector3(xPos + cellSize, 0f, zPos));
                vertices.Add(new Vector3(xPos + cellSize, -layerHeight * cellSize, zPos));
                vertices.Add(new Vector3(xPos + cellSize, 0f, zPos + cellSize));
                vertices.Add(new Vector3(xPos + cellSize, -layerHeight * cellSize, zPos + cellSize));
                break;
            }
            case faces.West:
            {
                vertices.Add(new Vector3(xPos, -layerHeight * cellSize, zPos));
                vertices.Add(new Vector3(xPos, 0f, zPos));
                vertices.Add(new Vector3(xPos, -layerHeight * cellSize, zPos + cellSize));
                vertices.Add(new Vector3(xPos, 0f, zPos + cellSize));
                break;
            }


        }
        triangles.Add(vertStart);
        triangles.Add(vertStart + 2);
        triangles.Add(vertStart + 1);
        triangles.Add(vertStart + 2);
        triangles.Add(vertStart + 3);
        triangles.Add(vertStart + 1);
    }*/

    public GameObject clone()
    {
        GameObject newMesh = Instantiate(meshGameObject, transform.position, transform.rotation);
        newMesh.GetComponent<meshMaterial>().myMaterial = GetComponent<meshMaterial>().myMaterial;
        System.Array.Copy(grid, newMesh.GetComponent<meshGenerator>().grid, control.gridWidth * control.gridHeight);
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
