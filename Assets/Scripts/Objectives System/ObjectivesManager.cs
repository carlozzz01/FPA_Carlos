using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance { get; private set; }
    public static Action OnConditionChanged;

    [SerializeField] private ObjectivesUIManager _objectivesUI;
    [SerializeField] private List<ObjectiveSO> _activeObjectives = new();
    [SerializeField] private ObjectiveHintBinding[] _hintBindings;
    private bool _isActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Debug.LogError($"Duplicate {this}", gameObject); return; }
    }

    private void OnEnable()
    {
        OnConditionChanged += CheckObjectives;
    }

    private void OnDisable()
    {
        OnConditionChanged -= CheckObjectives;
    }

    private void Update()
    {
        if (Time.frameCount % 30 == 0) CheckObjectives();
    }

    public void Initialize()
    {
        _isActive = true;
        _objectivesUI.Initialize();
    }

    public void Toggle()
    {
        if (!_isActive) return;
        _objectivesUI.Toggle();
    }

    public void AddObjective(ObjectiveSO objective)
    {
        if (_activeObjectives.Contains(objective)) return;

        _activeObjectives.Add(objective);
        _objectivesUI.AddObjective(objective);
    }

    public Hint[] GetActiveHints()
    {
        var lastActiveObjective = _hintBindings.LastOrDefault(b => _activeObjectives.Contains(b.objective) && !b.objective.isCompleted);

        return lastActiveObjective.hints ?? Array.Empty<Hint>();
    }

    private void CheckObjectives()
    {
        foreach (var objective in _activeObjectives)
        {
            if (!objective.isCompleted && objective.CheckCompletion())
            {
                CompleteObjective(objective);

            }
        }
    }

    private void CompleteObjective(ObjectiveSO objective)
    {
        objective.SetCompleted(true);
        _objectivesUI.CompleteObjective(objective);
    }
}

[Serializable]
public struct ObjectiveHintBinding
{
    public ObjectiveSO objective;
    public Hint[] hints;
}