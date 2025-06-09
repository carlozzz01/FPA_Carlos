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
        if (_isReacting) return;

        bool conditionMet = false;
        bool conditionsMet = true;

        foreach (ReactionContainer reactionChain in _reactionContainers)
        {
            foreach (ReactionDecision item in reactionChain.Decisions)
            {
                if (!item.CheckDecision()) conditionsMet = false;
            }

            if (reactionChain.ReactOnTriggerEnter && conditionsMet && reactionChain.Usable)
            {
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

            bool reactionAchieved = false;

            foreach (ReactionContainer reactionChain in _reactionContainers)
            {
                bool conditionsMet = true;
                
                foreach (ReactionDecision item in reactionChain.Decisions)
                {
                    if (item.CheckDecision())
                    {
                        // Debug.Log($"Is decision {item.name} met? {conditionsMet}");
                    }
                    else
                    {
                        conditionsMet = false;
                    }

                    Debug.Log($"Is decision {item.name} met? {conditionsMet}");
                }

                if (conditionsMet && reactionChain.Usable)
                {
                    QueueReactions(reactionChain);

                    reactionAchieved = true;

                    break;
                }
            }

            if (!reactionAchieved) QueueReactions(_defaultReactions);

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
