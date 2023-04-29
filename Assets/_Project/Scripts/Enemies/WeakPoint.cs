using System.Collections;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    [SerializeField] private EnemyMelee _enemy;
    [SerializeField] private ParticleSystem _hitParticle;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER");
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = _enemy.GetComponent<IDamageable>();
            PlayerController playerController = other.GetComponent<PlayerController>();

            if(damageable != null && !_enemy.IsDead)
            {
                damageable.TakeDamage(9999, gameObject);
                ParticleSystem particle = Instantiate(_hitParticle);
                particle.transform.position = other.transform.position;
                StartCoroutine(DestroyParticle(particle.gameObject, particle.main.duration));
                playerController.SlimeJump();
            }
        }
    }

    private IEnumerator DestroyParticle(GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyImmediate(particle);
    }
}
