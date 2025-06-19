using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Lose Target Action", menuName = "State Machine/Actions/Lose Target")]
public class LoseTargetAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        LoseTarget(controller);
    }

    private void LoseTarget(StateMachineController controller)
    {
        controller.SetTarget(null);
    }
}
