using System;
using Managers;
using UnityEngine;

public class ReactionText : Reaction
{
    [Header("Text")]
    [SerializeField] private string _dialogueTextKey;
    [SerializeField] private Color _dialogueTextColor;
    [SerializeField] private float _characterReadTime = 0.1f;

    public static Action<TextData> OnShowText;
    public static Action OnHideText;

    protected override void React()
    {
        string text = TranslationManager.Instance.GetText(_dialogueTextKey);

        TextData data = new TextData(text, _dialogueTextColor, _characterReadTime);

        // if delayTimer is 0, set to dynamic delay based on text length. else, set to editor value
        _delayTimer += text.Length * _characterReadTime;

        OnShowText?.Invoke(data);
    }

    protected override void PostReact()
    {
        OnHideText?.Invoke();

        base.PostReact();
    }
}
