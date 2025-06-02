using UnityEngine;

public class Interactable_Rigidbody : Interactable
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Configuration")]
    [SerializeField] private float _maxFollowDelta;
    [SerializeField] private bool _breakable;
    [SerializeField] private float _breakVelocity;

    private Transform _holdPosition;
    private float _initialDamping;

    public override void Awake()
    {
        base.Awake();

        _initialDamping = _rigidbody.linearDamping;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(_rigidbody.linearVelocity.y);
        
        if (_breakable && Mathf.Abs(_rigidbody.linearVelocity.y) >= _breakVelocity)
        {
            gameObject.SetActive(false);
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

            _rigidbody.useGravity = false;

            _rigidbody.linearDamping = 10;
        }
    }

    private void Update()
    {
        if (_holdPosition == null) return;

        _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _holdPosition.position, _maxFollowDelta);
    }
}
