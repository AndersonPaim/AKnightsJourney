using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private PlayerLedgeGrab _playerLeadgeGrab;

    public delegate void AttackHandler(int attack);
    public AttackHandler OnStartAttack;
    public AttackHandler OnStopAttack;

    [SerializeField] private GameObject _hitCollider;
    private Animator _animator;

    private int _attackComboIndex = 1;

    public void StartAttack()
    {
        OnStartAttack?.Invoke(_attackComboIndex);
        StopAllCoroutines();

        _attackComboIndex++;

        if(_attackComboIndex > 4)
        {
            _attackComboIndex = 1;
        }
    }

    public void EnableAttackCollider()
    {
        _hitCollider.SetActive(true);
    }

    public void FinishClimbing()
    {
        _playerLeadgeGrab.Climbing();
    }

    public void DisableAttackCollider()
    {
        _hitCollider.SetActive(false);
        OnStopAttack?.Invoke(_attackComboIndex);
        StartCoroutine(StopAttackCombo());
    }

    private void Start()
    {
        Initialize();
        SetupDelegates();
        SetupPauseDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
        PauseInputDestroy();
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.GetPlayerController().OnPlayerInput += ReceiveInputs;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.GetPlayerController().OnPlayerInput -= ReceiveInputs;
    }

    private void SetupPauseDelegates()
    {
        GameManager.sInstance.GetInGameMenu().OnPause += PauseAnimation;
    }

    private void PauseInputDestroy()
    {
        GameManager.sInstance.GetInGameMenu().OnPause -= PauseAnimation;
    }

    private void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    private void PauseAnimation(bool isPaused)
    {
        if (isPaused)
        {
            RemoveDelegates();
        }
        else
        {
            SetupDelegates();
        }
    }
    private void ReceiveInputs(PlayerData playerData)
    {
        Movement(playerData.Movement, playerData.Velocity);
        SetIsGrounded(playerData.OnGround);
        Jump(playerData.Jump);
        DoubleJump(playerData.DoubleJump);
        Attack(playerData.Attack);
        Dash(playerData.Dash);
        Hanging(playerData.Hanging);
        Climb(playerData.Climbing);
    }

    private void SetIsGrounded(bool isGrounded)
    {
        _animator.SetBool(AnimationParameters.ISGROUNDED, isGrounded);

        if(isGrounded == false && _animator.GetBool(AnimationParameters.ISJUMPING) == false)
        {
            Falling(true);
        }
        else
        {
            Falling(false);
        }

        if (isGrounded)
        {
            _animator.SetBool(AnimationParameters.ISJUMPING, false);
        }
    }

    private void Falling(bool isFalling)
    {
        _animator.SetBool(AnimationParameters.ISFALLING, isFalling);
    }


    private void Movement(float direction, float velocity)
    {
        if (direction != 0)
        {
            _animator.SetBool(AnimationParameters.ISWALKING, true);
        }
        else
        {
            _animator.SetBool(AnimationParameters.ISWALKING, false);
        }

        _animator.SetFloat(AnimationParameters.VELOCITY, velocity);
    }

    private void Jump(bool isJumping)
    {
        _animator.SetBool(AnimationParameters.ISJUMPING, isJumping);
    }

    private void DoubleJump(bool isDoubleJumping)
    {
       _animator.SetBool(AnimationParameters.ISDOUBLEJUMPING, isDoubleJumping);
    }

    private void Attack(bool isAttacking)
    {
        if(isAttacking)
        {
            switch(_attackComboIndex)
            {
                case 1:
                    _animator.SetBool("isAttacking1", true);
                    _animator.SetBool("isAttacking2", false);
                    _animator.SetBool("isAttacking3", false);
                    _animator.SetBool("isAttacking4", false);
                    break;
                case 2:
                    _animator.SetBool("isAttacking1", false);
                    _animator.SetBool("isAttacking2", true);
                    _animator.SetBool("isAttacking3", false);
                    _animator.SetBool("isAttacking4", false);
                    break;
                case 3:
                    _animator.SetBool("isAttacking1", false);
                    _animator.SetBool("isAttacking2", false);
                    _animator.SetBool("isAttacking3", true);
                    _animator.SetBool("isAttacking4", false);
                    break;
                case 4:
                    _animator.SetBool("isAttacking1", false);
                    _animator.SetBool("isAttacking2", false);
                    _animator.SetBool("isAttacking3", false);
                    _animator.SetBool("isAttacking4", true);
                    break;
            }
        }
        else
        {
            _animator.SetBool("isAttacking1", false);
            _animator.SetBool("isAttacking2", false);
            _animator.SetBool("isAttacking3", false);
            _animator.SetBool("isAttacking4", false);
        }
    }

    private IEnumerator StopAttackCombo()
    {
        yield return new WaitForSeconds(1);
        _attackComboIndex = 1;
    }

    private void Dash(bool isDashing)
    {
       _animator.SetBool(AnimationParameters.DASH, isDashing);
    }

    private void Hanging(bool isHanging)
    {
       _animator.SetBool(AnimationParameters.HANGING, isHanging);
    }

    private void Climb(bool isClimbing)
    {
       _animator.SetBool(AnimationParameters.CLIMBING, isClimbing);
    }
}