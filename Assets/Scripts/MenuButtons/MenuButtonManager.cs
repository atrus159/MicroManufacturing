using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonPress()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnLevelSelectButtonPressed()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnOptionsButtonPress()
    {
        SceneManager.LoadScene("Options");
    }

    public void OnCreditsButtonPress()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
    public void OnFreePlayButton()
    {
        SceneManager.LoadScene("FreePlayLevel");
    }


    public void OnBackButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnFullScreenToggle()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            GameObject.Find("FullScreenButton").GetComponent<TextMeshProUGUI>().text = "Full Screen";
        }
        else
        {
            Screen.fullScreen = true;
            GameObject.Find("FullScreenButton").GetComponent<TextMeshProUGUI>().text = "Windowed Mode";
        }
    }
}
