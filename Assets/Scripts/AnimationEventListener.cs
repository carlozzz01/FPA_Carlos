using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : MonoBehaviour
{
    [SerializeField] private AnimationEvent[] _animationEvents;

    public void ExecuteTrigger(string id)
    {
        try
        {
            _animationEvents.FirstOrDefault(a => a.ID == id).events?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No event on given ID {id}");
            throw;
        }
    }
}

[Serializable]
public class AnimationEvent
{
    [SerializeField] private string _id;
    public UnityEvent events;

    public string ID => _id;
}
