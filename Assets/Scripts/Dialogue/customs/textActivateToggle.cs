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
        GameObject.Find("Canvas - Main").transform.Find("WetEtchToggle").gameObject.GetComponent<Activatable>().activate();
    }
    override public void Display()
    {
        base.Display();
        
    }

}