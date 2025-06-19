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
        [Header("Game Over")]
        [SerializeField] private CanvasGroup _gameOver;
        [SerializeField] private float _gameOverFadeDuration;
        [SerializeField] private Button _restartButton;

        [Header("Damage Flash")]
        [SerializeField] private float _damageFlashDurationIn;
        [SerializeField] private float _damageFlashDurationOut;
        [SerializeField] private CanvasGroup _damageFlash;

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

        public void TriggerDamageFlash()
        {
            StartCoroutine(DamageFlash());
        }

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

        private void ShowGameOver(bool value)
        {
            StartCoroutine(DisplayGameOver(value));
        }

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

            if (value)
            {
                EventSystem.current.SetSelectedGameObject(_restartButton.gameObject);
                InputController.Instance.EnableMouse(true);
            }
        }
    }

}