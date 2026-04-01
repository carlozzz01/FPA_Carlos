using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player _player;
    [SerializeField] private Image _uiIndicator;
    [SerializeField] private Image _launchIndicator;
    [SerializeField] private Transform _rigidbodyHolder;

    [Header("Configuration")]
    [SerializeField] private LayerMask _whatIsInteractable;
    [SerializeField] private float _range;
    [SerializeField] public Interactable currentInteractable { get; private set; }
    [SerializeField] private GameAudio _throwSound;

    [Header("Physics")]
    [SerializeField] private bool _isHoldingRigidboy;
    [SerializeField] private float _maxLaunchForce = 5f;
    [SerializeField] private float _launchForceGain = 0.3f;
    [SerializeField] private float _minimumForceForLaunch = 0.5f;
    [SerializeField] private bool _isChargingLaunch;
    [SerializeField] private float _launchForce;

    public Transform RigidbodyHolder => _rigidbodyHolder;
    public bool IsHoldingPickable => _isHoldingRigidboy;

    public static Action<ContextMessageSO> OnContextGiven;
    public static Action OnContextLost;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(_player.Head.position, _player.Head.forward * _range);
    }

    private void OnEnable()
    {
        _player.OnInteractInput += OnInteract;
        _player.OnShootInput += OnShoot;
    }

    private void OnDisable()
    {
        _player.OnInteractInput -= OnInteract;
        _player.OnShootInput -= OnShoot;
    }

    private void FixedUpdate()
    {
        CheckForInteractable();
        if (_isChargingLaunch) ChargeLaunch();
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

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_isHoldingRigidboy) StartChargingLaunch();
        }
        else if (context.canceled)
        {
            if (_isHoldingRigidboy && _launchForce > _minimumForceForLaunch)
            {
                Launch();
            }

            _isChargingLaunch = false;
            _launchForce = 0;
        }
    }

    private void Launch()
    {
        currentInteractable.Interact(this);

        AudioSource.PlayClipAtPoint(_throwSound.clip, transform.position, _throwSound.volume * AudioManager.Instance.SFXVolume);

        Interactable_Rigidbody interactable = currentInteractable as Interactable_Rigidbody;

        interactable.Rigidbody.AddForce(_rigidbodyHolder.forward.normalized * _launchForce, ForceMode.Impulse);

        currentInteractable = null;

        _isHoldingRigidboy = false;

        _launchIndicator.fillAmount = 0;
    }

    private void ChargeLaunch()
    {
        _launchForce += Time.deltaTime * _launchForceGain;

        _launchForce = Mathf.Min(_launchForce, _maxLaunchForce);

        _launchIndicator.fillAmount = _launchForce / _maxLaunchForce;
    }

    private void Interact()
    {
        _player.OnInteractionStarted.Invoke(currentInteractable);

        currentInteractable.Interact(this);

        if (currentInteractable is Interactable_Rigidbody)
        {
            if (_isHoldingRigidboy)
            {
                currentInteractable = null;
                _isHoldingRigidboy = false;
            }
            else
            {
                _isHoldingRigidboy = true;
                OnContextLost?.Invoke();
            }
        }
    }

    private void CheckForInteractable()
    {
        if (_isHoldingRigidboy && currentInteractable != null) return;

        RaycastHit hit;

        bool interactableHit = Physics.Raycast(_player.Head.position, _player.Head.forward, out hit, _range, _whatIsInteractable, QueryTriggerInteraction.Ignore);

        if (interactableHit && hit.collider.TryGetComponent(out Interactable interactable))
        {
            if (currentInteractable == null)
            {
                _uiIndicator.gameObject.SetActive(true);
            }

            currentInteractable = interactable;

            if (currentInteractable.ContextMessage != null) OnContextGiven?.Invoke(currentInteractable.ContextMessage);
        }
        else
        {
            if (currentInteractable != null)
            {
                _uiIndicator.gameObject.SetActive(false);

                OnContextLost?.Invoke();
            }

            currentInteractable = null;
        }
    }

    private void StartChargingLaunch()
    {
        _isChargingLaunch = true;
    }
}
