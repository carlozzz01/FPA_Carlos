using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "Stop Moving Action", menuName = "State Machine/Actions/Stop Moving")]
public class StopMovingAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        Stop(controller);
    }

    private void Stop(StateMachineController controller)
    {
        controller.StopMoving();
    }
}
