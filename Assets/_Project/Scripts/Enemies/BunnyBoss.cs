using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class BunnyBoss : MonoBehaviour, IDamageable
{
    public delegate void BossHPHandler(float health);
    public BossHPHandler OnStartBossHP;
    public BossHPHandler OnUpdateBossHP;

    public enum bossState
    {
        Idle,
        MeleeAttack,
        AerialAttack,
        Chasing,
        AttackCooldown,
        Dead,
        None,
    }

    [SerializeField] List<Transform> _projectilePosList = new List<Transform>();
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _carrot;
    [SerializeField] private float _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackDistance;
    [SerializeField] private Animator _screenFadeAnim;
    [SerializeField] private SceneController _sceneController;

    private float _direction = -1;
    private bool _isAttacking = false;
    private Vector3 _attackPos;
    private bossState _bossState = bossState.Idle;
    private Animator _anim;
    private Rigidbody _rb;
    private float _playerDistance;

    public bool IsVulnerable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void TakeDamage(float damage, GameObject attacker)
    {
        _health -= damage;

        if(_health <= 0)
        {
            ChangeState(bossState.Dead);
            _anim.SetTrigger("Death");
            GetComponent<Collider>().isTrigger = false;
            _rb.useGravity = true;
            StartCoroutine(FinishLevel());
        }

        OnUpdateBossHP?.Invoke(_health);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        StartCoroutine(Idle());

        OnStartBossHP?.Invoke(_health);
    }

    private void Update()
    {
        Vector3 look = new Vector3(transform.position.x, 0,  _player.transform.position.z);
        transform.LookAt(look);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        _playerDistance = Vector3.Distance(_player.transform.position, transform.position);

        if(_bossState == bossState.Chasing)
        {
            _rb.velocity += transform.forward * _moveSpeed * Time.deltaTime;

            if(_playerDistance < _attackDistance)
            {
                ChangeState(bossState.MeleeAttack);
                _anim.SetBool("IsWalking", false);
            }
        }
    }

    private void MeleeAttack()
    {
        if(_bossState == bossState.Dead)
        {
            return;
        }

        _bossState = bossState.MeleeAttack;

        _anim.SetBool("IsIdle", false);
        _anim.SetBool("IsAttackingMelee", true);

        ChangeState(bossState.AttackCooldown);
    }

    private IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(2);
        _screenFadeAnim.SetTrigger("Fade2");
        yield return new WaitForSeconds(3);
        //_sceneController.SetScene("Level6");
    }

    private IEnumerator AttackCooldown()
    {
        _bossState = bossState.AttackCooldown;
        //_anim.SetBool("IsAttackingMelee", false);
        _anim.SetBool("IsAttackingAerial", false);
        _anim.SetBool("IsIdle", true);
        yield return new WaitForSeconds(0);

        if(_bossState != bossState.Dead)
        {
            ChangeState(bossState.Idle);
        }
    }

    private IEnumerator Idle()
    {
        _bossState = bossState.Idle;
        _anim.SetBool("IsAttackingMelee", false);
        _anim.SetBool("IsWalking", false);
        _anim.SetBool("IsIdle", true);
        yield return new WaitForSeconds(0f);

        if(_bossState != bossState.Dead)
        {
            if(_playerDistance < _attackDistance)
            {
                ChangeState(bossState.MeleeAttack);
            }
            else
            {
                int randomBehaviour = Random.Range(1, 3);

                if(randomBehaviour == 1)
                {
                    ChangeState(bossState.AerialAttack);
                }
                else
                {
                    ChangeState(bossState.Chasing);
                }
            }
        }
    }

    private void Chasing()
    {
        _bossState = bossState.Chasing;
        _anim.SetBool("IsWalking", true);
    }

    private IEnumerator AerialAttack()
    {
        _bossState = bossState.AerialAttack;
        _anim.SetBool("IsIdle", false);
        _anim.SetBool("IsAttackingAerial", true);
        yield return new WaitForSeconds(1f);
        //aerial attack
        ChangeState(bossState.AttackCooldown);

        foreach(Transform pos in _projectilePosList)
        {
            Debug.Log("AERIAL ATTACK");
            GameObject carrot = Instantiate(_carrot);
            carrot.transform.position = pos.position;
        }
    }

    private void ChangeState(bossState state)
    {
        _bossState = state;

        if(state == bossState.AttackCooldown)
        {
            StartCoroutine(AttackCooldown());
        }
        else if(state == bossState.Chasing)
        {
            Chasing();
        }
        else if(state == bossState.Idle)
        {
            StartCoroutine(Idle());
        }
        else if(state == bossState.MeleeAttack)
        {
            MeleeAttack();
        }
        else if(state == bossState.AerialAttack)
        {
            StartCoroutine(AerialAttack());
        }
    }
}
