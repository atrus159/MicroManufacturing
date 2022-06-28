using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class substrateControl : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject subCam;
    public GameObject topCamPrefab;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        subCam = Instantiate(topCamPrefab, transform.position + new Vector3(0, 1001, 0), transform.rotation);
        subCam.GetComponent<Transform>().Rotate(90, 0, 0);
        mainCam.SetActive(true);
        subCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
