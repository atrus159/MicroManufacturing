using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerSpawner : MonoBehaviour
{
    public GameObject sceneManagerPrefab;

    private void Start()
    {
        if(!GameObject.Find("Global Scene Manager"))
        {
            Instantiate(sceneManagerPrefab);
        }
    }

}
