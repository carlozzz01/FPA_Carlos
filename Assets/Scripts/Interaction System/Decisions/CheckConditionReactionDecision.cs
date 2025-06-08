using Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "Check Condition", menuName = "Reaction System/Decisions/Check Condition")]
public class CheckConditionReactionDecision : ReactionDecision
{
    [Header("Configuration")]
    [SerializeField] private string _conditionID;

    public override bool CheckDecision()
    {
        return DataManager.Instance.Data.GetCondition(_conditionID).IsConditionMet;
    }
}
