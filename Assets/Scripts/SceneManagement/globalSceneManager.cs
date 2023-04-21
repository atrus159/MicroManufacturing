using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class globalSceneManager : MonoBehaviour
{
    string targetSceneFromLevel;
    private void Start()
    {
        targetSceneFromLevel = "";
        gameObject.name = "Global Scene Manager";
        DontDestroyOnLoad(gameObject);
    }

    public void gotoFromLevel(string target)
    {
        targetSceneFromLevel = target;
        SceneManager.LoadScene("PostLevel");
    }

    public void gotoFromMenu()
    {
        SceneManager.LoadScene(targetSceneFromLevel);
    }


}
