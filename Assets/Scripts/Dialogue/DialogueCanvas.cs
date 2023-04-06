using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    
    [SerializeField] TextParent[] listOfTexts;
    //private TextManager textManager;

    // Update is called once per frame

    private void Start()
    {
        TextManager.instance.EvokeText(listOfTexts);
    }

}
