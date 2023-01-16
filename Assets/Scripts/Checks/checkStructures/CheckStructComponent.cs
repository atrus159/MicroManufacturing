using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStructComponent
{
    public List<CheckStructComponent> adjacents;
    public control.materialType materialType;
    public Vector2Int facing;
    public bool isAnchor;
    public LayerStackHolder layers;

    public struct satisfyResult {
        public int layer;
        public int direction;
        public bool satisfied;
    }

    public CheckStructComponent(control.materialType materialType)
    {
        adjacents = new List<CheckStructComponent>();
        this.materialType = materialType;
        this.facing = Vector2Int.down;
        this.isAnchor = false;
        this.layers = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
    }


    virtual public satisfyResult satisfy(satisfyResult starting)
    {
        return new satisfyResult { layer = 0, direction = 1, satisfied = true };
    }
}
