using System.Collections;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    [Header("Description")]
    [TextArea]
    [SerializeField] private string _description;

    [Header("Configuration")]
    [SerializeField] private float _waitBeforeReaction;
    [SerializeField] private float _waitAfterReaction;

    protected float _delayTimer;

    private Interactable _interactable;

    protected virtual void React()
    {

    }

    protected virtual void PostReact()
    {
        _interactable.NextReaction();
    }

    protected virtual IEnumerator PerformReactionInTime()
    {
        _delayTimer = _waitBeforeReaction;

        while (_delayTimer > 0)
        {
            yield return new WaitForEndOfFrame();

            _delayTimer -= Time.deltaTime;
        }

        _delayTimer = _waitAfterReaction;

        React();

        while (_delayTimer > 0)
        {
            yield return new WaitForEndOfFrame();

            _delayTimer -= Time.deltaTime;
        }

        PostReact();
    }

    public void ExecuteReaction()
    {
        StartCoroutine(PerformReactionInTime());
    }

    public void SetInteractable(Interactable interactable)
    {
        _interactable = interactable;
    }
}
