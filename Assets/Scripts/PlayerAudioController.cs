using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioController : MonoBehaviour
{

    [SerializeField] private AudioClip _walkAudio;
    [SerializeField] private AudioClip _runAudio;
    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private AudioClip _dashAudio;

    [SerializeField] private float _runSpeed;

    [SerializeField] private AudioSource _footstepsAudioSource;

    [SerializeField] private AudioMixer _audioMixer;

    private AudioSource _audioSource;

    private bool _isDashing = false;
    private bool _isJumping = false;
    private bool _isDoubleJumping = false;
    private bool _isPaused = false;

    private float _isMoving;

    private ObjectPooler _objectPooler;

    private void Start()
    {
        SetupDelegates();
        Initialize();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void Initialize()
    {
        _objectPooler = GameManager.sInstance.GetObjectPooler();
        _audioMixer.SetFloat("playerVolume", 0);
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.GetPlayerController().OnPlayerInput += ReceiveInputs;
        GameManager.sInstance.GetInGameMenu().OnPause += PauseAudio;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.GetPlayerController().OnPlayerInput -= ReceiveInputs;
        GameManager.sInstance.GetInGameMenu().OnPause -= PauseAudio;
    }

    private void PauseAudio(bool isPaused)
    {
        if (isPaused)
        {
            _audioMixer.SetFloat("playerVolume", -80);
        }
        else
        {
            _audioMixer.SetFloat("playerVolume", 0);
        }
    }

    private void ReceiveInputs(PlayerData playerData)
    {
        if (playerData.Dash && !_isDashing)
        {
            Dash();
        }
        if (playerData.Jump && !_isJumping)
        {
            Jump();
        }
        if (playerData.DoubleJump && !_isDoubleJumping)
        {
            Jump();
        }
        Movement(playerData.Velocity, playerData.OnGround);
        _isDashing = playerData.Dash;
        _isJumping = playerData.Jump;
        _isDoubleJumping = playerData.DoubleJump;
        _isMoving = playerData.Movement;
    }

    private void Landing()
    {
        if (_isMoving == 0)
        {
            PlayAudio(_runAudio, 1);
        }
    }

    private void Movement(float velocity, bool isGrounded)
    {
        if (velocity <= _runSpeed && velocity > 0 && isGrounded && !_isPaused)
        {
            PlayFootstepsAudio(_walkAudio, 1);
        }
        else if (velocity > _runSpeed && isGrounded && !_isPaused)
        {
            PlayFootstepsAudio(_runAudio, 1);
        }
    }

    private void Jump()
    {
        PlayAudio(_jumpAudio, 0.2f);
    }

    private void Dash()
    {
        PlayAudio(_dashAudio, 1);
    }

    private void PlayFootstepsAudio(AudioClip audio, float volume)
    {
        _footstepsAudioSource.clip = audio;
        _footstepsAudioSource.volume = volume;

        if (!_footstepsAudioSource.isPlaying)
        {
            _footstepsAudioSource.Play();
        }
    }

    private void PlayAudio(AudioClip audio, float volume)
    {
        GameObject obj = _objectPooler.SpawnFromPool(1);
        _audioSource = obj.GetComponent<AudioSource>();
        obj.transform.position = transform.position;

        _audioSource.clip = audio;
        _audioSource.volume = volume;

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}