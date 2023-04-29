using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Colletables
{
    [SerializeField] private ParticleSystem _diamondParticle;
    [SerializeField] private GameObject _diamondObject;
    private Animator _anim;

    protected override void Initialize()
    {
        base.Initialize();
        _anim = _diamondObject.GetComponent<Animator>();
        itemPoints = 3;
        destroyDelay = 0.6f;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnEnterTrigger(other);
        _diamondParticle.Play();
        _anim.enabled = false;
    }

    protected override IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyDelay);
        DestroyImmediate(gameObject);
    }
}
