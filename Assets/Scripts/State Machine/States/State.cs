using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "State Machine/States/New State")]
public class State : ScriptableObject
{
    [SerializeField] private StateAction[] _startActions;
    [SerializeField] private StateAction[] _actions;
    [SerializeField] private StateAction[] _endActions;
    [SerializeField] private Transition[] _transitions;

    private void DoActions(StateMachineController controller, StateAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    public void StartState(StateMachineController controller)
    {
        DoActions(controller, _startActions);
    }

    public void UpdateState(StateMachineController controller)
    {
        DoActions(controller, _actions);

        CheckTransitions(controller);
    }

    public void EndState(StateMachineController controller)
    {
        DoActions(controller, _endActions);
    }

    private void CheckTransitions(StateMachineController controller)
    {
        for (int i = 0; i < _transitions.Length; i++)
        {
            bool decisionSucceeded = _transitions[i].Decision.Decide(controller);

            if (decisionSucceeded && _transitions[i].StateOnTrue != null)
            {
                controller.TransitionToState(_transitions[i].StateOnTrue);
                break;
            }
            else if (!decisionSucceeded && _transitions[i].StateOnFalse != null)
            {
                controller.TransitionToState(_transitions[i].StateOnFalse);
                break;
            }
        }
    }
}
