using System.Collections;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;

    private bool _canDamage = true;

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        DamageFlash flash = other.gameObject.GetComponent<DamageFlash>();

        if(damageable != null)
        {
            damageable.TakeDamage(100, gameObject);

            VisualEffects.sInstance.CameraShake();

            if(_hitParticle != null && _canDamage)
            {
                Debug.Log("PARTICLE");
                ParticleSystem particle = Instantiate(_hitParticle);
                particle.transform.position = other.transform.position;
                StartCoroutine(DestroyParticle(particle.gameObject, particle.main.duration));
            }
        }

        if(flash != null)
        {
            flash.Flash();
        }

        //StartCoroutine(DamageDelay());
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
