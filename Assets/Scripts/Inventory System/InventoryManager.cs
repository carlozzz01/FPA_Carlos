using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private Item[] _inventory = new Item[4];

        [Header("UI")]
        [SerializeField] private InventoryUI _ui;

        private static InventoryManager _instance;
        public static InventoryManager Instance => _instance;

        // public Item[] Inventory => _inventory;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public bool TryAddItemToInventory(string itemID)
        {
            int emptyInventorySlotIndex = -1;

            for (int i = 0; i < _inventory.Length; i++)
            {
                if (string.IsNullOrEmpty(_inventory[i].ID))
                {
                    emptyInventorySlotIndex = i;
                    break;
                }
            }

            if (emptyInventorySlotIndex < 0)
            {
                Debug.LogWarning("Inventory is full");

                return false;
            }

            if (DataManager.Instance.Data.TryGetItem(itemID, out Item item))
            {
                Item newItem = new Item(item.ID);

                _inventory[emptyInventorySlotIndex] = newItem;

                _ui.UpdateInventory();

                return true;
            }

            return false;
        }

        public string GetItemID(int index)
        {
            return _inventory[index].ID;
        }
    }
}

