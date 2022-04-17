using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.TakeDamage(100, gameObject);
            //Instantiate(_hitParticle, other.transform.position, other.transform.rotation);
        }
    }
}
