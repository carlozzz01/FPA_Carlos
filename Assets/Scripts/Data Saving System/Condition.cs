using UnityEngine;

[System.Serializable]
public class Condition
{
    public string id;

    [TextArea]
    public string description;

    [SerializeField] private bool _isConditionMet;

    public bool IsConditionMet => _isConditionMet;

    public void SetState(bool value)
    {
        _isConditionMet = value;
    }
}
