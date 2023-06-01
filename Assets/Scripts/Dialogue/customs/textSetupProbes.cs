using CGTespy.UI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textSetupProbes : BasicText
{

 
    override public void Initialize()
    {
        base.Initialize();
        ProbeScript ps = GameObject.Find("probes").GetComponent<ProbeScript>(); 
        ps.updateHide(true);
        ps.realignRed(0, 1, 50);
        ps.realignBlack(99, 1, 50);
    }
    override public void Display()
    {
        base.Display();
        
    }

}