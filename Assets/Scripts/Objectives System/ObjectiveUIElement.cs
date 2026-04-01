using Managers;
using TMPro;
using UnityEngine;

public class ObjectiveUIElement : MonoBehaviour
{
    [SerializeField] private ObjectiveSO _reference;
    [SerializeField] private GameObject _checkImage;
    [SerializeField] private TextTranslator _text;

    private bool _isCompleted;
    public ObjectiveSO Reference => _reference;

    public void Initialize(ObjectiveSO objectiveSO)
    {
        _reference = objectiveSO;
        _text.AssignKey(_reference.translationKey);
    }

    public void Complete()
    {
        if (_isCompleted) return;

        _isCompleted = true;

        _checkImage.SetActive(true);
    }
}