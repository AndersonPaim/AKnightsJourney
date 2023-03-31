using System.Collections;
using UnityEngine;

public class EnemyPig : EnemyMeleePatrolAI, IDamageable
{
    [SerializeField] private float _health;

    private Coroutine _lastRoutine = null;

    public void TakeDamage(float damage, GameObject attacker)
    {
        _health -= damage;

        if(_health > 0)
        {
            _anim.SetTrigger("TakeDamage");
            Knockback(attacker);
        }
        else
        {
            VisualEffects.sInstance.StartRippleEffect(transform.position);
            _anim.SetTrigger("Die");
            _isDead = true;
            StopCoroutine(_lastRoutine);
            StartCoroutine(DestroyDelay(2));
            GetComponent<Collider>().enabled = false;
            _hitCollider.enabled = false;
            _rb.isKinematic = true;
        }

        CancelAttack();
        _canAttack = true;
    }

    public void EnableHitCollider()
    {
        _hitCollider.enabled = true;
    }

    public void DisableHitCollider()
    {
        _hitCollider.enabled = false;
    }

    private void Knockback(GameObject attacker)
    {
        Vector3 knockbackDirection = new Vector3(0, 0, gameObject.transform.position.z - attacker.transform.position.z);
        _rb.velocity = new Vector3(0, 0, knockbackDirection.z) * 40;
    }

    private void LookAtPlayer()
    {
        if(_isDead)
        {
            return;
        }

        Vector3 look = new Vector3(transform.position.x, 0,  _player.transform.position.z);
        transform.LookAt(look);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_canSeePlayer)
        {
            if(CanAttackPlayer() && _canAttack)
            {
                _canMove = false;
                _lastRoutine = StartCoroutine(PrepareAttack(1));
            }
            else if(CanAttackPlayer())
            {
                _canMove = false;
            }
            else if(!CanAttackPlayer() && !_canAttack)
            {
                CancelAttack();
            }
        }
        else if(!_canAttack && !CanAttackPlayer())
        {
            StopCoroutine(_lastRoutine);
        }

        if(_canAttack)
        {
            Move();
            _anim.SetBool("IsMoving", true);
        }
        else
        {
            LookAtPlayer();
            _anim.SetBool("IsMoving", false);
        }

        if(!_canMove)
        {
            if(!CanAttackPlayer())
            {
                CancelAttack();
                _canMove = true;
            }
        }
    }

    protected override void CanSeePlayer(bool canSee)
    {
        base.CanSeePlayer(canSee);
        _anim.SetBool("CanSeePlayer", canSee);
    }

    private void Attack()
    {
        _lastRoutine = StartCoroutine(AttackDelay(_attackDelay));
    }

    private IEnumerator PrepareAttack(float delay)
    {
        _canAttack = false;
        _anim.SetTrigger("Idle");
        yield return new WaitForSeconds(delay);
        Attack();
    }

    private IEnumerator AttackDelay(float delay)
    {
        _anim.SetTrigger("Attack");
        _rb.AddForce((Vector3.forward * GetDirection()) * 100, ForceMode.Impulse);
        yield return new WaitForSeconds(delay);
        _canAttack = true;
    }

    private int GetDirection()
    {
        if(gameObject.transform.eulerAngles.y < 90)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    private IEnumerator DestroyDelay(float delay)
    {
        yield return new WaitForSeconds(0.2f);
        VisualEffects.sInstance.StopRippleEffect();
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void CancelAttack()
    {
        if(_lastRoutine != null)
        {
            StopCoroutine(_lastRoutine);
            _canAttack = true;
        }
    }
}
