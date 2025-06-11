using UnityEngine;

[CreateAssetMenu(fileName = "New Complex Look Decision", menuName = "State Machine/Decisions/Complex Look")]
public class ComplexLookDecision : Decision
{
    [SerializeField] private LayerMask _obstacleLayers;

    public override bool Decide(StateMachineController controller)
    {
        return Look(controller);
    }

    private bool Look(StateMachineController controller)
    {
        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, controller.Stats.Reach, controller.Stats.TargetLayers);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (Vector3.Angle(collider.transform.position - controller.Eyes.position, controller.transform.forward) < controller.Stats.FieldOfView / 2)
                {
                    RaycastHit hit;

                    Ray ray = new Ray(controller.Eyes.position, collider.transform.position + Vector3.up - controller.Eyes.position);

                    if (!Physics.Raycast(ray, out hit, controller.Stats.Reach, _obstacleLayers))
                    {
                        controller.SetTarget(collider.transform);

                        return true;
                    }
                }
            }
        }

        return false;
    }
}
