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
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        _settingsMenu.OnSetMusicVolume += SetMusicVolume;
    }

    private void RemoveDelegates()
    {
        _settingsMenu.OnSetMusicVolume -= SetMusicVolume;
    }

    private void Initialize()
    {
        _audioMixer.SetFloat("musicVolume", SaveSystem.localData.musicVolume);

        PlayMusic(_bgMusic);
    }

    private void SetMusicVolume(float volume)
    {
        /// --> Sliders do volume, apenas ao mexer um pouquinho o volume cai completamente (sei que a unity trata os valores
        ///de -80 a +20 mas então meu garoto, tu precisa pensar em um jeito de tornar esse slider  mais suave....
       ///     metade do slider deve ser metade do volume )
       
        _audioMixer.SetFloat("musicVolume", volume);
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
