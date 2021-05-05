using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public delegate void SetEffectsVolumeHandler(float volume);
    public SetEffectsVolumeHandler OnSetEffectsVolume;

    public delegate void SetMusicVolumeHandler(float volume);
    public SetMusicVolumeHandler OnSetMusicVolume;

    [SerializeField] private Slider _soundfxSlider;
    [SerializeField] private Slider _musicSlider;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SaveData data = SaveSystem.localData;
        _soundfxSlider.value = data.soundfxVolume;
        _musicSlider.value = data.musicVolume;
    }

    public void SoundfxVolume(float volume)
    {
        SaveSystem.localData.soundfxVolume = volume;
        OnSetEffectsVolume?.Invoke(volume);
    }

    public void MusicVolume(float volume)
    {
        SaveSystem.localData.musicVolume = volume;
        OnSetMusicVolume?.Invoke(volume);
    }

    public void Save()
    {
        SaveSystem.Save();
    }
}
