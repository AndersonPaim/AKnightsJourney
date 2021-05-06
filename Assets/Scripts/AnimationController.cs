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
        /// Ao invés de vc mandar o parâmetro como string, crie uma classe estatica, que guarda os parâmetros do animator (normalmente chamada de AnimationParameters)...
        /// ex:
        /// Public const string ISGROUNDED = "isGrounded"
        ///
        /// e dai tu pode acessar esse parâmetro assim:     _animator.SetBool(AnimationParamaters.ISGROUNDED, isGrounded);
        /// assim tu evita de escrever o parametro errado e, se tu por ventura mudar o nome do parâmetro é só tu mudar nessa classe estatica
        
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