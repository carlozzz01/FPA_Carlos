using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _emptySlotBackground;
    [SerializeField] private Image _filledSlotBackground;
    [SerializeField] private Image _selectedSlotBackground;

    private void Awake()
    {
        _emptySlotBackground.gameObject.SetActive(true);
        _filledSlotBackground.gameObject.SetActive(false);
        _selectedSlotBackground.gameObject.SetActive(false);
    }

    public void Clear()
    {
        _icon.gameObject.SetActive(false);

        _filledSlotBackground.gameObject.SetActive(false);
    }

    public void SetSprite(string itemID)
    {
        Sprite sprite = Resources.Load<Sprite>($"Items/{itemID}");

        _icon.sprite = sprite;

        _icon.gameObject.SetActive(true);

        _filledSlotBackground.gameObject.SetActive(true);
    }
}
