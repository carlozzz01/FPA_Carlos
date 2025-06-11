using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrive At Decision", menuName = "State Machine/Decisions/Arrive At")]
public class ArriveAtDecision : Decision
{
    public override bool Decide(StateMachineController controller)
    {
        return Arrive(controller);
    }

    private bool Arrive(StateMachineController controller)
    {
        return controller.IsCloseToDestination();
    }
}
