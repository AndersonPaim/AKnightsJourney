using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _dashParticle;
    [SerializeField] private ParticleSystem _landingParticle;
    [SerializeField] private ParticleSystem _runParticle;
    [SerializeField] private ParticleSystem _jumpParticle;
    [SerializeField] private ParticleSystem _doubleJumpParticle;
    [SerializeField] private ParticleSystem _damageParticle;

    [SerializeField] private float _runningEmissionRate;

    private bool _isJumping = false;
    private bool _isDoubleJumping = false;
    private bool _isDashing = false;

    private void Start()
    {
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.GetPlayerController().OnPlayerInput += ReceiveInputs;
        GameManager.sInstance.GetPlayerController().OnTakeDamage += DamageParticle;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.GetPlayerController().OnPlayerInput -= ReceiveInputs;
        GameManager.sInstance.GetPlayerController().OnTakeDamage -= DamageParticle;
    }

    private void ReceiveInputs(PlayerData playerData)
    {
        Running(playerData.Velocity, playerData.OnGround);
        Dash(playerData.Dash);
        Jump(playerData.Jump);
        DoubleJump(playerData.DoubleJump);
    }

    private void Dash(bool isDashing)
    {
        _isDashing = isDashing;

        if (isDashing)
        {
            _dashParticle.Play();
            _runParticle.Play();
        }
    }

    private void Jump(bool isJumping)
    {
        if (isJumping && !_isJumping)
        {
            _jumpParticle.Play();
        }

        _isJumping = isJumping;
    }

    private void DoubleJump(bool isDoubleJumping)
    {
        if (isDoubleJumping && !_isDoubleJumping)
        {
            _doubleJumpParticle.Play();
        }
        _isDoubleJumping = isDoubleJumping;
    }

    private void Running(float velocity, bool isGrounded)
    {
        ParticleSystem.EmissionModule emission = _runParticle.emission;

        if (velocity > 0 && isGrounded)
        {
            if (!_runParticle.isPlaying)
            {
                _runParticle.Play();
            }
            else
            {
                emission.rateOverTime = velocity * _runningEmissionRate;
            }
        }
        else if(_isDashing && isGrounded)
        {
            _runParticle.Play();
        }
        else
        {
            _runParticle.Stop();
        }
    }

    private void LandingParticle() //chama no evento de anima��o
    {
        _landingParticle.Play();
    }

    private void DamageParticle()
    {
        _damageParticle.Play();
    }

}
