using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class schematicManager : MonoBehaviour
{

    public Image mask;
    public Image schematic;

    public GameObject placeholderPrefab;

    public GameObject schematicView;
    public GameObject showSchematicView;
    public GameObject hideSchematicView;

    Texture maskTexture;
    Texture schematicTexture;
    Texture paintCanvasTexture;

    public int sliderValue;


    // for dropdown
    public List<Texture> schematicsList;
    public List<Texture> masksList;

    int gridWidth = 3; // 7 pictures max
    int gridStartX = 60;
    int gridStartY = 60;
    int schemWidth = 100;

    public bool updateSchem;
    
    void Start()
    {
        /*
         mask - here is null
         */
        updateSchem = false;
        updateGrid();

        schematicView.SetActive(false);
        hideSchematicView.SetActive(false);

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

        BitGrid paintCanvasGrid = GameObject.Find("drawing_panel").GetComponent<paint>().grid;

        Texture2D newTexture = paintCanvasGrid.gridToTexture(schematicTexture.width, schematicTexture.height);

        newTexture.Apply();
        Graphics.CopyTexture(newTexture, maskTexture);
    }

    public void updateSchematic(bool sliderUpdate = false)
    {
       // if (!sliderUpdate)
            //updateSchematicsList();

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

    public void updateGrid() {
        GameObject content = GameObject.Find("schemContent");
        GameObject placeholder = GameObject.Find("Placeholder");

        /*foreach (Texture item in schematicsList) {
            Debug.Log(schematicsList.IndexOf(item));
            Transform schemItem = blueprintImage.transform.GetChild(schematicsList.IndexOf(item));
            if (schemItem.gameObject.GetComponent<Image>() != null)
            {
                Graphics.CopyTexture(item, schemItem.gameObject.GetComponent<Image>().material.mainTexture);
            }

        }*/

        GameObject newObject = GameObject.Instantiate(placeholderPrefab);
        newObject.transform.parent = content.transform;
        newObject.transform.localPosition = placeholder.transform.localPosition + new Vector3(110, 0, 0);
        newObject.transform.localScale = new Vector3(1, 1, 1);

    }
    public void onSliderUpdate() {

        GameObject crossSlider = GameObject.Find("crossSlider");
        sliderValue = (int)crossSlider.GetComponent<Slider>().value;
        updateSchematic(true);
    }

    public void onSchematicGridButton() {
        schematicView.SetActive(true);
        showSchematicView.SetActive(false);
        hideSchematicView.SetActive(true);
    }


    public void onSchematicCloseButton() {
        showSchematicView.SetActive(true);
        schematicView.SetActive(false);
        hideSchematicView.SetActive(false);
    }
    public void updateText(string tool) {

        TextMeshProUGUI toolText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        toolText.text = "Last Tool: " + tool;
    }
}

