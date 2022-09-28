using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Tutorial : tutorialParent
{
    GameObject cameraCheck;
    GameObject dropdownCheck;
    GameObject depoButtonCheck;

    GameObject Dropdown;
    GameObject DepoButton;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Dropdown = GameObject.Find("Dropdown");
        Dropdown.SetActive(false);
        DepoButton = GameObject.Find("Deposit Button");
        DepoButton.SetActive(false);
        GameObject.Find("Etch Button").SetActive(false);
        GameObject.Find("Photoresist Button").SetActive(false);


        cameraCheck = initializeCheck<cameraPanCheck>("camera pan check");
        dropdownCheck = initializeCheck<dropdownSelectCheck>("dropdown select check");
        depoButtonCheck = initializeCheck<depositButtonCheck>("deposit button check");

    }
    override public void initializeChecks()
    {
        tutorialChecks.Add(delegate { displayText("Hello, and welcome to Micromanufacturing!\n\nToday we'll be teaching you how to create things that are really tiny!", new Vector2(500, 300)); });
        tutorialChecks.Add(delegate { displayText("In front of you is the <b><color=#4295f5>Substrate</color></b>. It's an extremely flat sheet of glass\nthat you can build your microscopic structures on top of.\nDon't worry, we've already cleaned it for you!", new Vector2(500, 100)); });
        tutorialChecks.Add(delegate
        {
            displayTextFree("Take a look at your <b><color=#4295f5>Substrate</color></b>. You can pan\nthe camera around with the <b><color=#f2971f>WASD keys</color></b>, or the <b><color=#f2971f>arrow keys</color></b>.", new Vector2(400, 200));
            cameraCheck.SetActive(true);
        });
        tutorialChecks.Add(delegate{ checkSpacer("camera pan check"); });
        tutorialChecks.Add(delegate { displayText("Well done!", new Vector2(500, 300)); });
        tutorialChecks.Add(delegate { displayText("You'll build your structure on the substrate through a process called <b><color=#4295f5>sputtering.</color></b>\nWhen heavy ions are fired at a sheet metal, they ionize some of that metal and\nknock it off of the sheet. This creates a cloud of metal ions that can build up\non the substrate's surface.", new Vector2(500, 300)); });
        tutorialChecks.Add(delegate
        {
            displayText("We have 3 metals for you to choose from, <b><color=#00EBFF>chromium</color></b>, <b><color=#717171>aluminum</color></b> and <b><color=#AB8100>Gold</color></b>.\nYou can select a metal from the <b><color=#f2971f>dropdown menu</color></b> in the top left.", new Vector2(500, 100));
            Dropdown.SetActive(true);
        });
        tutorialChecks.Add(delegate { 
            displayTextFree("Click on the <b><color=#f2971f>dropdown</color></b> and select <b><color=#717171>aluminum</color></b>.", new Vector2(400, 200));
            dropdownCheck.SetActive(true);
        });
        tutorialChecks.Add(delegate { checkSpacer("dropdown select check"); });
        tutorialChecks.Add(delegate
        {
            displayTextFree("Click the <b><color=#f2971f>deposit button</color></b> to start sputtering <b><color=#717171>aluminum</color></b>.", new Vector2(600, 150));
            depoButtonCheck.SetActive(true);
            DepoButton.SetActive(true);
        });
        tutorialChecks.Add(delegate { checkSpacer("deposit button check"); });
        tutorialChecks.Add(delegate { displayText("Welcome to the deposition chamber. Your machine is about to sputter <b><color=#717171>aluminum</color></b> on\nthe substrate and create a thin layer. The longer you run the process for,\nthe thicker the layer you create will be.", new Vector2(500, 400)); });
        tutorialChecks.Add(delegate { displayText("The <b><color=#f2971f>slider</color></b> in the bottom right controls how long you will run the sputtering process for.\nYou can see a preview of how much it <b><color=#717171>aluminum</color></b> it will deposit in green.", new Vector2(500, 400)); });
    }

}
