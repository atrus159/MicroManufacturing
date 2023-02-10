using UnityEngine;

public class BasicText : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private Vector2 location;
    GameObject textBox;

    public void Start()
    {
        textBox = GameObject.Find("Text");
    }
    public void Display()
    {
        Debug.Log("\"" + text + "\"" + "will be shown at location: " + location);
    }
}
