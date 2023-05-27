using CGTespy.UI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textResetLayerStack : BasicText
{

 
    override public void Initialize()
    {
        base.Initialize();
        GameObject process = GameObject.Find("New Process");
        LayerStackHolder ls = GameObject.Find("LayerStack").GetComponent<LayerStackHolder>();
        if (process)
        {
            process.GetComponent<ProcessParent>().onCancelButton();
        }
        ls.clear();
    }
    override public void Display()
    {
        base.Display();
        
    }

}