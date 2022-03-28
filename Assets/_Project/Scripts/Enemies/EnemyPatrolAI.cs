using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rayDistance;
    [SerializeField] private Transform _rayPos;
    private Rigidbody _rb;
    private bool _isPatrolling = true;
    private int _direction = 1;
    private Quaternion _enemyRotationLeft;
    private Quaternion _enemyRotationRight;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _enemyRotationLeft = Quaternion.Euler(0, 180, 0);
        _enemyRotationRight = Quaternion.Euler(0, 0, 0);
    }


    private void FixedUpdate()
    {
        _rb.velocity += new Vector3(_rb.velocity.x, _rb.velocity.y, _moveSpeed * _direction);

        if(IsHittingWall() || IsNearEdge())
        {
            if(_direction == 1)
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

        Debug.DrawLine(_rayPos.position, targetPos, Color.red);

        if(Physics.Linecast(_rayPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
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
}
