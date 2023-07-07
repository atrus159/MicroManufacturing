using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    
    [SerializeField] TextParent[] listOfTexts;
    //private TextManager textManager;

    // Update is called once per frame
    int startCount;
    bool startFlag = false;
    private void Start()
    {
        startCount = 0;
    }

    private void Update()
    {
        if (!startFlag)
        {
            if (startCount < 0)
            {
                startCount++;
            }
            else
            {
                GameObject GSM = GameObject.Find("Global Scene Manager");
                int ind = -1;
                if (GSM)
                {
                    ind = GSM.GetComponent<globalSceneManager>().curState.dialogueIndex;
                }
                if (ind == -1)
                {
                    ind = 0;
                }
                TextManager.instance.i = ind;
                TextManager.instance.EvokeText(listOfTexts);
                if (GSM)
                {
                    GSM.GetComponent<globalSceneManager>().curState.dialogueIndex = -1;
                }
                startFlag = true;
            }
        }  
    }
}
