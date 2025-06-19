using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "Start Moving Action", menuName = "State Machine/Actions/Start Moving")]
public class StartMovingAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        StartMoving(controller);
    }

    private void StartMoving(StateMachineController controller)
    {
        controller.StartMoving();
    }
}
