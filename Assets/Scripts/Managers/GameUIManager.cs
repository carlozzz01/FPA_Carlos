using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [Header("Pause")]
        [SerializeField] private CanvasGroup _pause;

        [Header("Game Won")]
        [SerializeField] private CanvasGroup _victory;

        [SerializeField] private float _gameWonFadeDuration;
        [SerializeField] private Button _replayButton;

        [Header("Game Over")]
        [SerializeField] private CanvasGroup _gameOver;
        [SerializeField] private float _gameOverFadeDuration;
        [SerializeField] private Button _restartButton;

        [Header("Damage Flash")]
        [SerializeField] private float _damageFlashDurationIn;
        [SerializeField] private float _damageFlashDurationOut;
        [SerializeField] private CanvasGroup _damageFlash;
        [SerializeField] private GameAudio _damageSound;

        private static GameUIManager _instance;
        public static GameUIManager Instance => _instance;

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

        private void OnEnable()
        {
            GameManager.OnGamePaused += EnablePauseMenu;
            GameManager.OnGameWon += () => ShowGameWon(true);
        }

        private void OnDisable()
        {
            GameManager.OnGamePaused -= EnablePauseMenu;
        }

        /// <summary>
        /// Starts damage flash
        /// </summary>
        public void TriggerDamageFlash()
        {
            StartCoroutine(DamageFlash());
        }

        /// <summary>
        /// Smoothly flashes Damage Image, and shows Game Over at the end
        /// </summary>
        /// <returns></returns>
        private IEnumerator DamageFlash()
        {
            _damageFlash.alpha = 0;

            float timer = 0;

            while (timer < _damageFlashDurationIn)
            {
                float t = timer / _damageFlashDurationIn;

                _damageFlash.alpha = Mathf.Lerp(0, 1, t);

                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            _damageFlash.alpha = 1;

            AudioSource.PlayClipAtPoint(_damageSound.clip, GameManager.Instance.Viking.transform.position, _damageSound.volume * AudioManager.Instance.SFXVolume);

            timer = 0;

            while (timer < _damageFlashDurationOut)
            {
                float t = timer / _damageFlashDurationOut;

                _damageFlash.alpha = Mathf.Lerp(1, 0, t);

                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            _damageFlash.alpha = 0;

            ShowGameOver(true);
        }

        /// <summary>
        /// Displays/Hides Game Over screen
        /// </summary>
        /// <param name="value"></param>
        private void ShowGameOver(bool value)
        {
            StartCoroutine(DisplayGameOver(value));
        }

        /// <summary>
        /// Smoothly fades Game Over screen
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private IEnumerator DisplayGameOver(bool value)
        {
            float timer = 0;

            float goalAlpha = value ? 1 : 0;
            float startAlpha = value ? 0 : 1;

            float t = 0;

            while (timer < _gameOverFadeDuration)
            {
                t = timer / _gameOverFadeDuration;

                _gameOver.alpha = Mathf.Lerp(startAlpha, goalAlpha, t);

                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            _gameOver.alpha = goalAlpha;
            _gameOver.interactable = value;
            _gameOver.blocksRaycasts = value;

            if (value)
            {
                EventSystem.current.SetSelectedGameObject(_restartButton.gameObject);
                InputController.Instance.EnableMouse(true);
            }
        }

        /// <summary>
        /// Displays/Hides Game Over screen
        /// </summary>
        /// <param name="value"></param>
        private void ShowGameWon(bool value)
        {
            StartCoroutine(DisplayGameWon(value));
        }

        /// <summary>
        /// Smoothly fades Game Over screen
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private IEnumerator DisplayGameWon(bool value)
        {
            float timer = 0;

            float goalAlpha = value ? 1 : 0;
            float startAlpha = value ? 0 : 1;

            float t = 0;

            while (timer < _gameOverFadeDuration)
            {
                t = timer / _gameOverFadeDuration;

                _victory.alpha = Mathf.Lerp(startAlpha, goalAlpha, t);

                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            _victory.alpha = goalAlpha;
            _victory.interactable = value;
            _victory.blocksRaycasts = value;
        }

        /// <summary>
        /// Activates pause menu depending on given bool
        /// </summary>
        /// <param name="state"></param>
        private void EnablePauseMenu(bool state)
        {
            _pause.alpha = state ? 1 : 0f;
            _pause.blocksRaycasts = state;
            _pause.interactable = state;
            InputController.Instance.EnableMouse(state);
        }
    }

}