using UnityEngine;

[System.Serializable]
public class Transition
{
    [SerializeField] private Decision _decision;
    [SerializeField] private State _stateOnTrue;
    [SerializeField] private State _stateOnFalse;

    public Decision Decision => _decision;
    public State StateOnTrue => _stateOnTrue;
    public State StateOnFalse => _stateOnFalse;
}
