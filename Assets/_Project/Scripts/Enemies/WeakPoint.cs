using System.Collections;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    [SerializeField] private EnemyMelee _enemy;
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private PlayerController _playerController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = _enemy.GetComponent<IDamageable>();

            if(damageable != null && !_enemy.IsDead)
            {
                damageable.TakeDamage(9999, gameObject);
                ParticleSystem particle = Instantiate(_hitParticle);
                particle.transform.position = other.transform.position;
                StartCoroutine(DestroyParticle(particle.gameObject, particle.main.duration));
                _playerController.SlimeJump();
            }
        }
    }

    private IEnumerator DestroyParticle(GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyImmediate(particle);
    }
}
