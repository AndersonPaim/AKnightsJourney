using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Managers;
using Coimbra.Services;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _soundfxSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private AudioMixer _gameMixer;

    private IAudioPlayer _audioPlayer;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _audioPlayer = ServiceLocator.Get<IAudioPlayer>();

        SaveData data = SaveSystem.Load();
        _soundfxSlider.value = data.soundfxVolume;
        _musicSlider.value = data.musicVolume;
    }

    public void SoundfxVolume(float volume)
    {
        SaveSystem.localData.soundfxVolume = volume;
        _gameMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        SaveSystem.Save();
    }

    public void MusicVolume(float volume)
    {
        SaveSystem.localData.musicVolume = volume;
        _gameMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        SaveSystem.Save();
    }

    public void Save()
    {
        SaveSystem.Save();
    }
}
