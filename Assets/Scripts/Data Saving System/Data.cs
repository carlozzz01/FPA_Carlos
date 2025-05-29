using System.Linq;
using UnityEngine;

[System.Serializable]
public class Data
{
    [SerializeField] private Condition[] _gameConditions;
    [SerializeField] private Item[] _gameItems;

    public Condition[] GameConditions => _gameConditions;

    public Condition GetCondition(string conditionID)
    {
        return _gameConditions.SingleOrDefault(c => c.id == conditionID);
    }

    public bool TryGetItem(string id, out Item item)
    {
        item = _gameItems.SingleOrDefault(item => item.ID == id);

        return item != null;
    }
}
