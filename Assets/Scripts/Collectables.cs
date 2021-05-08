using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Colletables : MonoBehaviour
{
    public delegate void CollectItemHandler(int points);
    public static CollectItemHandler OnCollectItem;

    public int itemPoints;

    protected void CollectItem()
    {
        OnCollectItem?.Invoke(itemPoints);
    }
    protected abstract void OnTriggerEnter(Collider other);

    protected abstract IEnumerator DestroyObject();
}