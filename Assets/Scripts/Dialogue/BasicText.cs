using CGTespy.UI;
using TMPro;
using UnityEngine;

public class BasicText : TextParent
{
    [SerializeField] private string text;
    [SerializeField] private Vector2 location;
    [SerializeField] private TextAnchor anchor;

    override public void Initialize()
    {
        base.Initialize();

        string textWithLines = "";
        int charPerLine = 50;
        int charCount = 0;
        foreach (char c in text)
        {
            if (c == '■')
            {
                charCount = -1;
                textWithLines += "\n";
                continue;
            }
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
        RectTransform dialogueSystem = GameObject.Find("DialogueSystem").GetComponent<RectTransform>();
        CGTespy.UI.RectTransformPresetApplyUtils.ApplyAnchorPreset(textBoxPosition.GetComponent<RectTransform>(), anchor);
        CGTespy.UI.RectTransformPresetApplyUtils.ApplyAnchorPreset(dialogueSystem, anchor);
        dialogueSystem.anchoredPosition = new Vector3(0, 0, 0);
        textBoxPosition.GetComponent<RectTransform>().anchoredPosition = location;
        //control.setPaused(control.pauseStates.tutorialPaused);
    }
}
