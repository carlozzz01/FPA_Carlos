using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputs;

public class GameManager : MonoBehaviour, IGameActions
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
    public static Action OnGameWon;
    public static Action<bool> OnGamePaused;
    private bool _isPaused;

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

    private void Start()
    {
        AudioManager.Instance.PlayMusic("ambient");
    }

    /// <summary>
    /// Calls OnGameOver action
    /// </summary>
    public void GameOver()
    {
        OnGameOver?.Invoke();

        AudioManager.Instance.PlayMusic("death");
    }

    /// <summary>
    /// Calls OnGameOver action
    /// </summary>
    public void WinGame()
    {
        OnGameWon?.Invoke();

        AudioManager.Instance.PlayMusic("complete");

        InputController.Instance.EnableMouse(true);

        InputController.Instance.DisablePlayerInputs();
    }

    /// <summary>
    /// Reads Pause input
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPaused = !_isPaused;

            OnGamePaused?.Invoke(_isPaused);

            PauseGame(_isPaused);
        }
    }

    /// <summary>
    /// Reads Cheat input
    /// </summary>
    /// <param name="context"></param>
    public void OnCheat(InputAction.CallbackContext context)
    {
    }

    /// <summary>
    /// Pauses game
    /// </summary>
    /// <param name="isPaused"></param>
    private void PauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    /// <summary>
    /// Pause game and invokes events
    /// </summary>
    /// <param name="isPaused"></param>
    public void PauseGameWithNotify(bool isPaused)
    {
        _isPaused = isPaused;

        OnGamePaused?.Invoke(isPaused);

        PauseGame(isPaused);
    }

    public void RequestLevel(string levelName)
    {
        SceneController.Instance.LoadScene(levelName, true);
    }
}
