using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "State Machine/Stats/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Configuration")]
    [SerializeField] private float _patrolSpeed = 0.5f;
    [SerializeField] private float _chaseSpeed = 2f;
    [SerializeField] private float _reach = 50f;
    [SerializeField] private float _timeToDisengage = 20f;
    [SerializeField] private float _minAttackRange = 2f;
    [SerializeField] private float _lookSphereCastRadius = 0.8f;
    [SerializeField] private float _fieldOfView = 90;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private float _hearRange = 5f;

    public float PatrolSpeed => _patrolSpeed;
    public float ChaseSpeed => _chaseSpeed;
    public float Reach => _reach;
    public float MinAttackRange => _minAttackRange;
    public float LookSphereCastRadius => _lookSphereCastRadius;
    public float FieldOfView => _fieldOfView;
    public LayerMask TargetLayers => _targetLayers;
    public float HearRange => _hearRange;
}
