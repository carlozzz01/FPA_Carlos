using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Look Decision", menuName = "State Machine/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateMachineController controller)
    {
        return Look(controller);
    }

    private bool Look(StateMachineController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.Eyes.position, controller.Eyes.forward * controller.Stats.Reach);

        Ray ray = new Ray(controller.Eyes.position, controller.Eyes.forward);

        bool targetFound = Physics.SphereCast
        (
            ray,
            controller.Stats.LookSphereCastRadius,
            out hit,
            controller.Stats.Reach,
            controller.Stats.TargetLayers
        );

        if (targetFound)
        {
            controller.SetTarget(hit.transform);

            return true;
        }

        return false;
    }
}
