using Managers;
using TMPro;
using UnityEngine;

public class TextTranslator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _key;

    private void OnEnable()
    {
        TranslationManager.OnTextLoaded += SetText;
    }

    private void OnDisable()
    {
        TranslationManager.OnTextLoaded -= SetText;
    }

    /// <summary>
    /// Assings text from TranslationManager
    /// </summary>
    private void SetText()
    {
        _text.text = TranslationManager.Instance.GetText(_key);
    }

    public void AssignKey(string newKey)
    {
        _key = newKey;

        SetText();
    }
}
