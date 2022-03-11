using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Colletables
{
    [SerializeField] private ParticleSystem _coinParticle1;
    [SerializeField] private ParticleSystem _coinParticle2;

    protected override void Initialize()
    {
        base.Initialize();
        itemPoints = 1;
        destroyDelay = 0.6f;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnEnterTrigger(other);
        _coinParticle1.Stop();
        _coinParticle2.Play();
    }

    protected override IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
