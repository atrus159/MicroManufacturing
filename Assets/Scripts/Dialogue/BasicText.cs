using UnityEngine;

public class BasicText : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private Vector2 location;

    public void Display()
    {
        Debug.Log("\"" + text + "\"" + "will be shown at location: " + location);
    }
}
