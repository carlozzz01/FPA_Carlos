using UnityEngine;
using Managers;

public class ReactionSetCondition : Reaction
{
    [SerializeField] private string _conditionID;
    [SerializeField] private bool _value;

    protected override void React()
    {
        DataManager.Instance.SetCondition(_conditionID, _value);
    }
}
