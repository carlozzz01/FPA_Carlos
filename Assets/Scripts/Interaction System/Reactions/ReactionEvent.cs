using UnityEngine;
using UnityEngine.Events;

public class ReactionEvent : Reaction
{
    [Header("Event Configuration")]
    [SerializeField] private UnityEvent _OnInteract;

    protected override void React()
    {
        _OnInteract?.Invoke();
    }
}
