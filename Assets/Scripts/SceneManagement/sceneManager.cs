using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    //Makes singleton of sceneManager
    public static sceneManager instance;
    //To access the sceneManager from any script, use
    //sceneManager.instance.functionYouWantToCall()

    //Const Reference To Main Menu
    private const string mainMenu = "MainMenu";
    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
