using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CheckStruct
{
    public CheckStructComponent head;
    public int startLayer;

    public CheckStruct(int startLayer, int direction)
    {
        head = new CheckStructComponent(control.materialType.empty, direction);
        this.startLayer= startLayer;
        head.setEndState(true);
    }

    public void append(CheckStructComponent toAppend, CheckStructComponent appendTo, bool holdEndState = false)
    {
        appendTo.adjacents.Add(toAppend);
        toAppend.setEndState(true);
        if (!holdEndState)
        {
            appendTo.setEndState(false);
        }
    }

    public void appendAllEnds(CheckStructComponent toAppend, bool holdEndState = false)
    {
        appendAllRecurse(toAppend, head, holdEndState);
    }

    void appendAllRecurse(CheckStructComponent toAppend, CheckStructComponent appendTo, bool holdEndState)
    {
        if (appendTo.isEnd())
        {
            append(toAppend.clone(), appendTo, holdEndState);
            toAppend.setEndState(true);
            return;
        }
        foreach(CheckStructComponent curComponent in appendTo.adjacents)
        {
            appendAllRecurse(toAppend, curComponent, holdEndState);
        }
    }

    public bool satisfy()
    {
        CheckStructComponent.satisfyResult startingResult = new CheckStructComponent.satisfyResult();
        startingResult.startingLayers = new List<int>();
        startingResult.startingLayers.Add(startLayer);
        startingResult.direction = head.direction;
        startingResult.satisfied = true;
        return satisfyRecurse(head, startingResult, 0);
    }

    bool satisfyRecurse(CheckStructComponent startingComponent, CheckStructComponent.satisfyResult startingResult, int startingIndex)
    {
        CheckStructComponent.satisfyResult curResult = startingComponent.satisfy(startingResult, startingIndex);
        if (!curResult.satisfied)
        {
            return false;
        }

        if (startingComponent.isEnd())
        {
            return true;
        }

        foreach (CheckStructComponent curComponent in startingComponent.adjacents)
        {
            bool any = false;
            for (int i = 0; i < curResult.startingLayers.Count; i++)
            {
                if (satisfyRecurse(curComponent,curResult, i))
                {
                    any = true;
                }
            }
            if (!any)
            {
                return false;
            }
        }
        return true;
    }

}
