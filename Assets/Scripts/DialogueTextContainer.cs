using TMPro;
using UnityEngine;

public class DialogueTextContainer : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    void OnEnable()
    {
        ReactionText.OnShowText += DisplayText;
        ReactionText.OnHideText += HideText;
    }

    void OnDisable()
    {
        ReactionText.OnShowText -= DisplayText;
        ReactionText.OnHideText -= HideText;
    }

    public void DisplayText(DialogueData data)
    {
        _dialogueText.text = data.text;
        _dialogueText.color = data.textColor;
        _canvasGroup.alpha = 1;
    }

    public void HideText()
    {
        _dialogueText.text = "";
        _canvasGroup.alpha = 0;
    }
}

public class DialogueData
{
    public string text = "";
    public Color textColor = Color.black;

    public DialogueData(string text, Color textColor)
    {
        this.text = text;
        this.textColor = textColor;
    }
}
