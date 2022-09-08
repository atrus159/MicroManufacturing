using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Tutorial : tutorialParent
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GameObject.Find("Dropdown").SetActive(false);
        GameObject.Find("Deposit Button").SetActive(false);
        GameObject.Find("Etch Button").SetActive(false);
        GameObject.Find("Photoresist Button").SetActive(false);
    }
    override public void initializeChecks()
    {
        tutorialChecks.Add(delegate { displayText("Hello, and welcome to Micromanufacturing!\n\nToday we'll be teaching you how to create things that are really tiny!", new Vector2(500, 300)); });
        tutorialChecks.Add(delegate { displayText("In front of you is the <b><color=#4295f5>Substrate</color><b>. It's an extremely flat sheet of glass\nthat you can build your microscopic structures on top of.\nDon't worry, we've already cleaned it for you!", new Vector2(500, 100)); });
        tutorialChecks.Add(delegate { displayTextFree("Take a look at your <b><color=#4295f5>Substrate</color><b>. You can pan\nthe camera around with the <b><color=#f2971f>WASD keys</color></b>, or the <b><color=#f2971f>arrow keys</color></b>.", new Vector2(400, 200)); });
        tutorialChecks.Add(delegate { displayText("This is a third test", new Vector2(300, 500)); });
    }

}
