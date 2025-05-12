using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Configuration")]
    // [SerializeField] private bool _interactOnEnter;
    [SerializeField] private ReactionContainer _positiveReactions;
    [SerializeField] private ReactionContainer _defaultReactions;

    [Header("Conditions")]
    [SerializeField] private string[] _conditions;


    [Header("Components")]
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _handle;

    private bool _isReacting;
    private Queue<Reaction> _reactions = new Queue<Reaction>();

    private void Awake()
    {
        if (_collider == null) _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _collider.isTrigger = true;
    }

    /// <summary>
    /// Plays the chain of reactions of this Interactable
    /// </summary>
    public void Interact()
    {
        if (!_isReacting)
        {
            // Debug.Log("Interact");

            _isReacting = true;

            if (_handle != null)
            {
                // TODO: invoke event
                // interactor.StartIKAnimation(_handle);
            }

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
