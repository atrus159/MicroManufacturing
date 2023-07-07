using CGTespy.UI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textCreateBlueprint : BasicText
{

    public GameObject blueprintPrefab;
    public Sprite blueprintImage;
    public string blueprintName;
 
    override public void Initialize()
    {
        base.Initialize();
        if(GameObject.Find("Global Scene Manager").GetComponent<globalSceneManager>().curState.activateFlags[8])
        {
            GameObject.Find("Global Scene Manager").GetComponent<globalSceneManager>().curState.activateFlags[8] = false;
        }
        else
        {
            GameObject newPrint = Instantiate(blueprintPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newPrint.transform.parent = GameObject.Find("Canvas - Tutorial Text").transform;
            newPrint.GetComponentInChildren<TextMeshProUGUI>().text = blueprintName;
            newPrint.transform.GetChild(0).GetComponent<Image>().sprite = blueprintImage;
            newPrint.transform.position = new Vector3(UnityEngine.Screen.width / 2, UnityEngine.Screen.height / 2, 0);
        }
    }
    override public void Display()
    {
        base.Display();
        
    }

}