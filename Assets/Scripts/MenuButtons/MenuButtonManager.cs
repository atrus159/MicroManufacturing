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
        GameObject.Find("Global Scene Manager").GetComponent<globalSceneManager>().continueFromMenu();
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
        if(GameObject.Find("Global Scene Manager").GetComponent<globalSceneManager>().optionsMenuFlag)
        {
            GameObject.Find("Global Scene Manager").GetComponent<globalSceneManager>().optionsMenuFlag = false;
            GameObject.Find("Global Scene Manager").GetComponent<globalSceneManager>().continueFromMenu();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
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

    public void lv1Button()
    {
        SceneManager.LoadScene("Level1");
    }

    public void lv2Button()
    {
        SceneManager.LoadScene("Level2");
    }

    public void lv3Button()
    {
        SceneManager.LoadScene("Level3");
    }

    public void lv4Button()
    {
        SceneManager.LoadScene("Level4");
    }


    public void onContinueButton()
    {
        GameObject.Find("Global Scene Manager").GetComponent<globalSceneManager>().gotoFromMenu();
    }
}
