using UnityEngine;
using Managers;

public class Interactable_Item : Interactable
{
    [SerializeField] private string _itemID;

    public override void Interact(PlayerInteraction player)
    {
        if (DataManager.Instance.Data.TryGetItem(_itemID, out Item result))
        {
            if (InventoryManager.Instance.TryAddItemToInventory(_itemID))
            {
                // result._isPicked = true;
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"No item found with name {_itemID}");
        }
    }
}
