using Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "Play Sound Action", menuName = "State Machine/Actions/Play Sound")]
public class PlaySoundAction : StateAction
{
    [SerializeField] private GameAudio[] _sounds;
    public override void Act(StateMachineController controller)
    {
        foreach (var sound in _sounds)
        {
            AudioSource.PlayClipAtPoint(sound.clip, controller.transform.position, sound.volume * AudioManager.Instance.SFXVolume);
        }
    }
}
