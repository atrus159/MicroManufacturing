using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject highlight;

    public void activate()
    {
        gameObject.SetActive(true);
        if (highlight)
        {
            Instantiate(highlight, transform);
        }
    }
}
