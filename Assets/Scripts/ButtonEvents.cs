using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public void onNewGameButton()
    {
        //StartCoroutine(LoadYourAsyncScene("Level1"));
        SceneManager.LoadScene("Level1");
    }

    public void onOptionsButton()
    {

    }


    /*IEnumerator LoadYourAsyncScene(string sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }*/
}
