using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;

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
       _animator.SetBool(AnimationParameters.ATTACK, isAttacking);
    }

    private void Dash(bool isDashing)
    {
       _animator.SetBool(AnimationParameters.DASH, isDashing);
    }
}