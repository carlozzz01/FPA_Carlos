using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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
            Destroy(this);
        }
    }

    /// <summary>
    /// Loads scene with given name
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        DataManager.Instance.SaveGameData();
        DataManager.Instance.SaveSettingsPrefs();

        Application.Quit();
    }
}
