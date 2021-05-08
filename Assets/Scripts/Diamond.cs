using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Colletables
{
    private BoxCollider _collider;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _collider = gameObject.GetComponent<BoxCollider>();
        itemPoints = 3;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CollectItem();
        _collider.enabled = false;
        StartCoroutine(DestroyObject());
    }

    protected override IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
