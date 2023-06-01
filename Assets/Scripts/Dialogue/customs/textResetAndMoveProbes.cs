using CGTespy.UI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textResetAndMoveProbes : BasicText
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
        ProbeScript oldRed = GameObject.Find("probes").GetComponent<ProbeScript>();
        ProbeScript red = GameObject.Find("probesRed").GetComponent<ProbeScript>();
        ProbeScript blue = GameObject.Find("probesBlue").GetComponent<ProbeScript>();
        ProbeScript green = GameObject.Find("probesGreen").GetComponent<ProbeScript>();
        red.updateHide(true);
        blue.updateHide(true);
        green.updateHide(true);
        red.realignRed(0, 1, 25);
        red.realignBlack(99, 1, 75);
        blue.realignRed(0, 1, 50);
        blue.realignBlack(99, 1, 25);
        green.realignRed(0, 1, 75);
        green.realignBlack(99, 1, 50);
        oldRed.updateHide(false);
    }
    override public void Display()
    {
        base.Display();
        
    }

}