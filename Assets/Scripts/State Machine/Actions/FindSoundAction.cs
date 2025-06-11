using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Find Sound Action", menuName = "State Machine/Actions/Find Sound")]
public class FindSoundAction : StateAction
{
    public override void Act(StateMachineController controller)
    {
        FindSound(controller);
    }

    private void FindSound(StateMachineController controller)
    {
        if (controller.HeardSounds.Count > 0)
        {
            // focus on last sound heard
            controller.FocusOnLastSound();
        }

        controller.GoToLastSoundPosition();
        controller.Chase();
    }
}
