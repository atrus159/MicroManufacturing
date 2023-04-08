using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    //Singleton
    public static TextManager instance {get; private set;}
    bool holdFlag;
    GameObject TextBox;
    bool goForward;
    GameObject placeCount;
    public bool skipFlag;
    string skipOnObjFlag;
    private void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            TextBox = GameObject.Find("Textbox");
            holdFlag = false;
        }
        goForward = true;
        placeCount = GameObject.Find("placeCount");
        placeCount.SetActive(false);
        skipFlag = false;
        skipOnObjFlag = "";
    }
    private bool isPlayingText = false;
    private KeyCode advanceTextKeycode = KeyCode.Space;
    private KeyCode backTextKeycode = KeyCode.LeftShift;

    public void EvokeText(TextParent[] _Text)
    {
        if (!isPlayingText)
        {
            isPlayingText = true;
            StartCoroutine(PlayText(_Text));
        }
        isPlayingText = false;
    }

    public void startHold()
    {
        holdFlag = true;
    }
    public void endHold()
    {
        holdFlag = false;
        skipFlag = true;
    }

    private IEnumerator PlayText(TextParent[] _Text)
    {
        int currentInd = 0;
        int i = 0;
        int totalFreeTextCount = 0;
        int currentFreeTextCount = 0;
        while( i< _Text.Length)
        {
            TextParent _text = _Text[i];
            _text.Display();
            skipOnObjFlag = _text.skipOnObj;
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(WaitForPlayerInput());
            yield return new WaitUntil(() => holdFlag == false);
            if (skipFlag)
            {
                while (_Text[i].GetType() != typeof(FreeText))
                {
                    i++;
                    currentInd++;
                }
                skipFlag = false;
            }
            if (goForward)
            {
                i++;
                if (i < currentInd && _Text[i].GetType() == typeof(FreeText))
                {
                    i++;
                    currentFreeTextCount ++;
                }
                if(i > currentInd)
                {
                    currentInd= i;
                    if(_Text[i].GetType() == typeof(FreeText))
                    {
                        totalFreeTextCount++;
                        currentFreeTextCount++;
                    }
                }
                else if(i < currentInd)
                {
                    updatePlaceCount(i - currentFreeTextCount + 1, currentInd - totalFreeTextCount + 1);
                }
                else
                {
                    placeCount.SetActive(false);
                }
                continue;
            }
            if (!goForward && i-1 >= 0)
            {
                placeCount.SetActive(true);
                i--;
                if (_Text[i].GetType() == typeof(FreeText))
                {
                    i--;
                    currentFreeTextCount--;
                }
                updatePlaceCount(i - currentFreeTextCount + 1, currentInd - totalFreeTextCount + 1);
            }
        }
        yield return null;
    }

    void updatePlaceCount(int numerator, int denominator)
    {
        placeCount.GetComponent<TextMeshProUGUI>().text = "(" + numerator + "/" + denominator + ")";
    }
    
    public GameObject GetTextBox()
    {
        return TextBox;
    }

    private IEnumerator WaitForPlayerInput()
    {
        while ((!Input.GetKeyDown(advanceTextKeycode) && !Input.GetKeyDown(backTextKeycode) && holdFlag == false && skipFlag == false) || control.isPaused() == control.pauseStates.menuPaused)
        {
            if(skipOnObjFlag != "" && GameObject.Find(skipOnObjFlag)){
                skipOnObjFlag = "";
                break;
            }
            yield return null;
        }
        if (Input.GetKeyDown(advanceTextKeycode))
        {
            goForward = true;
        }
        if (Input.GetKeyDown(backTextKeycode))
        {
            goForward = false;
        }
    }
}
