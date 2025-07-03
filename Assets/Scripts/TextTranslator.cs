using Managers;
using TMPro;
using UnityEngine;

public class TextTranslator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _key;

    private void OnEnable()
    {
        TranslationManager.OnTextLoaded += AssignText;
    }

    private void OnDisable()
    {
        TranslationManager.OnTextLoaded -= AssignText;
    }

    /// <summary>
    /// Assings text from TranslationManager
    /// </summary>
    private void AssignText()
    {
        _text.text = TranslationManager.Instance.GetText(_key);
    }
}
