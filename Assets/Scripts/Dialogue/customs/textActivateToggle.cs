using CGTespy.UI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textActivateToggle : BasicText
{

 
    override public void Initialize()
    {
        base.Initialize();
        GameObject.Find("WetEtchToggle").GetComponent<ToggleCustom>().visible = true;
        GameObject.Find("WetEtchToggle").GetComponent<Image>().enabled = true;
    }
    override public void Display()
    {
        base.Display();
        
    }

}