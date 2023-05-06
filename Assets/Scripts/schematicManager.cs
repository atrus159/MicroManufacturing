using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    void updateMask()
    {
        /*if (mask.material.mainTexture) {


        }*/

        //paintCanvas.image.material.mainTexture;
        Graphics.CopyTexture(paintCanvasTexture, maskTexture);
    }

    void updateSchematic()
    {


    }
    // Update is called once per frame
    void Update()
    {
         updateMask();
    }

    public void setPixel(int i, int j, int val)
    {
        /*int scaleFactor = 3;
        Color toSet = (val == 0) ? Color.white : Color.black;
        for (int indI = 0; indI < scaleFactor; indI++)
        {
            for (int indJ = 0; indJ < scaleFactor; indJ++)
            {
                texture.SetPixel(scaleFactor * i + indI, scaleFactor * j + indJ, toSet);
            }
        }*/
    }
}

