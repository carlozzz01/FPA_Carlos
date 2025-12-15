using System;
using Managers;
using TMPro;
using UnityEngine;

public class ContextMessageManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _fadeDuration = 0.25f;

    private void OnEnable()
    {
        PlayerInteraction.OnContextGiven += ShowContext;
        PlayerInteraction.OnContextLost += HideContext;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnContextGiven -= ShowContext;
        PlayerInteraction.OnContextLost -= HideContext;
    }

    private void ShowContext(ContextMessageSO sO)
    {
        _text.text = TranslationManager.Instance.GetText(sO.Message);

        UIAnimator.Fade(_canvasGroup, true, _fadeDuration);
    }

    private void HideContext()
    {
        UIAnimator.Fade(_canvasGroup, false, _fadeDuration);
    }
}
