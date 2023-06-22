using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _bgMusic;

    [SerializeField] private SettingsMenu _settingsMenu;

    [SerializeField] private AudioMixer _audioMixer;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SaveData data = SaveSystem.Load();
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(data.musicVolume) * 20);
        PlayMusic(_bgMusic);
    }

    private void PlayMusic(AudioClip musicClip)
    {
        _audioSource.clip = musicClip;

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
