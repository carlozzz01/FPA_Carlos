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
        // Debug.Log("looking");

        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, controller.Stats.Reach, controller.Stats.TargetLayers);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (Vector3.Angle(collider.transform.position - controller.Eyes.position, controller.transform.forward) < controller.Stats.FieldOfView / 2)
                {
                    RaycastHit hit;

                    float distanceToCollider = Vector3.Distance(controller.Eyes.position, collider.bounds.center);

                    distanceToCollider = Mathf.Clamp(distanceToCollider, 0, controller.Stats.Reach);

                    Ray ray = new Ray(controller.Eyes.position, collider.transform.position + Vector3.up - controller.Eyes.position);

                    if (!Physics.Raycast(ray, out hit, distanceToCollider, _obstacleLayers))
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
