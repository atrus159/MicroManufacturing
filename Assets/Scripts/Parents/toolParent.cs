using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolParent 
{
    public paint paintCanvas;
    public bool callOffCanvasFlag;
    public toolParent()
    {
        paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();
        callOffCanvasFlag = false;
    }

    virtual public void onMouseDown(int i, int j)
    {

    }
    
    virtual public void onClick(int i, int j)
    {

    }

    virtual 
        public void onRelease(int i, int j)
    {

    }
}
