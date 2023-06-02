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
    public GameObject lastToolPrefab;

    // updated by other objects
    public int sliderValue;
    public bool updateSchem;
    public string lastText;


    // values for the schematic grid
    int gridCount;
    int gridWidth = 3; // 7 pictures max
    int gridStartX = 60;
    int gridStartY = -210;
    int schemWidth = 100;
    bool prevMaskUsed;
    
    void Start()
    {
        prevMaskUsed = false;
        updateSchem = false;
        gridCount = 0;

        schematicView.SetActive(false);
        hideSchematicView.SetActive(false);

        maskTexture = new Texture2D(100, 100);
        schematicTexture = new Texture2D(100, 100);

        mask.material.mainTexture = maskTexture;
        schematic.material.mainTexture = schematicTexture;

        lastText = null;

    }

    public void toolUsed(bool maskUsed, bool sliderUpdate = false) {
        schematicView.SetActive(true);

        if (!sliderUpdate)
            updateGrid(prevMaskUsed);

        updateMask(!maskUsed);

        updateSchematic();

        prevMaskUsed = maskUsed;

        schematicView.SetActive(false);


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

        float posX = gridCount % gridWidth * 300;
        float posY = - gridCount / gridWidth * 130;

        addToGrid(schematicTexture, new Vector2(posX, posY), lastText);

        if(maskUsed)
            addToGrid(maskTexture, new Vector2(posX + 110, posY));

        gridCount += 1;

        schematicView.SetActive(false);
    }

    void addToGrid(Texture texture, Vector2 offset, string text_add = null) {

        GameObject content = GameObject.Find("schemContent");
        GameObject newObject = GameObject.Instantiate(placeholderPrefab);
        newObject.transform.SetParent(content.transform);

        Image newImage = newObject.GetComponent<Image>();

        newImage.material = new Material(Shader.Find("UI/Default"));
        newImage.material.mainTexture = new Texture2D(100, 100);
        Graphics.CopyTexture(texture, newImage.material.mainTexture);

        newObject.transform.localPosition = new Vector3(gridStartX, gridStartY, 0) + new Vector3(offset.x, offset.y, 0);
        newObject.transform.localScale = new Vector3(1, 1, 1);


        if (text_add != null) {
            GameObject newText = GameObject.Instantiate(lastToolPrefab);
            newText.transform.SetParent(content.transform);

            newText.GetComponent<TextMeshProUGUI>().text = text_add;
            newText.transform.localPosition = new Vector3(gridStartX, gridStartY, 0) + new Vector3(offset.x + 45, offset.y - 80, 0);
            newText.transform.localScale = new Vector3(1, 1, 1);

        }

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

        schematicView.SetActive(true);


        TextMeshProUGUI toolText = GameObject.Find("lastTool").GetComponent<TextMeshProUGUI>();
        lastText = toolText.text;
        toolText.text = "Last Tool:" + "\n" + tool;

        schematicView.SetActive(false);

    }
}

