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

    public GameObject deleteButton;

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
    int gridStartY = -240;
    int schemWidth = 100;
    bool prevMaskUsed;

    bool firstFlag;

    List<GameObject> steps;
    
    void Start()
    {
        maskTexture = new Texture2D(100, 100);
        schematicTexture = new Texture2D(100, 100);


        prevMaskUsed = false;
        updateSchem = false;
        gridCount = 0;
        onSliderUpdate();
        schematicView.SetActive(false);
        hideSchematicView.SetActive(false);

        mask.material.mainTexture = maskTexture;
        schematic.material.mainTexture = schematicTexture;

        lastText = null;
        firstFlag = false;
        steps = new List<GameObject>();
    }

    public void toolUsed(bool maskUsed, bool sliderUpdate = false) {
        bool alreadyActive = schematicView.activeSelf;
        schematicView.SetActive(true);

        if (!sliderUpdate)
        {
            if (firstFlag)
            {
                updateGrid(prevMaskUsed);
            }
            else
            {
                firstFlag = true;
            }
        }

        updateMask(!maskUsed);

        updateSchematic();


        prevMaskUsed = maskUsed;

        if (!alreadyActive)
        {
            schematicView.SetActive(false);
        }
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
        bool alreadyActive = schematicView.activeSelf;
        schematicView.SetActive(true);

        float posX = gridCount % gridWidth * 300;
        float posY = - gridCount / gridWidth * 130;

        if (lastText.Length > 11)
            lastText = lastText.Substring(11);



        if (maskUsed)
        {
            addToGrid(schematicTexture, new Vector2(posX, posY), lastText, maskTexture);
        }
        else
        {
            addToGrid(schematicTexture, new Vector2(posX, posY), lastText);
        }


        gridCount += 1;
        if (!alreadyActive)
        {
            schematicView.SetActive(false);
        }
    }

    public void onDeleteButton(int index)
    {
        GameObject.Destroy(steps[index]);
        steps.RemoveAt(index);
        for(int i = index; i < steps.Count; i++)
        {
            float posX = i % gridWidth * 300;
            float posY = -i / gridWidth * 130;
            steps[i].transform.SetLocalPositionAndRotation(new Vector3(gridStartX, gridStartY, 0) + new Vector3(posX, posY, 0), Quaternion.identity);
            steps[i].GetComponentInChildren<schematicDeleteButtonHandler>().count = i;
        }
        gridCount--;
    }

    void addToGrid(Texture schematicTr, Vector2 offset, string text_add = null, Texture maskTr = null) {

        GameObject content = GameObject.Find("schemContent");

        GameObject newContainer = new GameObject();
        newContainer.AddComponent<RectTransform>();
        newContainer.transform.SetParent(content.transform);

        GameObject newSchematic = GameObject.Instantiate(placeholderPrefab);
        newSchematic.transform.SetParent(newContainer.transform);


        Image schematicImage = newSchematic.GetComponent<Image>();

        schematicImage.material = new Material(Shader.Find("UI/Default"));
        schematicImage.material.mainTexture = new Texture2D(100, 100);
        Graphics.CopyTexture(schematicTr, schematicImage.material.mainTexture);


        newContainer.transform.localPosition = new Vector3(gridStartX, gridStartY, 0) + new Vector3(offset.x, offset.y, 0);
        newSchematic.transform.localPosition = new Vector3(0, 0, 0);
        newSchematic.transform.localScale = new Vector3(1, 1, 1);

        if(maskTr != null)
        {
            GameObject newMask = GameObject.Instantiate(placeholderPrefab);
            newMask.transform.SetParent(newContainer.transform);


            Image maskImage = newMask.GetComponent<Image>();

            maskImage.material = new Material(Shader.Find("UI/Default"));
            maskImage.material.mainTexture = new Texture2D(100, 100);
            Graphics.CopyTexture(maskTr, maskImage.material.mainTexture);

            newMask.transform.localPosition = new Vector3(110, 0, 0);
            newMask.transform.localScale = new Vector3(1, 1, 1);

        }

        if (text_add != null) {
            GameObject newText = GameObject.Instantiate(lastToolPrefab);
            newText.transform.SetParent(newContainer.transform);

            newText.GetComponent<TextMeshProUGUI>().text = text_add;
            newText.transform.localPosition = new Vector3( 45, - 80, 0);
            newText.transform.localScale = new Vector3(1, 1, 1);

        }

        GameObject db = GameObject.Instantiate(deleteButton);
        db.transform.SetParent(newContainer.transform);
        db.transform.localPosition = new Vector3(190, 50, 0);
        db.transform.localScale = new Vector3(1, 1, 1);
        db.GetComponent<schematicDeleteButtonHandler>().count = gridCount;
        steps.Add(newContainer);
        GameObject.Find("endSpacer_0").transform.SetAsLastSibling();
        GameObject.Find("endSpacer_1").transform.SetAsLastSibling();
        GameObject.Find("endSpacer_2").transform.SetAsLastSibling();

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
        GameObject.Find("Main Camera").GetComponent<OrbitCamera>().LockOut();
    }

    public void onSchematicCloseButton() {
        showSchematicView.SetActive(true);
        schematicView.SetActive(false);
        hideSchematicView.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<OrbitCamera>().UnlockOut();
    }
    public void updateText(string tool) {
        bool alreadyActive = schematicView.activeSelf;
        schematicView.SetActive(true);


        TextMeshProUGUI toolText = GameObject.Find("lastTool").GetComponent<TextMeshProUGUI>();
        lastText = toolText.text;
        toolText.text = "Last Tool:" + "\n" + tool;
        if(!alreadyActive )
        {
            schematicView.SetActive(false);
        }
    }
}

