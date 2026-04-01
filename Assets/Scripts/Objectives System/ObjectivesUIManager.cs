using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private float _heightClosed = 125f;
    [SerializeField] private float _heightOpened = 450f;

    [Header("Objective List")]
    [SerializeField] private ObjectiveUIElement _objectiveUIElementPrefab;
    [SerializeField] private RectTransform _container;
    [SerializeField] private CanvasGroup _listGroup;

    private Sequence _seq;

    private bool _isOpened;

    private List<ObjectiveUIElement> _activeElements = new();

    public void CompleteObjective(ObjectiveSO objective)
    {
        var element = _activeElements.FirstOrDefault(e => e.Reference == objective);
        element?.Complete();
    }
    public void Initialize()
    {
        UIAnimator.Fade(_canvasGroup, true);
        _isOpened = false;
    }

    public void Open()
    {
        _isOpened = true;

        Vector2 targetSize = _panel.sizeDelta;

        targetSize.y = _heightOpened;

        if (_seq.IsActive()) _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_panel.DOSizeDelta(targetSize, 0.25f));
        _seq.Append(_listGroup.DOFade(1, 0.25f));
    }

    public void Close()
    {
        _isOpened = false;

        Vector2 targetSize = _panel.sizeDelta;

        targetSize.y = _heightClosed;

        if (_seq.IsActive()) _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_listGroup.DOFade(0, 0.25f));
        _seq.Append(_panel.DOSizeDelta(targetSize, 0.25f));
    }

    public void Toggle()
    {
        if (_isOpened) Close();
        else Open();
    }

    public void AddObjective(ObjectiveSO newObjective)
    {
        if (newObjective.isInvisible) return;
        
        var newElement = Instantiate(_objectiveUIElementPrefab, _container);
        newElement.Initialize(newObjective);
        _activeElements.Add(newElement);
    }
}
