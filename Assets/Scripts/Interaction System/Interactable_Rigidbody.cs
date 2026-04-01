using Managers;
using UnityEngine;
using UnityEngine.Events;

public class Interactable_Rigidbody : Interactable
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _model;

    [Header("Configuration")]
    [SerializeField] private float _maxFollowDelta;
    [SerializeField] private float _turningRate = 30f;
    [SerializeField] private bool _isBreakable;
    [HideInInspector][SerializeField] private float _breakVelocity;
    [HideInInspector][SerializeField] private ParticleSystem _breakEffect;
    [SerializeField] private GameAudio _pickupSound;

    private Transform _holdPosition;
    // private float _initialDamping;

    public bool IsBreakable => _isBreakable;
    public float BreakVelocity => _breakVelocity;
    public ParticleSystem BreakEffect => _breakEffect;
    [HideInInspector] public UnityEvent OnBreak;
    public Rigidbody Rigidbody => _rigidbody;

    public override void Awake()
    {
        base.Awake();

        // _initialDamping = _rigidbody.linearDamping;
    }

    private void FixedUpdate()
    {
        if (_holdPosition == null) return;

        _rigidbody.WakeUp();

        _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, _holdPosition.position, _maxFollowDelta * Time.fixedDeltaTime));
        _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.up), _turningRate * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_isBreakable && !collision.transform.CompareTag("Player"))
        {
            // Checks collision on all axis, sets boolean to true if velocity is surpasses limit velocity
            bool broke = false;

            if (Mathf.Abs(_rigidbody.linearVelocity.y) >= _breakVelocity)
            {
                broke = true;
            }
            else if (Mathf.Abs(_rigidbody.linearVelocity.x) >= _breakVelocity)
            {
                broke = true;
            }
            else if (Mathf.Abs(_rigidbody.linearVelocity.z) >= _breakVelocity)
            {
                broke = true;
            }

            // If the limit is surpassed, 
            if (broke)
            {
                // Deactivate the model, play vfx, and call break event
                _breakEffect.Play();

                OnBreak?.Invoke();

                var collider = GetComponent<Collider>();

                collider.enabled = false;

                _model.gameObject.SetActive(false);

                Destroy(gameObject, 1f);
            }
        }
    }

    public override void Interact(PlayerInteraction player)
    {
        if (player.IsHoldingPickable && player.currentInteractable == this)
        {
            Drop();
        }
        else
        {
            PickUp(player);
        }
    }

    private void PickUp(PlayerInteraction player)
    {
        _holdPosition = player.RigidbodyHolder;

        // to wake rigidbody up
        _rigidbody.WakeUp();

        _rigidbody.useGravity = false;

        _rigidbody.linearVelocity = Vector3.zero;

        AudioSource.PlayClipAtPoint(_pickupSound.clip, transform.position, _pickupSound.volume * AudioManager.Instance.SFXVolume);
    }

    public void Drop()
    {
        _holdPosition = null;

        _rigidbody.useGravity = true;

        _rigidbody.WakeUp();

        _rigidbody.linearVelocity = Vector3.zero;
    }

    public void SetBreakVelocity(float velocity)
    {
        _breakVelocity = velocity;
    }

    public void SetBreakEffect(ParticleSystem effect)
    {
        _breakEffect = effect;
    }

    public void RotateUpwards()
    {
        _rigidbody.rotation = Quaternion.Euler(Vector3.up);
    }
}
