using CGTespy.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class paint : MonoBehaviour
{
    public BitGrid grid;
    public BitGrid gridOld;
    public Image image;
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

    float alphaValue = 0.5f;

    RectTransform trans;

    bool successfulClick;

    Resolution res;
    // Start is called before the first frame update

    List<BitGrid> prevStates;
    List<BitGrid> nextStates;
    BitGrid curState;

    public int prevCount = 0;
    public int nextCount = 0;

    GameObject redoButton;
    GameObject undoButton;

    void Start()
    {
        redoButton = GameObject.Find("RedoButton");
        undoButton = GameObject.Find("UndoButton");
        grid = new BitGrid();
        gridOld = new BitGrid();
        image = GetComponent<Image>();
        trans = GetComponent<RectTransform>();
        clickCoords = new Vector2Int(-1, -1);

        /* Fills canvas with white background */
        texture = new Texture2D((int) trans.sizeDelta.x, (int)trans.sizeDelta.y, TextureFormat.RGBA32, false);
        oldTexture = new Texture2D((int)trans.sizeDelta.x, (int)trans.sizeDelta.y, TextureFormat.RGBA32, false);
        updateScale();
        scaleFactor = 3;
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for(int j = 0; j< BitGrid.gridHeight; j++)
            {
                this.setPixel(i, j, 0);
            }
        }
        texture.Apply();
        image.material.mainTexture = texture;

        curColor = 1;
        tools = new toolParent[5] { new brushTool(), new bucketTool(), new lineTool(), new rectangleTool(), new elipseTool()};
        curTool = 0;
        fillMode = 0;
        successfulClick = false;
        res = Screen.currentResolution;

        prevStates = new List<BitGrid>();
        nextStates = new List<BitGrid>();
        curState = BitGrid.zeros();
    }

    public void setPixel(int i, int j, int val)
    {
        if (i == 0 && j == 0 && val == 1)
            Debug.Log("0,0");

        if (i == 99 && j == 0 && val == 1)
            Debug.Log("100,0");
        grid.setPoint(i,j,val);
        Color toSet = (val == 0) ? Color.white : Color.black;
        toSet.a = alphaValue;
        for (int indI = 0; indI < scaleFactor; indI++)
        {
            for(int indJ = 0; indJ < scaleFactor; indJ++)
            {
                texture.SetPixel(xOffset + scaleFactor * i + indI, yOffset + scaleFactor * j + indJ, toSet);
            }
        }
    }



    public void addState()
    {
        BitGrid toAdd = new BitGrid();
        toAdd.set(curState);
        prevStates.Add(toAdd);
        curState.set(grid);
        nextStates.Clear();
        prevCount = prevStates.Count; nextCount = nextStates.Count;
    }

    public void undoState()
    {
        if(prevStates.Count == 0)
        {
            return;
        }
        BitGrid toMove = prevStates.LastOrDefault<BitGrid>();
        prevStates.Remove(toMove);
        BitGrid ToAdd = new BitGrid();
        ToAdd.set(curState);
        nextStates.Add(ToAdd);
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
            {
                setPixel(i, j, toMove.getPoint(i,j));
            }
        }
        curState.set(grid);
        texture.Apply();
        prevCount = prevStates.Count; nextCount = nextStates.Count;
    }

    public void redoState()
    {
        if(nextStates.Count == 0)
        {
            return;
        }
        BitGrid toAdd = new BitGrid();
        toAdd.set(grid);
        prevStates.Add(toAdd);
        BitGrid toSet = nextStates.LastOrDefault<BitGrid>();
        nextStates.Remove(toSet);
        for (int i = 0; i < BitGrid.gridWidth; i++)
        {
            for (int j = 0; j < BitGrid.gridHeight; j++)
            {
                setPixel(i, j, toSet.getPoint(i, j));
            }
        }
        curState.set(grid);
        texture.Apply();
        prevCount = prevStates.Count; nextCount = nextStates.Count;
    }

    Vector3 getMousePos()
    {
        float mx = Input.mousePosition.x;
        float my = Input.mousePosition.y;
        float centerX = Screen.width * 0.5f;
        float centerY = Screen.height * 0.5f;
        Vector3 toReturn = new Vector3(-2, -2, -2);
        float width = scaleFactor * BitGrid.gridWidth;
        float height = scaleFactor * BitGrid.gridHeight;


        float onCanvasX = mx - (centerX - width * 0.5f);
        float onCanvasY = my - (centerY - height * 0.5f);




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

        //Debug.Log(toReturn.x + ", " + toReturn.y);
        return toReturn;
    }

    void updateScale()
    {
        /*float width = trans.lossyScale.x*300;
        float height = trans.lossyScale.y*300;
        //Debug.Log(width + ", " + height);
        float minDist = width;
        if(height < width)
        {
            minDist = height;
        }
        scaleFactor = (int) Mathf.Floor((float) minDist / BitGrid.gridWidth);*/
        trans.sizeDelta = new Vector2(scaleFactor*BitGrid.gridWidth / trans.lossyScale.x, scaleFactor * BitGrid.gridHeight / trans.lossyScale.y);
        GameObject.Find("camera_panel").GetComponent<RectTransform>().sizeDelta= trans.sizeDelta;
        //Debug.Log(scaleFactor);

        redoButton.transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(trans.sizeDelta.x*0.45f, trans.sizeDelta.y*0.55f), Quaternion.identity);
        undoButton.transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(trans.sizeDelta.x*0.25f, trans.sizeDelta.y*0.55f), Quaternion.identity);
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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                undoState();
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                redoState();
                }
            }
        }

        
        if (res.Equals(Screen.currentResolution))
        {

            updateScale();

            res = Screen.currentResolution;

        }
    }
}
