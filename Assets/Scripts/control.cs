using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    //test
    public static int gridWidth = 100;
    public static int gridHeight = 100;
    public struct materialData
    {
        public materialData(Material m, int ef)
        {
            material = m;
            etchFlag = ef;
        }

        public Material material { get; }
        public int etchFlag { get; }

    }

    public Material m_gold;
    public Material m_chromium;
    public Material m_aluminum;
    public Material m_photoresist;

    public enum materialType
    {
        gold,
        chromium,
        aluminum,
        photoresist
    }

    public static Dictionary<materialType, materialData> materialsList = new Dictionary<materialType, materialData>();
    // Start is called before the first frame update
    void Start()
    {
        materialsList.Add(materialType.gold, new materialData(m_gold, 0));
        materialsList.Add(materialType.chromium, new materialData(m_chromium, 0));
        materialsList.Add(materialType.aluminum, new materialData(m_aluminum, 0));
        materialsList.Add(materialType.photoresist, new materialData(m_photoresist, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnValueChanged(float newValue)
    {
        GameObject proc = GameObject.Find("New Process");
        proc.GetComponent<ProcessParent>().OnValueChanged(newValue);
    }
    public void onValueChange(int num)
    {
        GameObject layer = GameObject.Find("LayerStack");
        layer.GetComponent<LayerStackHolder>().onValueChange(num);
    }
}
