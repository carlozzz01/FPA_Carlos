using UnityEngine;

public class ReactionPlaySound : Reaction
{
    [SerializeField] private GameAudio _audio;

    protected override void React()
    {
        AudioSource.PlayClipAtPoint(_audio.clip, transform.position, _audio.volume);
    }
}