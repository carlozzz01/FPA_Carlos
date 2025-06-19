using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Set Patrol Speed Action", menuName = "State Machine/Actions/Set Patrol Speed")]
public class SetPatrolSpeed : StateAction
{
    public override void Act(StateMachineController controller)
    {
        SetSpeed(controller);
    }

    private void SetSpeed(StateMachineController controller)
    {
        controller.SetSpeed(controller.Stats.PatrolSpeed);
    }
}
