using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SchematicDrawing : MonoBehaviour
{
	public SpriteRenderer mask;
	public SpriteRenderer schematic;
	//public paint paintCanvas;
	// Use this for initialization
	void Start()
	{
		//paintCanvas = GameObject.Find("drawing_panel").GetComponent<paint>();
		mask = GameObject.Find("lastMask").GetComponent<SpriteRenderer>();
        mask = GameObject.Find("lastSchematic").GetComponent<SpriteRenderer>();

    }

	void updateMask() {

	}

	void updateSchematic() {


	}
	// Update is called once per frame
	void Update()
	{
        //mask.sprite. = paintCanvas.texture;
    }
}

