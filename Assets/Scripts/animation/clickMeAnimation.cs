using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickMeAnimation : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public float frameTime;
    public float cycles;

    float t;
    int curIndex;
    int cycleCount;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        t = 0.0f;
        curIndex = 0;
        image.sprite = sprites[curIndex];
        cycleCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") || TextManager.instance.skipFlag)
        {
            Destroy(gameObject);
        }

        t += Time.deltaTime;
        if(t > frameTime)
        {
            t = 0.0f;
            curIndex += 1;
            if(curIndex >= sprites.Count)
            {
                curIndex = 0;
                cycleCount++;
                if(cycleCount >= cycles)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            image.sprite = sprites[curIndex];
        }
        
    }
}
