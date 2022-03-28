using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectsController : MonoBehaviour
{
    [SerializeField] private AudioClip _finishAudio;
    [SerializeField] private AudioClip _gameOverAudio;
    [SerializeField] private AudioClip _coinAudio;
    [SerializeField] private AudioClip _checkpointAudio;

    [SerializeField] private AudioMixerGroup _effectsAudioMixer;
    [SerializeField] private AudioMixerGroup _finishAudioMixer;

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private GameObject _player;

    private ObjectPooler _objectPooler;

    private AudioSource _audioSource;


    void Start()
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
        _audioMixer.SetFloat("effectsVolume", SaveSystem.localData.soundfxVolume);
        _audioMixer.SetFloat("levelVolume", 0);
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.OnFinish += Finish;
        GameManager.sInstance.OnGameOver += GameOver;
        GameManager.sInstance.GetCheckPointManager().OnCheckpoint += CheckPoint;
        Colletables.OnCollectItem += CollectItem;
        GameManager.sInstance.GetInGameMenu().OnPause += PauseAudio;
        GameManager.sInstance.GetSettingsMenu().OnSetEffectsVolume += SetEffectsVolume;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.OnFinish -= Finish;
        GameManager.sInstance.OnGameOver -= GameOver;
        GameManager.sInstance.GetCheckPointManager().OnCheckpoint -= CheckPoint;
        Colletables.OnCollectItem -= CollectItem;
        GameManager.sInstance.GetInGameMenu().OnPause -= PauseAudio;
        GameManager.sInstance.GetSettingsMenu().OnSetEffectsVolume += SetEffectsVolume;
    }

    private void SetEffectsVolume(float volume)
    {
        _audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
    }

    private void PauseAudio(bool isPaused)
    {
        if (isPaused)
        {
            _audioMixer.SetFloat("levelVolume", -80);
        }
        else
        {
            _audioMixer.SetFloat("levelVolume", 0);
        }
    }

    private void CheckPoint(int checkpoint)
    {
        PlayAudio(_checkpointAudio, 1, _effectsAudioMixer);
    }

    private void CollectItem(int points)
    {
        PlayAudio(_coinAudio, 1, _effectsAudioMixer);
    }

    private void GameOver()
    {
        PlayAudio(_gameOverAudio, 0.5f, _effectsAudioMixer);
    }

    private void Finish()
    {
        PlayAudio(_finishAudio, 0.5f, _finishAudioMixer);
    }

    private void PlayAudio(AudioClip audio, float volume, AudioMixerGroup audioMixer)
    {
        GameObject obj = _objectPooler.SpawnFromPool(1);
        _audioSource = obj.GetComponent<AudioSource>();
        obj.transform.position = _player.transform.position;

        _audioSource.outputAudioMixerGroup = audioMixer;

        _audioSource.clip = audio;
        _audioSource.volume = volume;

        obj.GetComponent<AudioSourceObject>().Disable();

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
