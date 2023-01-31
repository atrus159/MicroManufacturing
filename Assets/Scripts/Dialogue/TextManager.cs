using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    //Singleton
    public static TextManager instance {get; private set;}
    
    private void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private bool isPlayingText = false;
    private KeyCode advanceTextKeycode = KeyCode.U;

    public void EvokeText(BasicText[] _basicText)
    {
        if (!isPlayingText)
        {
            isPlayingText = true;
            StartCoroutine(PlayText(_basicText));
        }
        isPlayingText = false;
    }

    private IEnumerator PlayText(BasicText[] _basicText)
    {
        foreach (BasicText _text in _basicText)
        {
            _text.Display();
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(WaitForPlayerInput(advanceTextKeycode));
        }
        yield return null;
    }

    private IEnumerator WaitForPlayerInput(KeyCode _keycode)
    {
        while (!Input.GetKeyDown(_keycode))
        {
            yield return null;
        }
    }
}
