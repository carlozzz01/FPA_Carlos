using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using Managers;

public class ItemInspectorManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private CanvasGroup _mainCanvasGroup;
    [SerializeField] private float _fadeDuration = 1;
    [SerializeField] private InspectorItem _currentItem;
    [SerializeField] private InspectorItem[] _items;

    [Header("Text elements")]
    [SerializeField] private float _textDisplayDelay;
    [SerializeField] private CanvasGroup _textCanvasGroup;
    [SerializeField] private TextMeshProUGUI _text;

    private Coroutine _fadeCoroutine;
    private float _fadeTimer;

    private static ItemInspectorManager _instance;
    public static ItemInspectorManager Instance => _instance;

    public static Action OnInspectStarted;
    public static Action OnInspectCanceled;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _fadeTimer = _fadeDuration;
    }

    public void ShowItem(string itemID)
    {
        SelectNewItem(itemID);

        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(DisplayItem());

        OnInspectStarted?.Invoke();
    }

    private void SelectNewItem(string itemID)
    {
        if (_currentItem != null) _currentItem.SetActive(false);

        _currentItem = _items.FirstOrDefault(i => i.ID == itemID);
        
        _currentItem.SetActive(true);
    }

    public void StopInspect()
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(HideItem());

        OnInspectCanceled?.Invoke();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float goalAlpha)
    {
        float startingAlpha = canvasGroup.alpha;

        _fadeTimer = _fadeDuration - _fadeTimer;

        while (_fadeTimer < _fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startingAlpha, goalAlpha, _fadeTimer / _fadeDuration);

            _fadeTimer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = goalAlpha;

        _fadeTimer = _fadeDuration;
    }

    private IEnumerator DisplayItem()
    {
        yield return StartCoroutine(FadeCanvasGroup(_mainCanvasGroup, 1));

        if (_currentItem.HasText)
        {
            yield return new WaitForSeconds(_textDisplayDelay);

            _text.text = TranslationManager.Instance.GetText(_currentItem.ID);

            yield return StartCoroutine(FadeCanvasGroup(_textCanvasGroup, 1));
        }
    }

    private IEnumerator HideItem()
    {
        yield return StartCoroutine(FadeCanvasGroup(_mainCanvasGroup, 0));

        if (_currentItem.HasText) _textCanvasGroup.alpha = 0;
    }
}
