using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private LevelData[] _levels;

        [Header("Scene Fade")]
        [SerializeField] private CanvasGroup _fade;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private bool _fadeOnStart;

        private static SceneController _instance;
        public static SceneController Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _fade.alpha = _fadeOnStart ? 1 : 0;
        }

        private void Start()
        {
            if (_fadeOnStart)
            {
                StartCoroutine(Fade(0f));
            }
        }

        /// <summary>
        /// Loads scene with given key
        /// </summary>
        /// <param name="sceneKey"></param>
        public void LoadScene(string sceneKey, bool fade = true)
        {
            string sceneName = "";

            sceneKey = sceneKey.Trim().ToLower();

            for (int i = 0; i < _levels.Length; i++)
            {
                LevelData level = _levels[i];

                if (level.key == sceneKey.ToLower().Trim())
                {
                    sceneName = level.sceneName;

                    break;
                }
            }

            if (sceneName != "")
            {
                if (fade)
                {
                    StartCoroutine(FadeThenLoad(sceneName));
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
            else
            {
                Debug.LogWarning($"No scene found that matches key \"{sceneKey}\"");
            }
        }

        /// <summary>
        /// Loads scene with given key
        /// </summary>
        /// <param name="sceneKey"></param>
        public void LoadScene(string sceneKey)
        {
            string sceneName = "";

            sceneKey = sceneKey.Trim().ToLower();

            foreach (LevelData level in _levels)
            {
                if (level.key == sceneKey.ToLower().Trim())
                {
                    sceneName = level.sceneName;

                    break;
                }
            }

            if (sceneName != "")
            {
                StartCoroutine(FadeThenLoad(sceneName));

            }
            else
            {
                Debug.LogWarning($"No scene found that matches key \"{sceneKey}\"");
            }
        }

        /// <summary>
        /// Loads scene with given index
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void LoadScene(int sceneIndex, bool fade = true)
        {
            if (sceneIndex < 0 || sceneIndex > _levels.Length - 1)
            {
                Debug.LogWarning("No scene found with given index");

                return;
            }

            string sceneName = _levels[sceneIndex].key;

            LoadScene(sceneName, fade);
        }

        /// <summary>
        /// Loads scene with given index
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void LoadScene(int sceneIndex)
        {
            if (sceneIndex < 0 || sceneIndex > _levels.Length - 1)
            {
                Debug.LogWarning("No scene found with given index");

                return;
            }

            string sceneName = _levels[sceneIndex].key;

            LoadScene(sceneName, true);
        }

        private IEnumerator Fade(float goalAlpha)
        {
            float timer = 0;

            float time = _fadeDuration;

            float currentAlpha = _fade.alpha;

            while (timer < time)
            {
                _fade.alpha = Mathf.Lerp(currentAlpha, goalAlpha, timer / time);

                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            _fade.alpha = goalAlpha;
        }

        private IEnumerator FadeThenLoad(string sceneName)
        {
            // start loading next scene, prevent scene activation
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // prepare fade duration variables
            float time = _fadeDuration / 2;
            float timer = 0;

            // block the screen
            _fade.blocksRaycasts = true;
            _fade.interactable = true;

            // fade to black
            while (timer < time)
            {
                _fade.alpha = Mathf.Lerp(0, 1, timer / time);

                yield return new WaitForEndOfFrame();

                timer += Time.unscaledDeltaTime;
            }

            // ensure 100% alpha
            _fade.alpha = 1;

            // wait one frame, so the screen fully turns black
            yield return new WaitForEndOfFrame();

            // wait for load to complete
            while (asyncLoad.progress < 0.9f)
            {
                Debug.Log("loading");
                Debug.Log($"progress: {asyncLoad.progress}");

                yield return null;
            }

            // once loading is complete, allow load scene
            asyncLoad.allowSceneActivation = true;

            // now that scene activation is allowed, wait for complete loading
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // wait one frame
            yield return new WaitForEndOfFrame();

            // turn time scale to 1, run game normally
            Time.timeScale = 1;

            TranslationManager.Instance.InvokeOnTextLoaded();

            // redo fade, this time to clear the screen
            timer = 0;

            while (timer < time)
            {
                _fade.alpha = Mathf.Lerp(1, 0, timer / time);

                yield return new WaitForEndOfFrame();

                timer += Time.unscaledDeltaTime;
            }

            // ensure 0% alpha
            _fade.alpha = 0;

            // wait one frame, so the screen fully turns clear
            yield return new WaitForEndOfFrame();
        }
    }
}

[Serializable]
public struct LevelData
{
    public string key;

    public string sceneName;
}
