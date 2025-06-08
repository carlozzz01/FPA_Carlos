using UnityEngine;
using UnityEngine.Events;

public class Interactable_Rigidbody : Interactable
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _model;

    [Header("Configuration")]
    [SerializeField] private float _maxFollowDelta;
    [SerializeField] private bool _isBreakable;
    [HideInInspector][SerializeField] private float _breakVelocity;
    [HideInInspector][SerializeField] private ParticleSystem _breakEffect;

    private Transform _holdPosition;
    // private float _initialDamping;

    public bool IsBreakable => _isBreakable;
    public float BreakVelocity => _breakVelocity;
    public ParticleSystem BreakEffect => _breakEffect;
    [HideInInspector] public UnityEvent OnBreak;

    public override void Awake()
    {
        base.Awake();

        // _initialDamping = _rigidbody.linearDamping;
    }

    private void Update()
    {
        if (_holdPosition == null) return;

        _rigidbody.WakeUp();

        _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _holdPosition.position, _maxFollowDelta);
    }

    private void OnCollisionEnter(Collision collision)
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
                
                _model.gameObject.SetActive(false);

                _breakEffect.Play();

                OnBreak?.Invoke();
            }
        }
    }

    public override void Interact(PlayerInteraction player)
    {
        if (player.IsHoldingPickable && player.currentInteractable == this)
        {
            _holdPosition = null;

            _rigidbody.useGravity = true;

            _rigidbody.WakeUp();

            // _rigidbody.linearDamping = _initialDamping;

            _rigidbody.linearVelocity = Vector3.zero;
        }
        else
        {
            _holdPosition = player.PickableHolder;

            // to wake rigidbody up
            _rigidbody.WakeUp();

            _rigidbody.useGravity = false;

            // _rigidbody.linearDamping = 10;

            _rigidbody.linearVelocity = Vector3.zero;
        }
    }

    public void SetBreakVelocity(float velocity)
    {
        _breakVelocity = velocity;
    }

    public void SetBreakEffect(ParticleSystem effect)
    {
        _breakEffect = effect;
    }

    public void PullPoolItem(string poolID)
    {
        PoolManager.Instance.Pull(poolID, transform.position, transform.rotation);
    }
}
