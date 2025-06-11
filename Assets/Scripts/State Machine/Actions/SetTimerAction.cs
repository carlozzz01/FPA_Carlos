using UnityEngine;

[CreateAssetMenu(fileName = "Set Timer Action", menuName = "State Machine/Actions/Set Timer")]
public class SetTimerAction : StateAction
{
    [SerializeField] private float _timerDuration;

    public override void Act(StateMachineController controller)
    {
        SetTimer(controller);
    }

    private void SetTimer(StateMachineController controller)
    {
        if (!controller.IsStateTimerRunning())
        {
            controller.StartStateTimer(_timerDuration);
        }
    }
}
