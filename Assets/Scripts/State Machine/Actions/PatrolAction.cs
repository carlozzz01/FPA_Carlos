using UnityEngine;

[CreateAssetMenu(fileName = "Patrol Action", menuName = "State Machine/Actions/Patrol")]
public class PatrolAction : StateAction
{
    [Header("Configuration")]
    [SerializeField] private bool _randomPatrol = false;

    public override void Act(StateMachineController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateMachineController controller)
    {
        controller.Patrol();

        if (controller.IsCloseToDestination())
        {
            if (_randomPatrol)
            {
                controller.GoToRandomWaypoint();
            }
            else
            {
                controller.IncreaseNextWaypointIndex();
            }
        }
    }
}
