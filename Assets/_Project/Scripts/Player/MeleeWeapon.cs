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

            if(_hitParticle != null)
            {
                ParticleSystem particle = Instantiate(_hitParticle, other.transform.position, other.transform.rotation);
                StartCoroutine(DestroyParticle(particle.gameObject, particle.main.duration));
            }
        }
    }

    private IEnumerator DestroyParticle(GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(particle);
    }
}
