using UnityEngine;

[CreateAssetMenu(fileName = "Decrease Timer Action", menuName = "State Machine/Actions/Decrease Timer")]
public class DecreaseTimerAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        DecreaseTimer(controller);
    }

    private void DecreaseTimer(StateMachineController controller)
    {
        controller.DecreaseStateTimer();
    }
}
