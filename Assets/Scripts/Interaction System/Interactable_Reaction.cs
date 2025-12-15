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

                NextReaction();

                conditionMet = true;

                break;
            }
        }

        if (_defaultReactions == null || !_defaultReactions.Usable) return;

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
            _isReacting = true;

            bool reactionAchieved = false;

            foreach (ReactionContainer reactionChain in _reactionContainers)
            {
                bool conditionsMet = true;

                foreach (ReactionDecision item in reactionChain.Decisions)
                {
                    if (item.CheckDecision())
                    {

                    }
                    else
                    {
                        conditionsMet = false;
                    }
                }

                if (conditionsMet && reactionChain.Usable)
                {
                    Debug.Log("decision true");

                    QueueReactions(reactionChain);

                    NextReaction();

                    reactionAchieved = true;

                    break;
                }
            }

            if (reactionAchieved) return;

            if (_defaultReactions == null || !_defaultReactions.Usable)
            {
                _isReacting = false;
                return;
            }

            QueueReactions(_defaultReactions);
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
