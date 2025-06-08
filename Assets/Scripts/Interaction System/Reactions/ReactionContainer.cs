using UnityEngine;

public class ReactionContainer : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private ReactionDecision _decision;
    [SerializeField] private Reaction[] _reactions;
    [SerializeField] private bool _oneUseOnly;
    [SerializeField] private bool _reactOnTriggerEnter; aaa
    private bool _used;

    public ReactionDecision Decision => _decision;
    public bool Usable
    {
        get
        {
            if (_oneUseOnly)
            {
                return !_used;
            }
            else
            {
                return true;
            }
        }
    }
    public bool ReactOnTriggerEnter => _reactOnTriggerEnter; aaa

    private void OnValidate()
    {
        GetReactionsInChildren();
    }

    public Reaction[] GetReactions()
    {
        if (!_used) _used = true;

        return _reactions;
    }

    [ContextMenu("GetReactionsInChildren")]
    public void GetReactionsInChildren()
    {
        _reactions = GetComponentsInChildren<Reaction>();
    }
}
