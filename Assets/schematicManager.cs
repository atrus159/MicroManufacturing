using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class schematicManager : MonoBehaviour
{

    public Image mask;
    public Image schematic;
    //public paint paintCanvas;
    // Use this for initialization
    void Start()
    {
        /*
         mask - here is null
         */
        mask = GameObject.Find("lastMask").GetComponent<Image>();
        schematic = GameObject.Find("lastSchematic").GetComponent<Image>();

    }

    void updateMask()
    {
        paint paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();
        /*if (mask.material.mainTexture) {


        }*/

         //paintCanvas.image.material.mainTexture;

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

