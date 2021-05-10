using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Colletables : MonoBehaviour
{
    public delegate void CollectItemHandler(int points);
    public static CollectItemHandler OnCollectItem;

    public int itemPoints;

    public float destroyDelay;

    private Collider _collider;

    public virtual void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _collider = GetComponent<Collider>();
    }

    protected void CollectItem()
    {
        OnCollectItem?.Invoke(itemPoints);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        OnEnterTrigger(other);
    }

    protected virtual void OnEnterTrigger(Collider other)
    {
        CollectItem();
        Collider.enabled = false;
        StartCoroutine(DestroyObject());
    }

    protected abstract IEnumerator DestroyObject();

    public Collider Collider
    {
        get { return _collider; }
    }
}