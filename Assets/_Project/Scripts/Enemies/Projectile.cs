using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
            }
        }

        Destroy(gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

}
