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
        GameObject newPrint = Instantiate(blueprintPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newPrint.transform.parent = GameObject.Find("Canvas - Tutorial Text").transform;
        newPrint.GetComponentInChildren<TextMeshProUGUI>().text = blueprintName;
        newPrint.GetComponentInChildren<Image>().sprite= blueprintImage;
        newPrint.transform.position = new Vector3(100, 0, 100);
    }
    override public void Display()
    {
        base.Display();
        
    }

}