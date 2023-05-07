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
    //public paint paintCanvas;
    // Use this for initialization
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
    }

    public void updateMask()
    {
        Graphics.CopyTexture(paintCanvasTexture, maskTexture);
    }

    public void updateSchematic()
    {
        GameObject layer = GameObject.Find("LayerStack");
        BitGrid crossSection = layer.GetComponent<LayerStackHolder>().crossSectionFromDepth(50);

        int width = schematicTexture.width;
        int height = schematicTexture.height;

        Texture2D newTexture = new Texture2D(schematicTexture.width, schematicTexture.height);
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++) {
                Color toSet = (crossSection.getPoint(i / (width/ 100), j / (width / 100)) == 0) ? Color.white : Color.black;
                newTexture.SetPixel(i, j, toSet);
             }

        newTexture.Apply();
        Graphics.CopyTexture(newTexture, schematicTexture);
    }

    public void updateText(string tool) {

        TextMeshProUGUI toolText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        toolText.text = "Last Tool: " + tool;
    }
}

