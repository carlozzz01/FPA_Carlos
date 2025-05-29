using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player _player;
    [SerializeField] private Image _uiIndicator;
    [SerializeField] private Transform _pickableHolder;

    [Header("Configuration")]
    [SerializeField] private LayerMask _whatIsInteractable;
    [SerializeField] private float _range;

    [SerializeField] public Interactable currentInteractable { get; private set; }
    [SerializeField] private bool _isHoldingPickable;

    public Transform PickableHolder => _pickableHolder;
    public bool IsHoldingPickable => _isHoldingPickable;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(_player.Head.position, _player.Head.forward * _range);
    }

    private void OnEnable()
    {
        _player.OnInteractInput += OnInteract;
    }

    private void OnDisable()
    {
        _player.OnInteractInput -= OnInteract;
    }

    private void FixedUpdate()
    {
        CheckForInteractable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentInteractable != null)
            {
                Interact();
            }
        }
    }

    private void Interact()
    {
        _player.OnInteractionStarted.Invoke(currentInteractable);

        currentInteractable.Interact(this);

        if (currentInteractable is Pickable)
        {
            if (_isHoldingPickable)
            {
                currentInteractable = null;
                _isHoldingPickable = false;
            }
            else
            {
                _isHoldingPickable = true;
            }
        }
    }

    private void CheckForInteractable()
    {
        if (_isHoldingPickable && currentInteractable != null) return;

        RaycastHit hit;

        if (Physics.Raycast(_player.Head.position, _player.Head.forward, out hit, _range, _whatIsInteractable) && hit.collider.TryGetComponent(out Interactable interactable))
        {
            Debug.Log("Interactable found");

            if (currentInteractable == null)
            {
                _uiIndicator.gameObject.SetActive(true);
            }

            currentInteractable = interactable;
        }
        else if (currentInteractable != null)
        {
            if (currentInteractable != null)
            {
                _uiIndicator.gameObject.SetActive(false);
            }

            currentInteractable = null;
        }
    }
}
