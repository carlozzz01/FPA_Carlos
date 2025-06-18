using UnityEngine;

[CreateAssetMenu(fileName = "Investigate Action", menuName = "State Machine/Actions/Investigate")]
public class InvestigateAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        Investigate(controller);
    }

    private void Investigate(StateMachineController controller)
    {
        controller.GoToSuspicionPoint();
    }
}
