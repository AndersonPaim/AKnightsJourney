using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyPatrolAI, IDamageable
{
    public void TakeDamage(float damage)
    {
        Destroy(gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_canSeePlayer)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector3 look = new Vector3(transform.position.x, 0,  _player.transform.position.z);
        transform.LookAt(look);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
