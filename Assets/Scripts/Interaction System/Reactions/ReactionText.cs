using System;
using Managers;
using UnityEngine;

public class ReactionText : Reaction
{
    [Header("Text")]
    [TextArea]
    [SerializeField] private string _dialogueTextKey;
    [SerializeField] private Color _dialogueTextColor;
    [SerializeField] private float _characterReadTime = 0.1f;

    public static Action<DialogueData> OnShowText;
    public static Action OnHideText;

    protected override void React()
    {
        string text = TranslationManager.Instance.GetText(_dialogueTextKey);

        DialogueData data = new DialogueData(text, _dialogueTextColor);

        OnShowText?.Invoke(data);

        // if delayTimer is 0, set to dynamic delay based on text length. else, set to editor value
        _delayTimer = _delayTimer == 0 ? text.Length * _characterReadTime : _delayTimer;
    }

    protected override void PostReact()
    {
        OnHideText?.Invoke();

        base.PostReact();
    }
}
