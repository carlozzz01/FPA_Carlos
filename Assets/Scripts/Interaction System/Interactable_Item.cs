using UnityEngine;
using Managers;

public class Interactable_Item : Interactable
{
    [SerializeField] private string _itemID;
    [SerializeField] private GameAudio _pickupSound;

    public override void Interact(PlayerInteraction player)
    {
        if (DataManager.Instance.Data.TryGetItem(_itemID, out Item result))
        {
            if (InventoryManager.Instance.TryAddItemToInventory(_itemID))
            {
                AudioSource.PlayClipAtPoint(_pickupSound.clip, transform.position, _pickupSound.volume * AudioManager.Instance.SFXVolume);
                
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"No item found with name {_itemID}");
        }
    }
}
