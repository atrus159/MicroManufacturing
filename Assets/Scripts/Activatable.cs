using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activatable : MonoBehaviour
{
    public enum buttonTypes
    {
        big,
        small
    }
    public buttonTypes buttonType;

    // Start is called before the first frame update
    public GameObject highlight;

    public void activate()
    {
        gameObject.SetActive(true);
        if (highlight)
        {
            Vector3 offset;
            Transform pt;
            switch (buttonType)
            {
                case buttonTypes.big:
                    offset = new Vector3(0, 0, 0);
                    pt = transform;
                    break;
                case buttonTypes.small:
                    offset = new Vector3(0, 0, 0);
                    pt = transform.parent.parent;
                    break;
                default:
                    offset = new Vector3(0, 0, 0);
                    pt = transform;
                    break;

            }
            GameObject newObj = Instantiate(highlight, transform.position + offset, transform.rotation, pt.parent);
        }
    }
}
