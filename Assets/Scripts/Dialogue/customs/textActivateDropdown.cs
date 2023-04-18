using CGTespy.UI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textActivateDropdown : BasicText
{

 
    override public void Initialize()
    {
        base.Initialize();
        GameObject.Find("Dropdown").GetComponent<DropdownCustom>().visible = true;
        GameObject.Find("Dropdown").GetComponent<Image>().enabled = true;
    }
    override public void Display()
    {
        base.Display();
        
    }

}