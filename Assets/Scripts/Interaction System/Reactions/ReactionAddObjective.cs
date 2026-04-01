using UnityEngine;

public class ReactionAddObjective : Reaction
{
    [SerializeField] private ObjectiveSO _objective;

    protected override void React()
    {
        ObjectivesManager.Instance.AddObjective(_objective);
    }
}
