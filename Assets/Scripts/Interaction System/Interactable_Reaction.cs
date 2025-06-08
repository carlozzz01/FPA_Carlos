using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Interactable_Reaction : Interactable
{
    [Header("Configuration")]
    [SerializeField] private ReactionContainer _defaultReactions;
    [SerializeField] private List<ReactionContainer> _reactionContainers;

    [Header("Debug")]
    private bool _isReacting;
    private Queue<Reaction> _reactionQueue = new Queue<Reaction>();

    private void Start()
    {
        // _collider.isTrigger = _interactOnTriggerEnter;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Player") && _interactOnTriggerEnter)
        // {
        //     Interact(null);
        // }

        bool conditionMet = false;

        foreach (ReactionContainer reactionChain in _reactionContainers)
        {
            if (reactionChain.ReactOnTriggerEnter && reactionChain.Decision.CheckDecision() && reactionChain.Usable)
            {
                aaa

                QueueReactions(reactionChain);

                conditionMet = true;

                break;
            }
        }

        if (!conditionMet && _defaultReactions.ReactOnTriggerEnter) QueueReactions(_defaultReactions);

        if (conditionMet || _defaultReactions.ReactOnTriggerEnter) NextReaction();
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

            bool conditionMet = false;

            foreach (ReactionContainer reactionChain in _reactionContainers)
            {
                if (reactionChain.Decision.CheckDecision() && reactionChain.Usable)
                {
                    QueueReactions(reactionChain);

                    conditionMet = true;

                    break;
                }
            }

            if (!conditionMet) QueueReactions(_defaultReactions);

            NextReaction();
        }
    }

    private void QueueReactions(ReactionContainer reactionContainer)
    {
        _reactionQueue.Clear();

        foreach (Reaction reaction in reactionContainer.GetReactions())
        {
            reaction.SetInteractable(this);

            _reactionQueue.Enqueue(reaction);
        }
    }

    public void NextReaction()
    {
        if (_reactionQueue.Count > 0)
        {
            _reactionQueue.Dequeue().ExecuteReaction();
        }
        else
        {
            _isReacting = false;
        }
    }
}
