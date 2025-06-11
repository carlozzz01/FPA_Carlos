using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Rotate Lost Action", menuName = "State Machine/Actions/Rotate Lost")]
public class RotateLostAction : StateAction
{
    [SerializeField] private float _turnSpeed = 4f;
    [SerializeField] private float _lostDuration = 2f;

    public override void Act(StateMachineController controller)
    {
        RotateTowardsTarget(controller);
    }

    private void RotateTowardsTarget(StateMachineController controller)
    {
        if (!controller.IsStateTimerRunning())
        {
            controller.StartStateTimer(_lostDuration);
        }

        controller.StopMoving();

        if (controller.Target != null)
        {
            Vector3 goalDirection = Vector3.ProjectOnPlane(controller.Target.position - controller.Eyes.position, Vector3.up);

            Quaternion goalRotation = Quaternion.LookRotation(goalDirection);

            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, goalRotation, _turnSpeed * Time.deltaTime);
        }

        controller.DecreaseStateTimer();

        if (controller.StateTimer <= 0)
        {
            controller.SetTarget(null);
        }
    }
}
