using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class schematicManager : MonoBehaviour
{
    public GameObject schematicView;
    public GameObject showSchematicView;
    public GameObject hideSchematicView;

    public Image mask;
    public Image schematic;
    Texture maskTexture;
    Texture schematicTexture;

    public GameObject placeholderPrefab;

    // updated by other objects
    public int sliderValue;
    public bool updateSchem;


    // values for the schematic grid
    int gridCount;
    int gridWidth = 7; // 7 pictures max
    int gridStartX = 60;
    int gridStartY = 60;
    int schemWidth = 100;
    bool prevMaskUsed;
    
    void Start()
    {
        prevMaskUsed = false;
        updateSchem = false;
        gridCount = 0;

        schematicView.SetActive(false);
        hideSchematicView.SetActive(false);

        mask = transform.GetChild(0).GetComponent<Image>();
        schematic = transform.GetChild(1).GetComponent<Image>();

        maskTexture = new Texture2D(100, 100);
        schematicTexture = new Texture2D(100, 100);

        mask.material.mainTexture = maskTexture;
        schematic.material.mainTexture = schematicTexture;

    }

    public void toolUsed(bool maskUsed, bool sliderUpdate = false) {
        if (!sliderUpdate)
            updateGrid(prevMaskUsed);

        updateMask(!maskUsed);

        updateSchematic();

        prevMaskUsed = maskUsed;

    }

    public void updateMask(bool makeEmpty = false)
    {
        if (!makeEmpty)
        {

            BitGrid paintCanvasGrid = GameObject.Find("drawing_panel").GetComponent<paint>().grid;

            Texture2D newTexture = paintCanvasGrid.gridToTexture(100, 100);

            newTexture.Apply();

            Graphics.CopyTexture(newTexture, maskTexture);
        }
        else {
            Texture2D maskTexture = new Texture2D(100, 100);
        }
    }

    void updateSchematic(bool sliderUpdate = false)
    {
        GameObject layer = GameObject.Find("LayerStack");

        SchematicGrid crossSection = layer.GetComponent<LayerStackHolder>().crossSectionFromDepth(sliderValue);

        Texture2D newTexture = crossSection.gridToTexture(100, 100);

        newTexture.Apply();
        Graphics.CopyTexture(newTexture, schematicTexture);
    }

    public void updateGrid(bool maskUsed) {
        schematicView.SetActive(true);

        GameObject content = GameObject.Find("schemContent");
        GameObject placeholder = GameObject.Find("Placeholder");

        addToGrid(schematicTexture);

        if(maskUsed)
            addToGrid(maskTexture);

        schematicView.SetActive(false);
    }

    void addToGrid(Texture texture) {

        GameObject content = GameObject.Find("schemContent");
        GameObject placeholder = GameObject.Find("Placeholder");

        GameObject newObject = GameObject.Instantiate(placeholderPrefab);
        newObject.transform.SetParent(content.transform);


        Image newImage = newObject.GetComponent<Image>();

        newImage.material = new Material(Shader.Find("UI/Default"));
        newImage.material.mainTexture = new Texture2D(100, 100);
        Graphics.CopyTexture(texture, newImage.material.mainTexture);

        float posX = gridCount % 7 * 110;
        float posY = -gridCount / 7 * 110;
        Debug.Log(posX + " " + posY);
        newObject.transform.localPosition = placeholder.transform.localPosition + new Vector3(posX, posY, 0);
        newObject.transform.localScale = new Vector3(1, 1, 1);

        gridCount += 1;

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

