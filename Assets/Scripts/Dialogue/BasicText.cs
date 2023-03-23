using TMPro;
using UnityEngine;

public class BasicText : TextParent
{
    [SerializeField] private string text;
    [SerializeField] private Vector2 location;

    override public void Initialize()
    {
        base.Initialize();

        string textWithLines = "";
        int charPerLine = 50;
        int charCount = 0;
        foreach (char c in text)
        {
            textWithLines += c;
            if (c == '*')
            {
                continue;
            }
            if (charCount >= charPerLine)
            {
                if (c == ' ')
                {
                    charCount = -1;
                    textWithLines += "\n";

                }
            }
            charCount++;
        }

        string newText = "";
        bool firstStar = true;
        foreach (char c in textWithLines){
            string curChar = c.ToString();
            if(curChar == "*")
            {
                if (firstStar)
                {
                    curChar = "<color=#03B4FF><i>";
                    firstStar = false;
                }
                else
                {
                    curChar = "</color></i>";
                    firstStar = true;
                }
            }
            newText += curChar;
        }
        text = newText;
    }
    override public void Display()
    {
        base.Display();
        textBoxText.text = text;
        textBoxPosition.GetComponent<RectTransform>().anchoredPosition = location;
        control.setPaused(control.pauseStates.tutorialPaused);
    }
}
