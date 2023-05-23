using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class BeeBoss : MonoBehaviour, IDamageable
{
    public static Action OnFinish;

    public delegate void BossHPHandler(float health);
    public BossHPHandler OnStartBossHP;
    public BossHPHandler OnUpdateBossHP;

    public enum bossState
    {
        Idle,
        Attacking,
        Shoot,
        Returning,
        AttackCooldown,
        Dead,
        None,
    }

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _damageCollider;
    [SerializeField] private GameObject _projetileObject;
    [SerializeField] private List<Transform> _idlePosition;
    [SerializeField] private List<Transform> _projectilePosList;
    [SerializeField] private float _health;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _idleTime;
    [SerializeField] private float _shootForce;
    [SerializeField] private Animator _screenFadeAnim;
    [SerializeField] private SceneController _sceneController;

    private float _direction = -1;
    private bool _isAttacking = false;
    private Vector3 _attackPos;
    private bossState _bossState = bossState.None;
    private Animator _anim;
    private Rigidbody _rb;

    public bool IsVulnerable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void TakeDamage(float damage, GameObject attacker)
    {
        _health -= damage;

        if(_health <= 0)
        {
            ChangeState(bossState.Dead);
            _anim.SetTrigger("Die");
            GetComponent<Collider>().isTrigger = false;
            _rb.useGravity = true;
            StartCoroutine(FinishLevel());
        }

        OnUpdateBossHP?.Invoke(_health);
    }

    private void Shoot()
    {
        _anim.SetTrigger("Attack");

        foreach(Transform pos in _projectilePosList)
        {
            GameObject obj = Instantiate(_projetileObject, pos.position, pos.rotation);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * _shootForce);
        }

        //_audioPlayer.PlayAudio(_attackSFX, transform.position);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        StartCoroutine(AttackCooldown());

        OnStartBossHP?.Invoke(_health);
    }

    private void Attack()
    {
        if(_bossState == bossState.Dead)
        {
            return;
        }

        _attackPos = _player.transform.position;
        _bossState = bossState.Attacking;

        _anim.SetBool("isIdle", false);
        _anim.SetBool("isAttacking", true);
        _damageCollider.SetActive(true);

        transform.DOMove(_attackPos, _attackSpeed).OnComplete(() => { ChangeState(bossState.AttackCooldown); });
    }

    private IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(2);
        OnFinish?.Invoke();
    }

    private IEnumerator AttackCooldown()
    {
        _damageCollider.SetActive(false);
        _anim.SetBool("isAttacking", false);
        _anim.SetBool("isIdle", true);
        yield return new WaitForSeconds(_attackCooldown);

        if(_bossState != bossState.Dead)
        {
            ChangeState(bossState.Returning);
        }
    }

    private IEnumerator Idle()
    {
        Shoot();
        _anim.SetBool("isMoving", false);
        _anim.SetBool("isIdle", true);
        yield return new WaitForSeconds(_idleTime);

        if(_bossState != bossState.Dead)
        {
            ChangeState(bossState.Attacking);
        }
    }

    private void Returning()
    {
        if(_bossState == bossState.Dead)
        {
            return;
        }

        _anim.SetBool("isAttacking", false);
        _anim.SetBool("isIdle", false);
        _anim.SetBool("isMoving", true);
        int pos = UnityEngine.Random.Range(0, 4);
        _attackPos = _idlePosition[pos].position;
        RotateBee(pos);

        transform.DOMove(_attackPos, _attackSpeed).OnComplete(delegate {ChangeState(bossState.Idle);});
    }

    private async Task RotateBee(int pos)
    {
        await Task.Delay(1800);

        Quaternion rotation;

        if(pos == 1 || pos == 3)
        {
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            rotation = Quaternion.Euler(0, 0, 0);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);
    }

    private void ChangeState(bossState state)
    {
        _bossState = state;

        if(state == bossState.AttackCooldown)
        {
            StartCoroutine(AttackCooldown());
        }
        else if(state == bossState.Returning)
        {
            Returning();
        }
        else if(state == bossState.Idle)
        {
            StartCoroutine(Idle());
        }
        else if(state == bossState.Attacking)
        {
            Attack();
        }
    }
}
