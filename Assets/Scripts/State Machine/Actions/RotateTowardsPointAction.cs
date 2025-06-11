using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Rotate Towards Point Action", menuName = "State Machine/Actions/Rotate Towards Point")]
public class RotateTowardsPointAction : StateAction
{

        [SerializeField] private float _turnSpeed = 4f;
    [SerializeField] private float _lostDuration = 2f;

    public override void Act(StateMachineController controller)
    {
        RotateTowardsPoint(controller);
    }

    private void RotateTowardsPoint(StateMachineController controller)
    {
        if (!controller.IsStateTimerRunning())
        {
            controller.StartStateTimer(_lostDuration);
        }

        controller.StopMoving();

        Vector3 goalDirection = Vector3.ProjectOnPlane(controller.Destination - controller.Eyes.position, Vector3.up);

        Quaternion goalRotation = Quaternion.LookRotation(goalDirection);

        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, goalRotation, _turnSpeed * Time.deltaTime);

        controller.DecreaseStateTimer();

        if (controller.StateTimer <= 0)
        {
            controller.SetTarget(null);
        }
    }
}
