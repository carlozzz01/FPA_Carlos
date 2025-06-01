using System.Collections;
using UnityEngine;
using Managers;

public class InventoryUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _hotbar;
    [SerializeField] private InventorySlot[] _inventory;

    [Header("Configuration")]
    [SerializeField] private float _transitionDuration;
    [SerializeField] private Transform _hiddenPosition;
    [SerializeField] private Transform _shownPosition;

    private void Awake()
    {
        // _hotbar.position = _hiddenPosition.position;
    }

    private IEnumerator FadeTowards(float goalAlpha, Vector3 goalPosition)
    {
        float timer = 0;
        Vector3 initialPosition = _hotbar.position;

        while (timer < _transitionDuration)
        {
            _hotbar.position = Vector3.Lerp(initialPosition, goalPosition, timer / _transitionDuration);

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        _hotbar.position = goalPosition;
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < _inventory.Length; i++)
        {
            InventorySlot slot = _inventory[i];

            if (!string.IsNullOrEmpty(InventoryManager.Instance.GetItemID(i)))
            {
                slot.SetSprite(InventoryManager.Instance.GetItemID(i));
            }
            else
            {
                slot.Clear();
            }
        }
    }
}
