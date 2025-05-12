using System.Linq;
using UnityEngine;

[System.Serializable]
public class Data
{
    public string currentScene;
    public string entrancePosition;
    [SerializeField] private Condition[] _gameConditions;

    public Condition[] GameConditions => _gameConditions;

    public Condition GetCondition(string conditionID)
    {
        return _gameConditions.SingleOrDefault(c => c.id == conditionID);
    }
}
