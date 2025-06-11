using UnityEngine;

// [CreateAssetMenu(fileName = "New Decision", menuName = "State Machine/Decision")]
public abstract class Decision : ScriptableObject
{
    public abstract bool Decide(StateMachineController controller);
}
