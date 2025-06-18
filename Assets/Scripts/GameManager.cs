using System;
using Managers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game References")]
    [SerializeField] private Player _player;
    [SerializeField] private InputController _inputs;
    [SerializeField] private StateMachineController _vikingNPC;

    [Header("Game Over Sequence")]
    [SerializeField] private float _playerTurnDuration = 0.6f;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public Transform Player => _player.transform;
    public StateMachineController Viking => _vikingNPC;
    public float PlayerTurnDuration => _playerTurnDuration;

    public static Action OnGameOver;
    public static Action<bool> OnGamePaused;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
