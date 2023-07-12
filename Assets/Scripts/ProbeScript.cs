using CGTespy.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    public GameObject red_probe_marker;
    public GameObject black_probe_marker;

    Vector3Int redPos;
    Vector3Int blackPos;

    float cubeWidth;
    float cubeHeight;

    public bool visible;
    Vector3 origin;

    GameObject ls;

    // Start is called before the first frame update
    void Start()
    {
        redPos = new Vector3Int(0, 0, 0);
        blackPos = new Vector3Int(0, 0, 0);
        cubeWidth = 0.1f;
        ls = GameObject.Find("LayerStack");
        cubeHeight = ls.GetComponent<LayerStackHolder>().layerHeight;
        origin = ls.transform.position + new Vector3(cubeWidth/2, -cubeHeight/2, cubeWidth/2);
        updateHide(visible);
        //realignRed(50,2,0);
        //realignBlack(50, 2, 99);
    }

    // Update is called once per frame
    public void realignRed(int x, int y, int z){
        Vector3Int position = new Vector3Int(x, y, z);
        red_probe.transform.position = origin + new Vector3(position.x * cubeWidth, position.y * cubeHeight, position.z * cubeWidth);
        red_cube.transform.position = origin + new Vector3(position.x * cubeWidth, position.y * cubeHeight, position.z * cubeWidth);
        red_probe.transform.localRotation = calcRotation(position);
        redPos = position;
        RectTransform dp = GameObject.Find("drawing_panel").GetComponent<RectTransform>();
        red_probe_marker.GetComponent<RectTransform>().anchoredPosition = new Vector3(- dp.rect.width/2 + x* dp.rect.width / 100,  - dp.rect.height / 2 + z * dp.rect.height / 100);
    }

    public void realignBlack(int x, int y, int z)
    {
        Vector3Int position = new Vector3Int(x, y, z);
        black_probe.transform.position = origin + new Vector3(position.x * cubeWidth, position.y * cubeHeight, position.z * cubeWidth);
        black_cube.transform.position = origin + new Vector3(position.x * cubeWidth, position.y * cubeHeight, position.z * cubeWidth);
        black_probe.transform.localRotation = calcRotation(position);
        blackPos = position;
        RectTransform dp = GameObject.Find("drawing_panel").GetComponent<RectTransform>();
        black_probe_marker.GetComponent<RectTransform>().anchoredPosition = new Vector3(- dp.rect.width/2 + x* dp.rect.width / 100,  - dp.rect.height / 2 + z * dp.rect.height / 100);
    }


    Quaternion calcRotation(Vector3Int pos)
    {
        bool lx = pos.x < pos.z;
        bool lhx = BitGrid.gridHeight - pos.x < pos.z;
        float rot = 0.0f;
        if(lx && lhx)
        {
            rot = 90.0f;
        }
        if(!lx && lhx)
        {
            rot = 180.0f;
        }
        if(lx && !lhx)
        {
            rot = 0.0f;
        }
        if(!lx && !lhx)
        {
            rot = -90;
        }

        return Quaternion.Euler(0.0f, rot, 0.0f);
    }

    //        paintCavas = GameObject.Find("drawing_panel").GetComponent<paint>();
    // btn = GetComponent<Button>();
    public void updateHide(bool visible) {
        // GetComponent()<Renderer>.enabled = true/false;
        red_probe.SetActive(visible);
        black_probe.SetActive(visible);
        red_cube.SetActive(visible);
        black_cube.SetActive(visible);
        this.visible = visible;
    }

    public bool getConectionStatus()
    {
        return ls.GetComponent<LayerStackHolder>().getConnectionStatus(rf(redPos),rf(blackPos));
    }

    public bool getCrossConectionStatus(ProbeScript ps)
    {
        bool reds = ls.GetComponent<LayerStackHolder>().getConnectionStatus(rf(redPos), rf(ps.redPos));
        bool blacks = ls.GetComponent<LayerStackHolder>().getConnectionStatus(rf(blackPos), rf(ps.blackPos));
        return reds || blacks;
    }

    Vector3Int rf(Vector3Int vector)
    {
        return new Vector3Int(vector.y, vector.x, vector.z);
    }

    //onConductivityButton()
}
