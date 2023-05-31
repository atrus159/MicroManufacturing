using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

// Future functionality:

// - realign itself to point towards source and destination point

// - add glowing box for the exact point (aligned with the probes)

// - will be called by level requirement manager?

public class ProbeScript : MonoBehaviour
{

    public GameObject red_probe;
    public GameObject black_probe;

    public GameObject red_cube;
    public GameObject black_cube;
    // Start is called before the first frame update
    void Start()
    {
      updateHide(false);
    }

    // Update is called once per frame
    public void realignRed(Vector3 position, Vector3 rot) {
        red_probe.transform.rotation = Quaternion.Euler(rot);
        red_probe.transform.position = position;

    }

    public void realignBlack(Vector3 position, Vector3 rot)
    {
        black_probe.transform.rotation = Quaternion.Euler(rot);
        red_probe.transform.position = position;

    }


    public void reAligncube(GameObject cube, Vector3Int pos) {

        float substrateLength = 0;
        float cubeLength = substrateLength / 100;
        float cubeHeight = 0f;

        Vector3 zeroPos = new Vector3(0, 0, 0);

        cube.transform.localScale = new Vector3(cubeLength, cube.transform.localScale.y, cubeLength);


        cube.transform.position = new Vector3(zeroPos.x + cubeLength * pos.x, zeroPos.y + cubeHeight * pos.y, zeroPos.z + cubeLength * pos.z);


    }

    //        paintCavas = GameObject.Find("drawing_panel").GetComponent<paint>();
    // btn = GetComponent<Button>();
    public void updateHide(bool visible) {
        // GetComponent()<Renderer>.enabled = true/false;
        red_probe.SetActive(visible);
        black_probe.SetActive(visible);
    }
}
