using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyPatrolAI, IDamageable
{

    public void TakeDamage(float damage)
    {
        _anim.SetTrigger("Die");
        _isDead = true;
        StartCoroutine(DestroyDelay(2));
    }

    public void EnableHitCollider()
    {
        _hitCollider.enabled = true;
    }

    public void DisableHitCollider()
    {
        _hitCollider.enabled = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_canSeePlayer)
        {
            ChasePlayer();

            if(CanAttackPlayer() && _canAttack)
            {
                _canMove = false;
                Attack();
            }
        }

        if(!_canMove)
        {
            if(!CanAttackPlayer())
            {
                _canMove = true;
            }
        }
    }

    protected override void CanSeePlayer(bool canSee)
    {
        base.CanSeePlayer(canSee);
        _anim.SetBool("CanSeePlayer", canSee);
    }

    private void ChasePlayer()
    {
        if(_isDead)
        {
            return;
        }

        Vector3 look = new Vector3(transform.position.x, 0,  _player.transform.position.z);
        transform.LookAt(look);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void Attack()
    {
        StartCoroutine(AttackDelay(_attackDelay));
    }

    private IEnumerator AttackDelay(float delay)
    {
        yield return new WaitForSeconds(1);
        _anim.SetTrigger("Attack");
        _canAttack = false;
        yield return new WaitForSeconds(delay);
        _canAttack = true;
    }

    private IEnumerator DestroyDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
