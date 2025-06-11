using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private State _currentState;
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private List<Transform> _waypoints;
    private int _nextWaypointIndex;
    private Transform _NextWaypoint => _waypoints[_nextWaypointIndex];
    public Vector3 Velocity => _navMeshAgent.velocity;

    [Header("Configuration")]
    [SerializeField] private bool _aiActive = true;
    [SerializeField] private EnemyStats _stats;
    [SerializeField] private Transform _eyes;
    private Transform _target;
    private Vector3 _lastSpottedTargetPosition;
    private List<Vector3> _heardSounds;
    private Vector3 _currentSoundPosition;
    private float _stateTimer = 0f;
    private bool _isStateTimerRunning = false;

    public Transform Eyes => _eyes;
    public EnemyStats Stats => _stats;
    public Transform Target => _target;
    public List<Vector3> HeardSounds => _heardSounds;
    public float StateTimer => _stateTimer;
    public Vector3 Destination => _navMeshAgent.destination;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_lastSpottedTargetPosition, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_currentSoundPosition, 0.5f);
    }

    private void Awake()
    {
        _heardSounds = new List<Vector3>();
    }

    private void Start()
    {
        _currentState.StartState(this);
    }

    private void Update()
    {
        if (!_aiActive) return;

        _currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != _currentState)
        {
            _currentState.EndState(this);
            _currentState = nextState;
            _currentState.StartState(this);

            _stateTimer = 0f;
            _isStateTimerRunning = true;
            Debug.Log($"transitioning to state {_currentState.name}");
        }
    }

    public void SetDestination(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);

        _navMeshAgent.isStopped = false;
    }

    public void StopMoving()
    {
        _navMeshAgent.isStopped = true;
    }

    public void GoToNextWaypoint()
    {
        SetDestination(_NextWaypoint.position);
    }

    public void GoToRandomWaypoint()
    {
        SetDestination(_waypoints[Random.Range(0, _waypoints.Count)].position);
    }

    public void IncreaseNextWaypointIndex()
    {
        _nextWaypointIndex = (_nextWaypointIndex + 1) % _waypoints.Count;
    }

    public void FeedFloatToAnimator(string parameterName, float parameterValue)
    {
        _animator.SetFloat(parameterName, parameterValue);
    }

    public void FeedTriggerToAnimator(string parameterName)
    {
        _animator.SetTrigger(parameterName);
    }

    public bool IsCloseToDestination()
    {
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending;
    }

    public bool IsCloseToPlayer()
    {
        return _target != null && _target.CompareTag("Player") && _navMeshAgent.remainingDistance <= _stats.MinAttackRange;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _lastSpottedTargetPosition = target != null ? target.position : _lastSpottedTargetPosition;
    }

    public void GoToLastTargetPosition()
    {
        SetDestination(_lastSpottedTargetPosition);
    }

    public void GoToLastSoundPosition()
    {
        SetDestination(_currentSoundPosition);
    }

    public void Chase()
    {
        _navMeshAgent.speed = _stats.ChaseSpeed;
    }

    public void Patrol()
    {
        GoToNextWaypoint();

        _navMeshAgent.speed = _stats.PatrolSpeed;
    }

    public void FocusOnLastSound()
    {
        _currentSoundPosition = _heardSounds[_heardSounds.Count - 1];
        _heardSounds.Clear();

        // SetDestination(_currentSoundPosition);
        // _navMeshAgent.speed = _stats.ChaseSpeed;
    }

    public void HearSound(Vector3 position)
    {
        _heardSounds.Add(position);
    }

    public bool IsStateTimerRunning()
    {
        return _isStateTimerRunning;
    }

    internal void StartStateTimer(float duration)
    {
        _isStateTimerRunning = true;
        _stateTimer = duration;
    }

    public void DecreaseStateTimer()
    {
        _stateTimer -= Time.deltaTime;
    }
}
