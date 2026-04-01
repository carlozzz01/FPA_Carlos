using UnityEngine;

[CreateAssetMenu(fileName = "Player Visible Decision", menuName = "State Machine/Decisions/Player Visible")]
public class PlayerVisibleDecision : Decision
{
    [SerializeField] private int _framesToLose = 5;
    private int _framesNotSeen = 0;

    public override bool Decide(StateMachineController controller)
    {
        bool seen = CheckLOS(controller);
        float distance = controller.Target != null ? Vector3.Distance(controller.transform.position, controller.Target.position) : -1f;
        Debug.Log($"Seen: {seen} | Distance: {distance} | MinAttackRange: {controller.Stats.MinAttackRange} | FramesNotSeen: {_framesNotSeen}");

        if (seen)
        {
            _framesNotSeen = 0;
            return true;
        }

        _framesNotSeen++;
        return _framesNotSeen < _framesToLose;
    }

    private bool CheckLOS(StateMachineController controller)
    {
        if (controller.Target == null) return false;

        float distanceToTarget = Vector3.Distance(controller.transform.position, controller.Target.position);

        // Si está muy cerca, siempre lo ve independientemente del FOV
        if (distanceToTarget <= controller.Stats.MinAttackRange * 1.5f)
        {
            return true;
        }

        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, controller.Stats.Reach, controller.Stats.TargetLayers);

        foreach (Collider col in colliders)
        {
            Vector3 directionToTarget = col.transform.position - controller.Eyes.position;
            directionToTarget.y = 0f; // ignorar diferencia vertical

            if (Vector3.Angle(directionToTarget, controller.transform.forward) < controller.Stats.FieldOfView / 2)
            {
                return true;
            }

        }

        return false;
    }
}