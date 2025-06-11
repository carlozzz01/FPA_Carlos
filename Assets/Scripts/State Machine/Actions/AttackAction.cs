using UnityEngine;

[CreateAssetMenu(fileName = "Attack Action", menuName = "State Machine/Actions/Attack")]
public class AttackAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        Attack(controller);
    }

    private void Attack(StateMachineController controller)
    {
        controller.StopMoving();
        Debug.Log("Attack player");
    }
}
