using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleePatrolAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _detectPlayerDistance;
    [SerializeField] private float _hitPlayerDistance;
    [SerializeField] private Transform _rayPos;
    [SerializeField] protected float _attackDelay;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected Collider _hitCollider;

    protected Rigidbody _rb;
    private bool _isPatrolling = true;
    private int _direction = 1;
    private Quaternion _enemyRotationLeft;
    private Quaternion _enemyRotationRight;

    protected GameObject _player;
    protected bool _canSeePlayer = false;
    protected bool _canHitPlayer = false;
    protected bool _canMove = true;
    protected bool _isDead = false;
    protected bool _canAttack = true;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _enemyRotationLeft = Quaternion.Euler(0, 180, 0);
        _enemyRotationRight = Quaternion.Euler(0, 0, 0);
    }

    protected virtual void FixedUpdate()
    {
        if(_canMove && !_isDead && !IsNearEdge())
        {
            _rb.velocity += transform.forward * _moveSpeed;
        }

        LookForPlayer();

        if(IsHittingWall() || IsNearEdge())
        {
            if(gameObject.transform.eulerAngles.y < 90)
            {
                ChangeEnemyRotation(-1);
            }
            else
            {
                ChangeEnemyRotation(1);
            }
        }
    }

    private bool IsHittingWall()
    {
        Vector3 targetPos = _rayPos.position;
        targetPos.z += _rayDistance * _direction;

        if(Physics.Linecast(_rayPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsNearEdge()
    {
        Vector3 targetPos = _rayPos.position;
        targetPos.y -= _rayDistance;

        Debug.DrawLine(_rayPos.position, targetPos, Color.blue);

        bool ground = Physics.Linecast(_rayPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));
        bool player = Physics.Linecast(_rayPos.position, targetPos, 1 << LayerMask.NameToLayer("Player"));

        if(ground || player)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ChangeEnemyRotation(int direction)
    {
        _direction = direction;

        switch (direction)
        {
            case -1:
                transform.rotation = Quaternion.Slerp(transform.rotation, _enemyRotationLeft, 1f);
                break;
            case 1:
                transform.rotation = Quaternion.Slerp(transform.rotation, _enemyRotationRight, 1f);
                break;
        }
    }

    protected bool CanAttackPlayer()
    {
        float dist = Vector3.Distance(_player.transform.position, transform.position);

        if(dist < _hitPlayerDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void LookForPlayer()
    {
        Vector3 targetPos = new Vector3(0, 0, 1);
        targetPos.z += _detectPlayerDistance * _direction;

        RaycastHit hit;
        RaycastHit hit2;

        bool ray1 = Physics.Raycast(transform.position, targetPos, out hit, Mathf.Infinity);
        bool ray2 = Physics.Raycast(transform.position, -targetPos, out hit2, Mathf.Infinity);

        Debug.DrawRay(transform.position, -targetPos, Color.red, 1);
        Debug.DrawRay(transform.position, targetPos, Color.yellow, 1);

        PlayerController player = hit.transform.gameObject.GetComponent<PlayerController>();
        PlayerController player2 = hit2.transform.gameObject.GetComponent<PlayerController>();

        if (ray1)
        {
            player = hit.transform.gameObject.GetComponent<PlayerController>();
            CanSeePlayer(true);

            if(player != null)
            {
                _canSeePlayer = true;
                _player = player.gameObject;
            }
        }

        if(ray2)
        {
            player2 = hit2.transform.gameObject.GetComponent<PlayerController>();
            CanSeePlayer(true);

            if(player2 != null)
            {
                _canSeePlayer = true;
                _player = player2.gameObject;
            }
        }

        if(!ray1 && !ray2)
        {
            if(_canSeePlayer)
            {
                CanSeePlayer(false);
            }
        }
        else if(player == null && player2 == null)
        {
            CanSeePlayer(false);
        }
    }

    protected virtual void CanSeePlayer(bool canSee)
    {
        _canSeePlayer = canSee;
    }
}
