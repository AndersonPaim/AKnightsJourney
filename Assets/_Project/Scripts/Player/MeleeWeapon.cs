using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        DamageFlash flash = other.gameObject.GetComponent<DamageFlash>();

        if(damageable != null)
        {
            damageable.TakeDamage(100, gameObject);

            if(_hitParticle != null)
            {
                ParticleSystem particle = Instantiate(_hitParticle, other.transform.position, other.transform.rotation);
                StartCoroutine(DestroyParticle(particle.gameObject, particle.main.duration));
            }
        }

        if(flash != null)
        {
            flash.Flash();
        }
    }

    private IEnumerator DestroyParticle(GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(particle);
    }
}
