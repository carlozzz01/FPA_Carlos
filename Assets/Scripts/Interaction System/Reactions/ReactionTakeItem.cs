using Managers;
using UnityEngine;

public class ReactionTakeItem : Reaction
{
    [SerializeField] private string _itemID;

    protected override void React()
    {
        if (DataManager.Instance.Data.TryGetItem(_itemID, out Item result))
        {
            if (InventoryManager.Instance.TryAddItemToInventory(_itemID))
            {
                // result._isPicked = true;
            }
        }
        else
        {
            Debug.LogWarning($"No item found with name {_itemID}");
        }
    }
}
