using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase Action", menuName = "State Machine/Actions/Chase")]
public class ChaseAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        Chase(controller);
    }

    private void Chase(StateMachineController controller)
    {
        if (controller.Target == null) return;

        // controller.GoToSuspicionPoint();
        controller.Chase();

        // if (controller.IsCloseToDestination())
        // {
        //     controller.SetTarget(null);
        // }
    }
}
