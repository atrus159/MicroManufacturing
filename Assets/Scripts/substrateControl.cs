using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class substrateControl : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject subCam;
    public GameObject viewCam;
    public GameObject topCamPrefab;
    // Start is called before the first frame update
    void Start()
    {
        viewCam.transform.SetPositionAndRotation(transform.position + new Vector3(0, 10, 0), transform.rotation);
        viewCam.GetComponent<Transform>().Rotate(90, 0, 0);
        subCam = Instantiate(topCamPrefab, transform.position + new Vector3(0, 2000, 0), transform.rotation);
        subCam.GetComponent<Transform>().Rotate(90, 0, 0);
        mainCam.SetActive(true);
        subCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
