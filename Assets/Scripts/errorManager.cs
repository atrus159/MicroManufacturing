using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CGTespy.UI;
public class errorManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject errorPrefab;
    GameObject curError;
    GameObject tutorialCanvas;
    List<string> errors;
    void Start()
    {
        tutorialCanvas = GameObject.Find("Canvas - Tutorial Text");
        errors = new List<string>();

    }



    // Update is called once per frame
    void Update()
    {
        if (!curError && errors.Count > 0)
        {
            string newError = errors[0];
            errors.RemoveAt(0);
            curError = Instantiate(errorPrefab);
            curError.GetComponentInChildren<TextMeshProUGUI>().text = newError;
            curError.transform.parent = tutorialCanvas.transform;
            CGTespy.UI.RectTransformPresetApplyUtils.ApplyAnchorPreset(curError.GetComponent<RectTransform>(), TextAnchor.LowerCenter);
            curError.transform.localPosition = new Vector3(-300, -100, 0);
        }
    }

    public void createError(string text)
    {
        errors.Add(text);
    }

}
