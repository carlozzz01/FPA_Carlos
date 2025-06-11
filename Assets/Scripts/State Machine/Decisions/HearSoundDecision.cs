using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Hear Sound Decision", menuName = "State Machine/Decisions/Hear Sound")]
public class HearSoundDecision : Decision
{
    public override bool Decide(StateMachineController controller)
    {
        return HearSound(controller);
    }

    private bool HearSound(StateMachineController controller)
    {
        return controller.HeardSounds.Count > 0;
    }
}
