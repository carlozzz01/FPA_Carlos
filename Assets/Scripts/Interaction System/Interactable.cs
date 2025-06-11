using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Collider _collider;
    [SerializeField] protected Transform _handle;

    public Transform Handle => _handle;

    public virtual void Awake()
    {
        if (_collider == null) _collider = GetComponent<Collider>();
    }

    private void Start()
    {
    }

    // public virtual void Interact()
    // {
    // }

    public virtual void Interact(PlayerInteraction player)
    {
    }

    public void PullPoolItem(string poolID)
    {
        PoolManager.Instance.Pull(poolID, transform.position, transform.rotation);
    }
}
