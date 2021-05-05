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
        PauseInputStart();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
        PauseInputDestroy();
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.playerController.OnPlayerInput += ReceiveInputs;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.playerController.OnPlayerInput -= ReceiveInputs;
    }

    private void PauseInputStart()
    {
        GameManager.sInstance.inGameMenu.OnPause += PauseAnimation;
    }

    private void PauseInputDestroy()
    {
        GameManager.sInstance.inGameMenu.OnPause -= PauseAnimation;
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
    }

    private void SetIsGrounded(bool isGrounded)
    {
        _animator.SetBool("isGrounded", isGrounded);
        
        if(isGrounded == false && _animator.GetBool("isJumping") == false)
        {
            Falling(true);
        }
        else
        {
            Falling(false);
        }
        
        if (isGrounded)
        {
            _animator.SetBool("isJumping", false);
        }
    }
    
    private void Falling(bool isFalling)
    {
        _animator.SetBool("isFalling", isFalling);
    }
    

    private void Movement(float direction, float velocity)
    {
        if (direction != 0)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }

        _animator.SetFloat("Velocity", velocity);
    }

    private void Jump(bool isJumping)
    {
        _animator.SetBool("isJumping", isJumping); 
    }

    private void DoubleJump(bool isDoubleJumping)
    {
       _animator.SetBool("isDoubleJumping", isDoubleJumping);
    }

}