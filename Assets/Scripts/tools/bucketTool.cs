using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bucketTool : toolParent
{
    public bucketTool() : base()
    {

    }

    public override void onMouseDown(int i, int j)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        int color = paintCanvas.curColor;
        fill(i,j, color, queue);

        while (queue.Count > 0)
        {
            Vector2Int toCheck = queue.Dequeue();

            fill(toCheck.x - 1, toCheck.y, color, queue);
            fill(toCheck.x, toCheck.y - 1, color, queue);
            fill(toCheck.x + 1, toCheck.y, color, queue);
            fill(toCheck.x, toCheck.y + 1, color, queue);
        }
        paintCanvas.texture.Apply();
    }

    private void fill(int i, int j, int color, Queue<Vector2Int> queue)
    {
        if(i < 0 || i >= control.gridWidth || j < 0 || j >= control.gridHeight)
        {
            return;
        }
        if (paintCanvas.grid[i,j] != color)
        {
            paintCanvas.setPixel(i, j, color);
            queue.Enqueue(new Vector2Int(i,j));
        }
    }

}
