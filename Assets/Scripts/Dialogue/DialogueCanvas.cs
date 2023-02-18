using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    
    [SerializeField] TextParent[] listOfTexts;
    //private TextManager textManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TextManager.instance.EvokeText(listOfTexts);
        }
    }
}
