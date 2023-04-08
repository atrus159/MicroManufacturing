using CGTespy.UI;
using System.Threading;
using TMPro;
using UnityEngine;

public class textDeactivateSLiderButtons : BasicText
{

    GameObject finishButton;
    GameObject cancelButton;
    public GameObject TargetText;
    override public void Initialize()
    {
        base.Initialize();
        Invoke("delayDeactivateButtons", 0);
    }
    override public void Display()
    {
        base.Display();

    }

    void delayDeactivateButtons()
    {
        finishButton = GameObject.Find("Finished Button(Clone)");
        cancelButton = GameObject.Find("Cancel Button(Clone)");
        TargetText.GetComponent<BasicText>().activates.Add(finishButton);
        TargetText.GetComponent<BasicText>().activates.Add(cancelButton);
        finishButton.SetActive(false);
        cancelButton.SetActive(false);
    }
}