using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Managers;

public class CustomDropdown : MonoBehaviour
{
    [System.Serializable]
    public class OptionData
    {
        public string label;
        public UnityEvent onClick;
    }

    [Header("References")]
    [SerializeField] private Button _dropdownButton;
    [SerializeField] private GameObject _optionsContainer;
    [SerializeField] private TextMeshProUGUI _selectedLabel;
    [SerializeField] private Button[] _optionButtons;

    [Header("Options Data")]
    [SerializeField] private OptionData[] _options;

    private bool _isOpen;

    private void Awake()
    {
        _dropdownButton.onClick.AddListener(Toggle);

        for (int i = 0; i < _optionButtons.Length; i++)
        {
            int index = i;
            _optionButtons[i].onClick.AddListener(() => Select(index));

            if (i < _options.Length)
            {
                _optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _options[i].label;
            }
        }
    }

    private void Start()
    {
        _optionsContainer.SetActive(false);
        _selectedLabel.text = TranslationManager.Instance.GetText(TranslationManager.Instance.CurrentLanguage);
    }

    /// <summary>
    /// Opens/closes the option container
    /// </summary>
    private void Toggle()
    {
        _isOpen = !_isOpen;
        _optionsContainer.SetActive(_isOpen);
    }

    /// <summary>
    /// Selects an option from the container
    /// </summary>
    /// <param name="index"></param>
    private void Select(int index)
    {
        if (index < 0 || index >= _options.Length) return;

        _options[index].onClick?.Invoke();

        _optionsContainer.SetActive(false);
        _isOpen = false;
        _selectedLabel.text = TranslationManager.Instance.GetText(_options[index].label);
    }
}
