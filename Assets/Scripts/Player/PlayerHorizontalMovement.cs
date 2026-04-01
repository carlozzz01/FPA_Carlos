using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontalMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player _player;
    [SerializeField] private AudioSource _audioSource;

    [Header("Speeds")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _sprintSpeed;

    [Header("Footsteps")]
    [SerializeField] private GameAudio[] _footstepSounds;
    [SerializeField] private float _walkFootstepDistance = 1.5f;
    [SerializeField] private float _crouchFootstepDistance = 2f;
    [SerializeField] private float _sprintFootstepDistance = 1f;

    private float _footstepDistance;

    private Vector3 _moveInput;
    private float _speed;

    private Vector3 _lastFootstepPosition;
    private int _lastFootstepIndex = -1;

    private void Awake()
    {
        if (_player == null) _player = GetComponent<Player>();
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();

        _lastFootstepPosition = transform.position;
    }

    private void OnEnable()
    {
        _player.OnMoveInput += OnMove;
        _player.OnCrouchInput += (context) => UpdateSpeed();
        _player.OnSprintInput += (context) => UpdateSpeed();
    }

    private void OnDisable()
    {
        _player.OnMoveInput -= OnMove;
    }

    private void Start()
    {
        UpdateSpeed();
    }

    private void FixedUpdate()
    {
        Move();
        CheckFootstep();
    }

    /// <summary>
    /// Reads Move input from Player
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        _moveInput.Set(input.x, 0f, input.y);
    }

    /// <summary>
    /// Moves the Player if it's grounded.
    /// </summary>
    private void Move()
    {
        if (_player.isGrounded)
        {
            // Quaternion * Vector3 essentially rotates the Vector3 towards the Quaternion direction
            Vector3 moveDirection = _player.transform.rotation * _moveInput;

            Vector3 currentHorizontalVelocity = _player.Rigidbody.linearVelocity;
            currentHorizontalVelocity.y = 0f;

            Vector3 targetHorizontalVelocity = _speed * moveDirection;

            _player.Rigidbody.AddForce(targetHorizontalVelocity - currentHorizontalVelocity, ForceMode.VelocityChange);
        }
    }

    private void CheckFootstep()
    {
        if (_footstepSounds.Length == 0) return;
        if (!_player.isGrounded) return;

        Vector3 currentPosition = transform.position;
        Vector3 horizontalDelta = currentPosition - _lastFootstepPosition;
        horizontalDelta.y = 0f;

        if (horizontalDelta.magnitude >= _footstepDistance)
        {
            PlayFootstep();
            _lastFootstepPosition = currentPosition;
        }
    }

    private void PlayFootstep()
    {
        // Evita repetir el mismo sonido dos veces seguidas
        int index;
        do
        {
            index = UnityEngine.Random.Range(0, _footstepSounds.Length);
        }
        while (_footstepSounds.Length > 1 && index == _lastFootstepIndex);

        _lastFootstepIndex = index;

        GameAudio audio = _footstepSounds[index];
        _audioSource.PlayOneShot(audio.clip, audio.volume * AudioManager.Instance.SFXVolume);
    }

    /// <summary>
    /// Updates the Player's speed depending on it's current state: Walk, Crouch, Sprint
    /// </summary>
    private void UpdateSpeed()
    {
        switch (_player.state)
        {
            case PlayerState.Walk:
                _speed = _walkSpeed;
                _footstepDistance = _walkFootstepDistance;
                break;
            case PlayerState.Crouch:
                _speed = _crouchSpeed;
                _footstepDistance = _crouchFootstepDistance;
                break;
            case PlayerState.Sprint:
                _speed = _sprintSpeed;
                _footstepDistance = _sprintFootstepDistance;
                break;
            default:
                _speed = 0f;
                _footstepDistance = _walkFootstepDistance;
                break;
        }
    }
}
