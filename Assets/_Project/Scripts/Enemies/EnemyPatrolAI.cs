using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _detectPlayerDistance;
    [SerializeField] private Transform _rayPos;
    private Rigidbody _rb;
    private bool _isPatrolling = true;
    private int _direction = 1;
    private Quaternion _enemyRotationLeft;
    private Quaternion _enemyRotationRight;

    protected GameObject _player;
    protected bool _canSeePlayer = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _enemyRotationLeft = Quaternion.Euler(0, 180, 0);
        _enemyRotationRight = Quaternion.Euler(0, 0, 0);
    }


    protected virtual void FixedUpdate()
    {
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

        _rb.velocity += transform.forward * _moveSpeed;
    }

    private bool IsHittingWall()
    {
        Vector3 targetPos = _rayPos.position;
        targetPos.z += _rayDistance * _direction;

        if(Physics.Linecast(_rayPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("HIT WALL");
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

        Debug.Log("IS NEAR EDGE: " + ground + " : " + player);

        if(ground || player)
        {
            return false;
        }
        else
        {
            Debug.Log("NEAR EDGE: ");
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
            _canSeePlayer = true;
            if(player != null)
            {
                Debug.Log("FIND PLAYER: FRENTE");

                _canSeePlayer = true;
                _player = player.gameObject;
            }
        }

        if(ray2)
        {
            player2 = hit2.transform.gameObject.GetComponent<PlayerController>();
            _canSeePlayer = true;
            if(player2 != null)
            {
                Debug.Log("FIND PLAYER: COSTAS");
                _canSeePlayer = true;
                _player = player2.gameObject;
            }
        }

        if(!ray1 && !ray2)
        {
            if(_canSeePlayer)
            {
                Debug.Log("CANT SEE PLAYER");
                _canSeePlayer = false;
            }
        }
        else if(player == null && player2 == null)
        {
            _canSeePlayer = false;
        }
    }
}
