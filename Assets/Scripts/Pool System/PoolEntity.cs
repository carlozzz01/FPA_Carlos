using System;
using UnityEngine;

public class PoolEntity : MonoBehaviour
{
    [Header("Pool Entity Configuration")]
    [SerializeField] private string _poolID;
    [SerializeField] protected bool _active;

    [Header("Pool Entity Components")]
    [SerializeField] private Renderer[] _renderers;

    public static Action<PoolEntity> OnReturnToPool;

    public string ID => _poolID;

    /// <summary>
    /// Initializes the entity's parameters after being pulled out of the pool.
    /// </summary>
    public virtual void Initialize()
    {
        EnableRenderers(true);
        _active = true;
    }

    /// <summary>
    /// Deactivates the entity's parameters befores being pushed into the pool.
    /// </summary>
    public virtual void Deactivate()
    {
        EnableRenderers(false);
        _active = false;
    }

    /// <summary>
    /// Returns the entity, deactivating itself.
    /// </summary>
    public void ReturnToPool()
    {
        Deactivate();
        OnReturnToPool.Invoke(this);
    }

    /// <summary>
    /// Toggles the renderers active state
    /// </summary>
    /// <param name="state"></param>
    protected void EnableRenderers(bool state)
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.enabled = state;
        }
    }

    /// <summary>
    /// Finds and stores the entity's renderers.
    /// </summary>
    [ContextMenu("Find renderers")]
    public void FindRenderers()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }

    /// <summary>
    /// Sets the entity ID
    /// </summary>
    /// <param name="ID"></param>
    public void SetID(string ID)
    {
        _poolID = ID;
    }
}
