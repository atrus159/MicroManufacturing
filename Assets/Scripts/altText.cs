using CGTespy.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class altText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Vector2 lowLeft;
    [SerializeField]
    private Vector2 upRight;
    [SerializeField]
    string text;
    [SerializeField]
    GameObject altTextPrefab;

    GameObject curAltText = null;
    bool hovering;
    // Update is called once per frame
    float hoverTime;


    float timeTillShow = 1.5f;

    int firstRescaleFlag;
    private void Start()
    {
        hovering = false;
        firstRescaleFlag = 0;
        hoverTime = 0;
    }
    void Update()
    {
        float mx = Input.mousePosition.x;
        float my = Input.mousePosition.y;
        float x = transform.position.x;
        float y = transform.position.y;
        if(mx > x + lowLeft.x && mx < x + upRight.x && my > y + lowLeft.y && my < y + upRight.y)
        {
            
            if(hoverTime < timeTillShow)
            {
                hoverTime += Time.deltaTime;
            }
            else
            {
                if (!hovering)
                {
                    if (curAltText == null)
                    {
                        curAltText = Instantiate(altTextPrefab);
                        curAltText.transform.parent = transform;
                        curAltText.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = text;
                    }
                    if (!curAltText.activeSelf)
                    {
                        curAltText.SetActive(true);
                    }
                    curAltText.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(mx, my, 0), Quaternion.identity);
                    hovering = true;
                }
                if(firstRescaleFlag == 0)
                {
                    firstRescaleFlag = 1;
                }else if(firstRescaleFlag == 1)
                {
                    RectTransform rel = curAltText.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                    //CGTespy.UI.RectTransformPresetApplyUtils.ApplyAnchorPreset(rel, TextAnchor.LowerRight);
                    rel.anchoredPosition = new Vector2(rel.LeftEdgeX(), -rel.LowerEdgeY());
                    firstRescaleFlag = 2;
                }
            }
        }
        else
        {
            if (hovering)
            {
                hoverTime = 0;
                curAltText.SetActive(false);
                hovering = false;
            }
        }

    }
}
