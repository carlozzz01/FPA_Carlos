using UnityEngine;
using Managers;

public class ReactionCondition : Reaction
{
    [SerializeField] private string _conditionID;
    [SerializeField] private bool _value;

    protected override void React()
    {
        DataManager.Instance.Data.GetCondition(_conditionID).SetState(_value);
    }
}
