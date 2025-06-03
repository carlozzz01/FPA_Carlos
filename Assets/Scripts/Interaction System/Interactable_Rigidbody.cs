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
    private float _initialDamping;

    public bool IsBreakable => _isBreakable;
    public float BreakVelocity => _breakVelocity;
    public ParticleSystem BreakEffect => _breakEffect;
    [HideInInspector] public UnityEvent breakEvent;

    public override void Awake()
    {
        base.Awake();

        _initialDamping = _rigidbody.linearDamping;
    }

    private void Update()
    {
        if (_holdPosition == null) return;

        _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _holdPosition.position, _maxFollowDelta);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(_rigidbody.linearVelocity.y);

        if (_isBreakable && Mathf.Abs(_rigidbody.linearVelocity.y) >= _breakVelocity)
        {
            _model.gameObject.SetActive(false);
            _breakEffect.Play();
        }
    }

    public override void Interact(PlayerInteraction player)
    {
        if (player.IsHoldingPickable && player.currentInteractable == this)
        {
            _holdPosition = null;

            _rigidbody.useGravity = true;

            _rigidbody.linearDamping = _initialDamping;
        }
        else
        {
            _holdPosition = player.PickableHolder;

            // to wake rigidbody up
            _rigidbody.WakeUp();

            _rigidbody.useGravity = false;

            _rigidbody.linearDamping = 10;
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
}
