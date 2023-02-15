using TMPro;
using UnityEngine;

public class BasicText : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private Vector2 location;
    TextMeshProUGUI textBoxText;
    RectTransform textBoxPosition;
    TextGenerationSettings settings;
    TextGenerator generator;
    bool initialFlag = false;

    public void Initialize()
    {
        textBoxText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        textBoxPosition = GameObject.Find("Textbox").GetComponent<RectTransform>();
        initialFlag = true;
        string newText = "";
        int charPerLine = 50;
        int charCount = 0;
        foreach(char c in text)
        {
            newText += c;
            if (charCount >= charPerLine)
            {
                if(c == ' ')
                {
                    charCount = -1;
                    newText += "\n";

                }
            }
            charCount++;
        }
        text = newText;


    }
    public void Display()
    {
        if (!initialFlag)
        {
            Initialize();
        }
        textBoxText.text = text;
        textBoxPosition.SetPositionAndRotation(location, Quaternion.Euler(0,0,0));
    }
}
