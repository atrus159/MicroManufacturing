using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCreator : MonoBehaviour
{

    public GameObject SpinCaster;

    public GameObject lightBeamPrefab;
    GameObject substrate;
    Vector2 bottomLeft;
    Vector2 topRight;

    enum states
    {
        standby,
        traveling
    }

    Vector3 beam1Pos, beam2Pos;

    GameObject beam1;
    GameObject beam2;
    bool middleFlag;
    float beamSpeed = 10.0f;
    int beamCrossHeight = 20;

    states curLightBeamState;

    // Start is called before the first frame update
    void Start()
    {
        substrate = GameObject.Find("Substrate");
        bottomLeft = new Vector2(substrate.transform.position.x - substrate.transform.lossyScale.x / 2, substrate.transform.position.z - substrate.transform.lossyScale.z / 2);
        topRight = new Vector2(substrate.transform.position.x + substrate.transform.lossyScale.x / 2, substrate.transform.position.z + substrate.transform.lossyScale.z / 2);
        curLightBeamState = states.standby;
        beam1 = Instantiate(lightBeamPrefab, new Vector3(bottomLeft.x, substrate.transform.position.y, topRight.y), Quaternion.Euler(0, 0, 0));
        beam1.SetActive(false);
        beam2 = Instantiate(lightBeamPrefab, new Vector3(topRight.x, substrate.transform.position.y, topRight.y), Quaternion.Euler(0, 0, 0));
        beam2.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (curLightBeamState == states.traveling)
        {
            beam1.transform.position += new Vector3(beamSpeed * Time.deltaTime, 0, -beamSpeed * Time.deltaTime);
            beam2.transform.position += new Vector3(-beamSpeed * Time.deltaTime, 0, -beamSpeed * Time.deltaTime);
            float yPos = beam1.transform.position.z - bottomLeft.y - (topRight.y - bottomLeft.y) / 2;
            float xPos = beam1.transform.position.x - bottomLeft.x - (topRight.x - bottomLeft.x) / 2;
            float angle1 = Mathf.Rad2Deg * Mathf.Atan2(beamCrossHeight, yPos) - 90;
            float angle2 = Mathf.Rad2Deg * Mathf.Atan2(beamCrossHeight, xPos) - 90;
            beam1.transform.rotation = Quaternion.Euler(angle1, 0, -angle2);
            beam2.transform.rotation = Quaternion.Euler(angle1, 0, angle2);
            if (beam1.transform.position.z <= topRight.y - (topRight.y - bottomLeft.y) / 2 && middleFlag == false)
            {
                middleFlag = true;
                GameObject.Find("LayerStack").GetComponent<LayerStackHolder>().etchLayer(control.materialType.photoresistComplement);
                GameObject.Find("Control").GetComponent<control>().PhotoResistEdge.SetActive(false);
            }
            if (beam1.transform.position.z <= bottomLeft.y)
            {
                beam1.SetActive(false);
                beam2.SetActive(false);
                curLightBeamState = states.standby;
            }
        }

    }


    public void makeSpinCaster()
    {
        GameObject newSpin = Instantiate(SpinCaster);
        newSpin.transform.position = new Vector3(-9, -27, 19);
        newSpin.transform.rotation = Quaternion.Euler(-90, 0, -90);
    }

    public void startBeams()
    {
        middleFlag = false;
        beam1.transform.position = new Vector3(bottomLeft.x, substrate.transform.position.y, topRight.y);
        beam2.transform.position = new Vector3(topRight.x, substrate.transform.position.y, topRight.y);
        beam1.SetActive(true);
        beam2.SetActive(true);
        curLightBeamState = states.traveling;

    }

}
