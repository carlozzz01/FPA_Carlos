using System;
using System.Collections;
using UnityEngine;

public class HintsManager : MonoBehaviour
{
    public static HintsManager Instance { get; private set; }
    [SerializeField] private float _hintDuration = 3f;

    private Coroutine _waitCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowHints(Hint[] hints)
    {
        foreach (var hint in hints)
        {
            if (!hint.decision.CheckDecision())
            {
                foreach (var hintedObject in hint.hintedObjects)
                {
                    if (hintedObject != null) hintedObject.Activate();
                }
            }
        }

        if (_waitCoroutine != null) StopCoroutine(_waitCoroutine);
        _waitCoroutine = StartCoroutine(WaitForDeactivation());
    }

    private IEnumerator WaitForDeactivation()
    {
        yield return new WaitForSeconds(_hintDuration);
        OutlineHintManager.Instance.DeactivateAll();
    }
}

[Serializable]
public struct Hint
{
    public ReactionDecision decision;
    public HintedObject[] hintedObjects;
}