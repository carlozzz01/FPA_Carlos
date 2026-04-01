using UnityEngine;

[CreateAssetMenu(fileName = "Animate Action", menuName = "State Machine/Actions/Animate")]
public class AnimateAction : StateAction
{
    [SerializeField] private string _parameter;
    [SerializeField] private AnimationType _animationType;

    public enum AnimationType
    {
        Float,
        Trigger
    }

    public override void Act(StateMachineController controller)
    {
        Animate(controller);
    }

    private void Animate(StateMachineController controller)
    {
        switch (_animationType)
        {
            case AnimationType.Float:
                controller.FeedFloatToAnimator(_parameter, controller.Velocity.magnitude);
                break;
            case AnimationType.Trigger:
                controller.FeedTriggerToAnimator(_parameter);
                break;
            default:
                break;
        }

    }
}