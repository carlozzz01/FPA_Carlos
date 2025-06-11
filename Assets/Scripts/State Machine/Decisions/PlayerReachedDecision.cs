using UnityEngine;

[CreateAssetMenu(fileName = "Player Reached Decision", menuName = "State Machine/Decisions/Player Reached")]
public class PlayerReachedDecision : Decision
{
    public override bool Decide(StateMachineController controller)
    {
        return Arrive(controller);
    }

    private bool Arrive(StateMachineController controller)
    {
        return controller.IsCloseToPlayer();
    }
}
