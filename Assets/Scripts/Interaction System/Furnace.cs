using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;

public class Furnace : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string _furnaceCondition;
    [SerializeField] private string _coalID;
    [SerializeField] private UnityEvent _OnIgnite;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PoolEntity entity))
        {
            if (entity.ID == _coalID)
            {
                DataManager.Instance.SetCondition(_furnaceCondition, true);

                _OnIgnite?.Invoke();
            }
        }
    }
}
