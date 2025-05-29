using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextContainer : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Fade Transition")]
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _afterBuildDelay;
    [SerializeField] private Transform _hiddenPoint;
    [SerializeField] private Transform _shownPoint;

    private bool _isBuilding;

    private void OnEnable()
    {
        ReactionText.OnShowText += DisplayText;
        ReactionText.OnHideText += HideText;
    }

    private void OnDisable()
    {
        ReactionText.OnShowText -= DisplayText;
        ReactionText.OnHideText -= HideText;
    }

    private void Start()
    {
        _canvasGroup.transform.position = _hiddenPoint.position;
        _canvasGroup.alpha = 0;
    }

    public void DisplayText(TextData data)
    {
        StartCoroutine(DisplayText(1, _shownPoint.position, data.text, data.timeBetweenCharacters));
    }

    private IEnumerator FadeTowards(float goalAlpha, Vector3 goalPosition)
    {
        float timer = 0;
        float initialAlpha = _canvasGroup.alpha;
        Vector3 initialPosition = _canvasGroup.transform.position;

        while (timer < _fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(initialAlpha, goalAlpha, timer / _fadeDuration);
            _canvasGroup.transform.position = Vector3.Lerp(initialPosition, goalPosition, timer / _fadeDuration);

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        _canvasGroup.alpha = goalAlpha;
        _canvasGroup.transform.position = goalPosition;
    }

    private IEnumerator BuildText(string text, float timeBetweenCharacters)
    {
        for (int i = 0; i < text.Length; i++)
        {
            _text.text = text.Substring(0, i);

            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }

    private IEnumerator DisplayText(float goalAlpha, Vector3 goalPosition, string text, float timeBetweenCharacters)
    {
        yield return FadeTowards(goalAlpha, goalPosition);

        _isBuilding = true;
        yield return BuildText(text, timeBetweenCharacters);
        _isBuilding = false;
    }

    private IEnumerator HideText(float goalAlpha, Vector3 goalPosition)
    {
        while (_isBuilding)
        {
            yield return null;
        }

        yield return new WaitForSeconds(_afterBuildDelay);

        yield return FadeTowards(goalAlpha, goalPosition);

        _text.text = "";
    }

    public void HideText()
    {
        StartCoroutine(HideText(0, _hiddenPoint.position));
    }
}

public class TextData
{
    public string text { get; private set; }
    public float timeBetweenCharacters { get; private set; }
    public Color textColor = Color.black;

    public TextData(string text)
    {
        this.text = text;
        textColor = Color.black;
        timeBetweenCharacters = 0.15f;
    }

    public TextData(string text, Color textColor)
    {
        this.text = text;
        this.textColor = textColor;
        timeBetweenCharacters = 0.15f;
    }

    public TextData(string text, float timeBetweenCharacters)
    {
        this.text = text;
        textColor = Color.black;
        this.timeBetweenCharacters = timeBetweenCharacters;
    }

    public TextData(string text, Color textColor, float timeBetweenCharacters)
    {
        this.text = text;
        this.textColor = textColor;
        this.timeBetweenCharacters = timeBetweenCharacters;
    }
}
