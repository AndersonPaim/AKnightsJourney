using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private Animator _cameraShake;

    private bool _canDamage = true;

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        DamageFlash flash = other.gameObject.GetComponent<DamageFlash>();

        if(damageable != null)
        {
            damageable.TakeDamage(100, gameObject);
            _cameraShake.SetTrigger("Shake");

            if(_hitParticle != null && _canDamage)
            {
                ParticleSystem particle = Instantiate(_hitParticle, other.transform.position, other.transform.rotation);
                StartCoroutine(DestroyParticle(particle.gameObject, particle.main.duration));
            }
        }

        if(flash != null)
        {
            flash.Flash();
        }

        StartCoroutine(DamageDelay());
    }

    private IEnumerator DamageDelay()
    {
        _canDamage = false;
        yield return new WaitForSeconds(0.5f);
        _canDamage = true;
    }

    private IEnumerator DestroyParticle(GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(particle);
    }
}
