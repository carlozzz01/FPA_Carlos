using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Data
{
    [SerializeField] private List<Condition> _gameConditions;
    [SerializeField] private Item[] _gameItems;

    public List<Condition> GameConditions => _gameConditions;

    public Condition GetCondition(string conditionID)
    {
        return _gameConditions.SingleOrDefault(c => c.id == conditionID);
    }

    public bool TrySetCondition(string conditionID, bool value)
    {
        int conditionIndex = _gameConditions.FindIndex(c => c.id == conditionID);

        if (conditionIndex > -1)
        {
            _gameConditions[conditionIndex].SetState(value);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryGetItem(string id, out Item item)
    {
        item = _gameItems.SingleOrDefault(item => item.ID == id);

        return item != null;
    }
}
