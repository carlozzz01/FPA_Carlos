using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Pool[] _pools;

    private static PoolManager _instance;
    public static PoolManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        PoolEntity.OnReturnToPool += Push;
    }

    private void OnDisable()
    {
        PoolEntity.OnReturnToPool -= Push;
    }

    private void Start()
    {
        InitializePools();
    }

    /// <summary>
    /// Returns an entity to its corresponding pool.
    /// </summary>
    /// <param name="entity"></param>
    private void Push(PoolEntity entity)
    {
        foreach (Pool pool in _pools)
        {
            if (pool.ID == entity.ID)
            {
                pool.Push(entity);
            }
        }
    }

    /// <summary>
    /// Pulls an entity from the pool in the given position and rotation
    /// </summary>
    /// <param name="poolID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public PoolEntity Pull(string poolID, Vector3 position, Quaternion rotation)
    {
        PoolEntity entity = null;

        foreach (Pool pool in _pools)
        {
            if (pool.ID == poolID)
            {
                if (!pool.TryPull(out entity))
                {
                    entity = CreatePoolEntity(poolID);
                }
            }
        }

        if (entity != null)
        {
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            entity.Initialize();
        }

        return entity;
    }

    /// <summary>
    /// Initializes all the pools created in the Editor.
    /// </summary>
    private void InitializePools()
    {
        foreach (Pool pool in _pools)
        {
            for (int i = 0; i < pool.Prewarm; i++)
            {
                PoolEntity entity = CreatePoolEntity(pool.ID);
                entity.Deactivate();
                pool.Push(entity);
            }
        }
    }

    /// <summary>
    /// Creates an entity for the pool with the given ID.
    /// </summary>
    /// <param name="poolID"></param>
    /// <returns></returns>
    private PoolEntity CreatePoolEntity(string poolID)
    {
        PoolEntity entity = null;

        foreach (Pool pool in _pools)
        {
            if (pool.ID == poolID)
            {
                entity = Instantiate(pool.Prefab, transform);
                entity.SetID(poolID);
            }
        }

        return entity;
    }
}
