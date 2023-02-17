using TMPro;
using UnityEngine;

public class BasicText : TextParent
{
    [SerializeField] private string text;
    [SerializeField] private Vector2 location;

    override public void Initialize()
    {
        base.Initialize();

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
    override public void Display()
    {
        base.Display();
        textBoxText.text = text;
        textBoxPosition.SetPositionAndRotation(location, Quaternion.Euler(0,0,0));
    }
}
