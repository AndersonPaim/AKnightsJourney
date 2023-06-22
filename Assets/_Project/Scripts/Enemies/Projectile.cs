using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coimbra;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private GameObject _trailEffect;
    [SerializeField] private VisualEffect _explosionEffect;

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        _trailEffect.SetActive(false);
        _explosionEffect.gameObject.SetActive(true);

        if(damageable != null)
        {
            damageable.TakeDamage(100, gameObject);

            if(_hitParticle != null)
            {
                //ParticleSystem particle = Instantiate(_hitParticle, other.transform.position, other.transform.rotation);
            }
        }

        StartCoroutine(DestroyDelay(0.15f));
    }

    private void OnEnable()
    {
        _trailEffect.SetActive(true);
        _explosionEffect.gameObject.SetActive(false);
        StartCoroutine(DestroyDelay(6));
    }

    private IEnumerator DestroyDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyImmediate(gameObject);
    }

}
