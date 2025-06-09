using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputs;

public class Player : MonoBehaviour, IPlayerActions
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Transform _head;
    public Rigidbody Rigidbody => _rigidbody;
    public CapsuleCollider Collider => _collider;
    public Transform Head => _head;

    [Header("Controls")]
    [SerializeField] private bool _lockCursor;
    [SerializeField] private bool _holdToCrouch;
    [SerializeField] private bool _holdToSprint;
    public bool HoldToCrouch => _holdToCrouch;
    public bool HoldToSprint => _holdToSprint;

    [Header("State")]
    public bool isGrounded;
    public PlayerState state { get; private set; }

    public Action<InputAction.CallbackContext> OnCrouchInput;
    public Action<InputAction.CallbackContext> OnInteractInput;
    public Action<InputAction.CallbackContext> OnLookInput;
    public Action<InputAction.CallbackContext> OnMoveInput;
    public Action<InputAction.CallbackContext> OnSprintInput;

    public Action<Interactable> OnInteractionStarted;

    private void OnEnable()
    {
        InspectorManager.OnInspectStarted += OnInspectStarted;
        InspectorManager.OnInspectCanceled += OnInspectCanceled;
    }

    private void OnDisable()
    {
        InspectorManager.OnInspectStarted -= OnInspectStarted;
        InspectorManager.OnInspectCanceled -= OnInspectCanceled;
    }

    private void Awake()
    {
        if (_collider == null) _collider = GetComponent<CapsuleCollider>();
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        state = PlayerState.Walk;

        Cursor.lockState = _lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_lockCursor;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (state == PlayerState.Inspect) return;

        if (state != PlayerState.Sprint)
        {
            if (context.started)
            {
                if (_holdToCrouch)
                {
                    state = PlayerState.Crouch;
                }
                else
                {
                    state = state == PlayerState.Walk ? PlayerState.Crouch : PlayerState.Walk;
                }
            }
            else if (context.canceled)
            {
                if (_holdToCrouch)
                {
                    state = PlayerState.Walk;
                }
            }
        }

        OnCrouchInput?.Invoke(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (state == PlayerState.Inspect)
        {
            if (context.started) InspectorManager.Instance.StopInspect();
        }
        else
        {
            OnInteractInput?.Invoke(context);
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (state == PlayerState.Inspect) return;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (state == PlayerState.Inspect) return;

        OnLookInput?.Invoke(context);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (state == PlayerState.Inspect) return;

        OnMoveInput?.Invoke(context);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (state == PlayerState.Inspect) return;

        if (state != PlayerState.Crouch)
        {
            if (context.started)
            {
                if (_holdToSprint)
                {
                    state = PlayerState.Sprint;
                }
                else
                {
                    state = state == PlayerState.Walk ? PlayerState.Sprint : PlayerState.Walk;
                }
            }
            else if (context.canceled)
            {
                if (_holdToSprint)
                {
                    state = PlayerState.Walk;
                }
            }
        }

        OnSprintInput?.Invoke(context);
    }

    private void OnInspectStarted()
    {
        state = PlayerState.Inspect;
    }

    private void OnInspectCanceled()
    {
        state = PlayerState.Walk;
    }
}

public enum PlayerState
{
    Walk,
    Crouch,
    Sprint,
    Inspect
}
