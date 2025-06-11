using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New No Target Decision", menuName = "State Machine/Decisions/No Target")]
public class NoTargetDecision : Decision
{
    public override bool Decide(StateMachineController controller)
    {
        return CheckTarget(controller);
    }

    private bool CheckTarget(StateMachineController controller)
    {
        return controller.Target == null;
    }
}
