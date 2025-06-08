using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Interactable_Reaction : Interactable
{
    [Header("Configuration")]
    [SerializeField] private ReactionContainer _positiveReactions;
    [SerializeField] private ReactionContainer _defaultReactions;

    [Header("Conditions")]
    [SerializeField] private string[] _conditions;

    [Header("Debug")]
    private bool _isReacting;
    private Queue<Reaction> _reactions = new Queue<Reaction>();

    private void Start()
    {
        // _collider.isTrigger = _interactOnTriggerEnter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _interactOnTriggerEnter)
        {
            Interact(null);
        }
    }

    /// <summary>
    /// Plays the chain of reactions of this Interactable
    /// </summary>
    public override void Interact(PlayerInteraction player)
    {
        if (!_isReacting)
        {
            Debug.Log("Interact");

            _isReacting = true;

            bool conditionsMet = true;

            foreach (string conditionID in _conditions)
            {
                if (!DataManager.Instance.IsConditionMet(conditionID))
                {
                    conditionsMet = false;
                    break;
                }
            }

            if (conditionsMet && _conditions.Length > 0)
            {
                QueueReactions(_positiveReactions);
            }
            else
            {
                QueueReactions(_defaultReactions);
            }

            NextReaction();
        }
    }

    private void QueueReactions(ReactionContainer reactionContainer)
    {
        _reactions.Clear();

        foreach (Reaction reaction in reactionContainer.GetReactions())
        {
            reaction.SetInteractable(this);

            _reactions.Enqueue(reaction);
        }
    }

    public void NextReaction()
    {
        if (_reactions.Count > 0)
        {
            _reactions.Dequeue().ExecuteReaction();
        }
        else
        {
            _isReacting = false;
        }
    }
}
