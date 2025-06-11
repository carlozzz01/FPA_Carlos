using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SoundGenerator
{
    public static void GenerateSound(Vector3 position, float range)
    {
        Collider[] collisions = Physics.OverlapSphere(position, range);

        foreach (Collider collider in collisions)
        {
            float distanceToCollider = Vector3.Distance(collider.transform.position, position);

            bool isStateMachine = collider.TryGetComponent(out StateMachineController controller) && distanceToCollider <= controller.Stats.HearRange;

            if (isStateMachine)
            {
                controller.HearSound(position);
            }
        }
    }
}
