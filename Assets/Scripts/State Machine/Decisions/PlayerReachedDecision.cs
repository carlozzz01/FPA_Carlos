using UnityEngine;

[CreateAssetMenu(fileName = "Player Reached Decision", menuName = "State Machine/Decisions/Player Reached")]
public class PlayerReachedDecision : Decision
{
    public override bool Decide(StateMachineController controller)
    {
        bool reached = controller.IsCloseToPlayer();
        Debug.Log($"PlayerReached: {reached}");
        return reached;
    }
}
