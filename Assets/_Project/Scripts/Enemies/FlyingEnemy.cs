using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _projectileObj;
    [SerializeField] private List<Transform> _projectilePosList;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _pathAnim;
    [SerializeField] private Collider _hitCollider;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _shootForce;
    [SerializeField] private float _health;

    private Coroutine _lastRoutine = null;
    private bool _isDead = false;

    public void TakeDamage(float damage, GameObject attacker)
    {
        _health -= damage;

        if(_health > 0)
        {
            _anim.SetTrigger("TakeDamage");
        }
        else
        {
            _pathAnim.enabled = false;
            _anim.SetTrigger("Die");
            StartCoroutine(DestroyDelay(2));
            GetComponent<Collider>().enabled = false;
            _hitCollider.enabled = false;
            _isDead = true;
            _rb.useGravity = true;
            _rb.isKinematic = true;
        }
    }

    private void Start()
    {
        Coroutine coroutine = StartCoroutine(Shoot());
        _lastRoutine = coroutine;
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(_shootDelay);

        _anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.4f); //sync shoot with animation

        if(!_isDead)
        {
            foreach(Transform pos in _projectilePosList)
            {
                GameObject obj = Instantiate(_projectileObj, pos.position, pos.rotation);
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.AddRelativeForce(Vector3.forward * _shootForce);
            }

            StartCoroutine(Shoot());
         }
    }

    private IEnumerator DestroyDelay(float delay)
    {
        if(_lastRoutine != null)
        {
            StopCoroutine(_lastRoutine);
        }

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
