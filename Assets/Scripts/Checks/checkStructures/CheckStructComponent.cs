using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStructComponent
{
    public List<CheckStructComponent> adjacents;
    public control.materialType materialType;
    public LayerStackHolder layers;
    public int direction;
    bool endState;



    public struct satisfyResult {
        public List<int> startingLayers;
        public int direction;
        public bool satisfied;
    }

    public CheckStructComponent(control.materialType materialType, int direction)
    {
        adjacents = new List<CheckStructComponent>();
        this.materialType = materialType;
        this.layers = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
        this.direction = direction;
        endState = false;
    }


    public bool isEnd()
    {
        return endState;
    }

    public void setEndState(bool newEndState)
    {
        endState = newEndState; 
    }

    virtual public satisfyResult satisfy(satisfyResult starting, int layerIndex = 0)
    {
        return starting;
    }

    virtual public CheckStructComponent clone()
    {
        return new CheckStructComponent(materialType, direction);
    }
}
