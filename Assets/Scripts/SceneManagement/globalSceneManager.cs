using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class globalSceneManager : MonoBehaviour
{
    string continueScene;
    string targetSceneFromLevel;
    public bool optionsMenuFlag;
    private void Start()
    {
        targetSceneFromLevel = "";
        gameObject.name = "Global Scene Manager";
        DontDestroyOnLoad(gameObject);
        continueScene = "Level1";
    }

    public void gotoMenuFromLevel(string target)
    {
        continueScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(target);
    }

    public void continueFromMenu()
    {
        SceneManager.LoadScene(continueScene);
        continueScene = "Level1";
    }

    public void gotoFromLevel(string target)
    {
        targetSceneFromLevel = target;
        continueScene = target;
        SceneManager.LoadScene("PostLevel");
    }

    public void gotoFromMenu()
    {
        SceneManager.LoadScene(targetSceneFromLevel);
        continueScene = "Level1";
    }




}
