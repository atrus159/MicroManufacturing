using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schematicDeleteButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public int count;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void onClick()
    {
        GameObject.Find("schematicManager").GetComponent<schematicManager>().onDeleteButton(count);
    }
}
