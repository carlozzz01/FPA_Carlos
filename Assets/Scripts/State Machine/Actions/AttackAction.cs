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
        Debug.Log("Attack player");

        controller.StopMoving();

        controller.SetHitReceiverActive(false);

        GameManager.Instance.GameOver();
    }
}
