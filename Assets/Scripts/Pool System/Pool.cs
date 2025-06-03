using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    [SerializeField] private string _id;
    [SerializeField] private PoolEntity _prefab;
    [SerializeField] private int _prewarm;
    [SerializeField] private Queue<PoolEntity> _entityQueue = new Queue<PoolEntity>();

    public Queue<PoolEntity> EntityQueue => _entityQueue;

    public string ID => _id;
    public PoolEntity Prefab => _prefab;
    public int Prewarm => _prewarm;

    /// <summary>
    /// Stores entity inside the queue. First in, first out.
    /// </summary>
    /// <param name="entity"></param>
    public void Push(PoolEntity entity)
    {
        _entityQueue.Enqueue(entity);
    }

    /// <summary>
    /// Tries to pull entity from the queue. First in, first out.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool TryPull(out PoolEntity entity)
    {
        return _entityQueue.TryDequeue(out entity);
    }
}
