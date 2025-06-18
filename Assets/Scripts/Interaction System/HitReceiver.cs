using UnityEngine;
using UnityEngine.Events;

public class HitReceiver : MonoBehaviour
{
    public UnityEvent OnHitReceived;
    [SerializeField] private string _tagToCheck;
    [SerializeField] private LayerMask _layerMask;

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag(_tagToCheck) || string.IsNullOrEmpty(_tagToCheck)) && ((_layerMask & (1 << collision.gameObject.layer)) != 0))
        {
            OnHitReceived?.Invoke();
        }
    }
}
