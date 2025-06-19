using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineController : MonoBehaviour
{
    [SerializeField] private State _currentState;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _eyes;
    private Transform _NextWaypoint => _waypoints[_nextWaypointIndex];
    public Vector3 Velocity => _navMeshAgent.velocity;

    [Header("Configuration")]
    [SerializeField] private bool _aiActive = true;
    [SerializeField] private EnemyStats _stats;
    [SerializeField] private List<Transform> _waypoints;

    [Header("Debug")]
    [SerializeField] private Transform _target;
    [SerializeField] private int _nextWaypointIndex;
    [SerializeField] private float _stateTimer = 0f;
    [SerializeField] private bool _isStateTimerRunning = false;
    private Vector3 _suspicionPoint;
    private List<Vector3> _heardSounds;
    private Vector3 _currentSoundPosition;

    public Transform Eyes => _eyes;
    public EnemyStats Stats => _stats;
    public Transform Target => _target;
    public List<Vector3> HeardSounds => _heardSounds;
    public float StateTimer => _stateTimer;
    public Vector3 Destination => _navMeshAgent.destination;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_suspicionPoint, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_currentSoundPosition, 0.5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _stats.Reach);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _stats.HearRange);
    }

    private void Awake()
    {
        _heardSounds = new List<Vector3>();
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
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

    /// <summary>
    /// AI transitions to given state
    /// </summary>
    /// <param name="nextState"></param>
    public void TransitionToState(State nextState)
    {
        if (nextState != _currentState)
        {
            _currentState.EndState(this);

            _stateTimer = 0f;
            _isStateTimerRunning = false;

            Debug.Log($"transitioning to state {nextState.name}");

            _currentState = nextState;

            _currentState.StartState(this);
        }
    }

    /// <summary>
    /// Sets given position as destination for the AI
    /// </summary>
    /// <param name="position"></param>
    public void SetDestination(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);

        _navMeshAgent.isStopped = false;
    }

    /// <summary>
    /// AI stops moving
    /// </summary>
    public void StopMoving()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.SetDestination(transform.position);
    }

    /// <summary>
    /// AI starts moving
    /// </summary>
    public void StartMoving()
    {
        _navMeshAgent.isStopped = false;
    }

    /// <summary>
    /// Sets next waypoint as destination
    /// </summary>
    public void GoToNextWaypoint()
    {
        SetDestination(_NextWaypoint.position);
    }

    /// <summary>
    /// Selects random waypoint
    /// </summary>
    public void GoToRandomWaypoint()
    {
        SetDestination(_waypoints[Random.Range(0, _waypoints.Count)].position);
    }

    /// <summary>
    /// Handles correct waypoint navigation
    /// </summary>
    public void IncreaseNextWaypointIndex()
    {
        _nextWaypointIndex = (_nextWaypointIndex + 1) % _waypoints.Count;
    }

    /// <summary>
    /// Feeds given float with given name to animator 
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="parameterValue"></param>
    public void FeedFloatToAnimator(string parameterName, float parameterValue)
    {
        _animator.SetFloat(parameterName, parameterValue);
    }

    /// <summary>
    /// Feeds trigger with given name to animation 
    /// </summary>
    /// <param name="parameterName"></param>
    public void FeedTriggerToAnimator(string parameterName)
    {
        _animator.SetTrigger(parameterName);
    }

    /// <summary>
    /// Returns wether the AI is close to its destination
    /// </summary>
    /// <returns></returns>
    public bool IsCloseToDestination()
    {
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending;
    }

    /// <summary>
    /// Returns wether the AI is close to the Player
    /// </summary>
    /// <returns></returns>
    public bool IsCloseToPlayer()
    {
        return _target != null && _target.CompareTag("Player") && _navMeshAgent.remainingDistance <= _stats.MinAttackRange;
    }

    /// <summary>
    /// Sets the AI's target
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        _target = target;
        _suspicionPoint = target != null ? target.position : _suspicionPoint;
    }

    /// <summary>
    /// Sets the position for the suspicion points
    /// </summary>
    /// <param name="position"></param>
    public void SetSuspicion(Vector3 position)
    {
        _suspicionPoint = position;
    }

    /// <summary>
    /// Sets suspicion point as destination
    /// </summary>
    public void GoToSuspicionPoint()
    {
        _navMeshAgent.speed = _stats.PatrolSpeed;
        SetDestination(_suspicionPoint);
    }

    /// <summary>
    /// Sets last sound position as destination
    /// </summary>
    public void GoToLastSoundPosition()
    {
        SetDestination(_currentSoundPosition);
    }

    /// <summary>
    /// Sets target position as destination and moves at chase speed
    /// </summary>
    public void Chase()
    {
        SetDestination(_target.position);

        _navMeshAgent.speed = _stats.ChaseSpeed;
    }

    /// <summary>
    /// Sets next waypoint as destination, and moves at patrol speed
    /// </summary>
    public void Patrol()
    {
        GoToNextWaypoint();

        _navMeshAgent.speed = _stats.PatrolSpeed;
    }

    /// <summary>
    /// Takes last sound as main focus
    /// </summary>
    public void FocusOnLastSound()
    {
        _currentSoundPosition = _heardSounds[_heardSounds.Count - 1];
        _heardSounds.Clear();

        // SetDestination(_currentSoundPosition);
        // _navMeshAgent.speed = _stats.ChaseSpeed;
    }

    /// <summary>
    /// Adds position to list of heard sounds
    /// </summary>
    /// <param name="position"></param>
    public void HearSound(Vector3 position)
    {
        _heardSounds.Add(position);
    }

    /// <summary>
    /// Returns wether the state timer is running
    /// </summary>
    /// <returns></returns>
    public bool IsStateTimerRunning()
    {
        return _isStateTimerRunning;
    }

    /// <summary>
    /// Starts state timer with given duration
    /// </summary>
    /// <param name="duration"></param>
    public void StartStateTimer(float duration)
    {
        _isStateTimerRunning = true;
        _stateTimer = duration;
    }

    /// <summary>
    /// Decreases state timer by Time.deltaTime
    /// </summary>
    public void DecreaseStateTimer()
    {
        _stateTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Stops the state timer
    /// </summary>
    public void StopStateTimer()
    {
        _stateTimer = -1;
        _isStateTimerRunning = false;
    }

    /// <summary>
    /// Method that reacts to the GameManager.OnGameOver action
    /// </summary>
    private void OnGameOver()
    {
        StartCoroutine(RotateTowardsPlayer());
    }

    /// <summary>
    /// Coroutine that rotates enemy towards player
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateTowardsPlayer()
    {
        _navMeshAgent.isStopped = true;

        Vector3 playerDir = Vector3.Normalize(GameManager.Instance.Player.position - transform.position);

        Quaternion playerDirRotation = Quaternion.LookRotation(playerDir, Vector3.up);

        transform.position = GameManager.Instance.Player.position - (playerDir.normalized * _stats.MinAttackRange * 2);

        float timer = 0;

        float time = GameManager.Instance.PlayerTurnDuration;

        Quaternion initialRotation = transform.rotation;

        while (timer < time)
        {
            playerDir = Vector3.Normalize(GameManager.Instance.Player.position - transform.position);

            playerDirRotation = Quaternion.LookRotation(playerDir, Vector3.up);

            transform.rotation = Quaternion.Slerp(initialRotation, playerDirRotation, timer / time);

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = GameManager.Instance.Player.position - (playerDir.normalized * _stats.MinAttackRange * 2);

        transform.rotation = playerDirRotation;
    }

    /// <summary>
    /// Causes AI to get stunned, pausing por a couple seconds
    /// </summary>
    public void GetStunned(State state)
    {
        TransitionToState(state);
    }

    /// <summary>
    /// Sets the AI's navigation speed
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        _navMeshAgent.speed = speed;
    }
}
