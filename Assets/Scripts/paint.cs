using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class paint : MonoBehaviour
{
    public BitGrid grid;
    public BitGrid gridOld;
    Image image;
    public Texture2D texture;
    public Texture2D oldTexture;
    public int scaleFactor;
    public int xOffset;
    public int yOffset;
    public int curColor;
    public toolParent[] tools;
    public int curTool;
    public Vector2Int clickCoords;
    public int fillMode;

    RectTransform trans;

    bool successfulClick;


    // Start is called before the first frame update
    void Start()
    {
        grid = new BitGrid();
        gridOld = new BitGrid();

        image = GetComponent<Image>();
        trans = GetComponent<RectTransform>();
        texture = new Texture2D((int) trans.sizeDelta.x, (int)trans.sizeDelta.y);
        oldTexture = new Texture2D((int)trans.sizeDelta.x, (int)trans.sizeDelta.y);
        clickCoords = new Vector2Int(-1, -1);
        image.material.mainTexture = texture;
        updateScale();
        for(int i = 0; i < BitGrid.gridWidth; i++)
        {
            for(int j = 0; j< BitGrid.gridHeight; j++)
            {
                setPixel(i, j, 0);
            }
        }
        texture.Apply();
        curColor = 1;
        tools = new toolParent[5];
        tools[0] = new brushTool();
        tools[1] = new bucketTool();
        tools[2] = new lineTool();
        tools[3] = new rectangleTool();
        tools[4] = new elipseTool();
        curTool = 0;
        fillMode = 0;
        successfulClick = false;
    }


    public void setPixel(int i, int j, int val)
    {
        grid.setPoint(i,j,val);
        Color toSet = Color.black;
        if(val == 0)
        {
            toSet = Color.white;
        }
        for(int indI = 0; indI < scaleFactor; indI++)
        {
            for(int indJ = 0; indJ < scaleFactor; indJ++)
            {
                texture.SetPixel(xOffset + scaleFactor * i + indI, yOffset + scaleFactor * j + indJ, toSet);
            }
        }
    }

    Vector3 getMousePos()
    {
        float mx = Input.mousePosition.x;
        float my = Input.mousePosition.y;
        Debug.Log(mx + " " + my);
        Debug.Log(scaleFactor);
        float width = trans.sizeDelta.x;
        float height = trans.sizeDelta.y;
        float centerX = Screen.width * 0.5f;
        float centerY = Screen.height * 0.5f;
        Vector3 toReturn = new Vector3(-2, -2, -2);


        float onCanvasX = mx - (centerX - width * 0.5f + xOffset) ;
        float onCanvasY = my - (centerY - height * 0.5f + yOffset);

        toReturn.x = Mathf.Round(onCanvasX / scaleFactor);
        toReturn.y = Mathf.Round(onCanvasY / scaleFactor);
        if(toReturn.x >= BitGrid.gridWidth)
        {
            toReturn.x = BitGrid.gridWidth-1;
            toReturn.z = -1;
        }
        if(toReturn.y >= BitGrid.gridHeight)
        {
            toReturn.y = BitGrid.gridHeight-1;
            toReturn.z = -1;
        }
        if(toReturn.x < 0)
        {
            toReturn.x = 0;
            toReturn.z = -1;
        }
        if(toReturn.y < 0)
        {
            toReturn.y = 0;
            toReturn.z = -1;
        }
        return toReturn;
    }

    void updateScale()
    {
        int width = (int)trans.sizeDelta.x;
        int height = (int)trans.sizeDelta.y;
        int minDist = width;
        if(height < width)
        {
            minDist = height;
        }
        scaleFactor = (int) Mathf.Floor((float) minDist / BitGrid.gridWidth);
        xOffset = (int)((width - (100 * scaleFactor)) * 0.5f);
        yOffset = (int)((height - (100 * scaleFactor)) * 0.5f);
    }

    public void saveGrid()
    {
        Graphics.CopyTexture(texture, oldTexture);
        gridOld.set(grid);
    }

    public void loadGrid()
    {
        Graphics.CopyTexture(oldTexture, texture);
        grid.set(gridOld);
    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find("Control").GetComponent<control>().hudVisible)
        {
            Vector3 mouseVec = getMousePos();
            if (mouseVec.z != -1)
            {
                if (Input.GetMouseButton(0) && !tools[curTool].callOffCanvasFlag)
                {
                    tools[curTool].onClick((int)mouseVec.x, (int)mouseVec.y);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    tools[curTool].onMouseDown((int)mouseVec.x, (int)mouseVec.y);
                    successfulClick = true;
                }
            }
            if (Input.GetMouseButtonUp(0) && successfulClick)
            {
                tools[curTool].onRelease((int)mouseVec.x, (int)mouseVec.y);
            }
            if (Input.GetMouseButton(0) && tools[curTool].callOffCanvasFlag)
            {
                tools[curTool].onClick((int)mouseVec.x, (int)mouseVec.y);
            }

        }
    }
}
