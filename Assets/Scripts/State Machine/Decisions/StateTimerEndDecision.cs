using System;
using UnityEngine;

[CreateAssetMenu(fileName = "State Timer End Decision", menuName = "State Machine/Decisions/State Timer End")]
public class StateTimerEndDecision : Decision
{
    public override bool Decide(StateMachineController controller)
    {
        return CheckTimer(controller);
    }

    private bool CheckTimer(StateMachineController controller)
    {
        return controller.StateTimer <= 0;
    }
}
