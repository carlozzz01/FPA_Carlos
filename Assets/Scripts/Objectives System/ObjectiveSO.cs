using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveSO", menuName = "Scriptable Objects/ObjectiveSO")]
public class ObjectiveSO : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField][TextArea] private string _description;
    [SerializeField] private string _translationKey;
    [SerializeField] private ReactionDecision[] _conditions;
    [SerializeField] private bool _isInvisible;

    private bool _isCompleted;

    public string translationKey => _translationKey;
    public bool isCompleted => _isCompleted;
    public bool isInvisible => _isInvisible;

    private void OnEnable()
    {
        _isCompleted = false;
    }

    public bool CheckCompletion()
    {
        foreach (var condition in _conditions)
        {
            if (!condition.CheckDecision()) return false;
        }

        return true;
    }

    public void SetCompleted(bool value)
    {
        _isCompleted = value;
    }

    public void Reset()
    {
        _isCompleted = false;
    }
}