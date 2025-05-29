using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private Item[] _inventory = new Item[4];

        private static InventoryManager _instance;
        public static InventoryManager Instance => _instance;

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
    }
}

