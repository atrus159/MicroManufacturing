using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class schematicManager : MonoBehaviour
{

    public Image mask;
    public Image schematic;
    Texture maskTexture;
    Texture schematicTexture;
    Texture paintCanvasTexture;

    public int sliderValue;


    // for dropdown
    public List<Texture> schematicsList;
    public List<Texture> masksList;
    
    void Start()
    {
        /*
         mask - here is null
         */

        paintCanvasTexture = GameObject.Find("drawing_panel").GetComponent<paint>().texture;
        mask = transform.GetChild(0).GetComponent<Image>();
        maskTexture = new Texture2D(paintCanvasTexture.width, paintCanvasTexture.height);
        mask.material.mainTexture = maskTexture;
        schematic = transform.GetChild(1).GetComponent<Image>();
        schematicTexture = new Texture2D(paintCanvasTexture.width, paintCanvasTexture.height);
        schematic.material.mainTexture = schematicTexture;

        schematicsList = new List<Texture>();
        masksList = new List<Texture>();
    }

    public void updateMask()
    {
        Graphics.CopyTexture(paintCanvasTexture, maskTexture);
    }

    public void updateSchematic(bool sliderUpdate = false)
    {
        if (!sliderUpdate)
            updateSchematicsList();

        GameObject layer = GameObject.Find("LayerStack");

        SchematicGrid crossSection = layer.GetComponent<LayerStackHolder>().crossSectionFromDepth(sliderValue);

        int width = schematicTexture.width;
        int height = schematicTexture.height;

        Texture2D newTexture = crossSection.gridToTexture(width, height);

        newTexture.Apply();
        Graphics.CopyTexture(newTexture, schematicTexture);
        //updateDropdown();
    }

    /*
     * Only called when a new operation has been done!!!
     */
    public void updateSchematicsList() {

        Texture2D copy = new Texture2D(schematicTexture.width, schematicTexture.height);
        Graphics.CopyTexture(schematicTexture, copy);
        schematicsList.Add(copy);

        if (schematicsList.Count > 3)
            schematicsList.RemoveAt(0);

        //updateDropdown();
    }

    public void updateMasksList()
    {

    }

    public void updateDropdown() {
        GameObject blueprintImage = GameObject.Find("schemList");

        foreach (Texture item in schematicsList) {
            Debug.Log(schematicsList.IndexOf(item));
            Transform schemItem = blueprintImage.transform.GetChild(schematicsList.IndexOf(item));
            if (schemItem.gameObject.GetComponent<Image>() != null)
            {
                Graphics.CopyTexture(item, schemItem.gameObject.GetComponent<Image>().material.mainTexture);
            }

        }

    }
    public void onSliderUpdate() {

        GameObject crossSlider = GameObject.Find("crossSlider");
        sliderValue = (int)crossSlider.GetComponent<Slider>().value;
        updateSchematic(true);
    }
    
    public void updateText(string tool) {

        TextMeshProUGUI toolText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        toolText.text = "Last Tool: " + tool;
    }
}

