using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class checkDonePressed : levelRequirementParent
{
    public GameObject donePrefab;
    bool donePressed;
    GameObject doneButton;
    bool firstCheck;
    public override void onStart()
    {
        base.onStart();
        name = "Take a look around the wafer, then click the “done” button.";
        description = "You can move the camera with the arrow keys.";
        firstCheck = true;
        checkOutsideEdits = true;
    }


    public void onPress()
    {
        donePressed = true;
        GameObject.Destroy(doneButton);
    }
    public override void check()
    {
        if (firstCheck)
        {
            doneButton = Instantiate(donePrefab);
            doneButton.transform.SetParent(GameObject.Find("Canvas - Main").transform, false);
            doneButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { onPress(); });
            donePressed = false;
            firstCheck = false;
        }
        met = donePressed;
    }
}
