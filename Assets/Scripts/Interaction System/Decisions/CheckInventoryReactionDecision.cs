using Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "Check Inventory", menuName = "Reaction System/Decisions/Check Inventory")]
public class CheckInventoryReactionDecision : ReactionDecision
{
    [Header("Configuration")]
    [SerializeField] private string _itemID;

    public override bool CheckDecision()
    {
        return InventoryManager.Instance.IsItemInInventory(_itemID);
    }
}
