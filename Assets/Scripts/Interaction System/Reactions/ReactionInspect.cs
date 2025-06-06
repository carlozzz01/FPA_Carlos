using UnityEngine;

public class ReactionInspect : Reaction
{
    [SerializeField] private string _id;

    protected override void React()
    {
        ItemInspectorManager.Instance.ShowItem(_id);
    }
}
