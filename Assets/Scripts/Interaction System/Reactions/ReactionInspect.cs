using UnityEngine;

public class ReactionInspect : Reaction
{
    [SerializeField] private string _id;

    protected override void React()
    {
        InspectorManager.Instance.ShowItem(_id);
    }
}
