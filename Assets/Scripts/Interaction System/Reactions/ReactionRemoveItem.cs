using Managers;
using UnityEngine;

public class ReactionRemoveItem : Reaction
{
    [Header("Configuration")]
    [SerializeField] private string _itemID;

    protected override void React()
    {
        InventoryManager.Instance.TryRemoveItemFromInventory(_itemID);
    }
}
