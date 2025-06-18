using System.Collections;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _gameOver;

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
        Debug.Log("damage flash");

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
    }
}
