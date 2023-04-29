using Coimbra.Services.Events;
using UnityEngine;
using UnityEngine.Audio;
using Coimbra;

namespace _Project.Scripts.Managers
{
    public class AudioManager : IAudioPlayer
    {
        public static AudioManager Instance;

        [SerializeField] private AudioMixer _gameAudioMixer;

        public void PlayAudio(SoundEffect soundEffect, Vector3 position)
        {
            AudioSource audioSource;

            GameObject obj = GameManager.sInstance.GetObjectPooler().SpawnFromPool(1);
            audioSource = obj.GetComponent<AudioSource>();
            obj.transform.position = position;

            audioSource.outputAudioMixerGroup = soundEffect.Mixer;
            audioSource.clip = soundEffect.Clip;
            audioSource.volume = soundEffect.Volume;
            obj.SetActive(false);
            obj.SetActive(true);

            if(soundEffect.Is3D)
            {
                audioSource.spatialBlend = 1;
            }
            else
            {
                audioSource.spatialBlend = 0;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                //DestroyImmediate(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            Initialize();
            SetupEvents();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void Initialize()
        {
            SaveData data = SaveSystem.Load();
            //_gameAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(data.SoundfxVolume) * 20);
            //_gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(data.MusicVolume) * 20);
        }

        private void SetupEvents()
        {
            //OnMusicVolumeUpdate.AddListener(MusicVolume);
            //OnEffectsVolumeUpdate.AddListener(EffectsVolume);
            GameManager.sInstance.GetInGameMenu().OnPause += PauseAudio;
        }

        private void DestroyEvents()
        {
            //OnMusicVolumeUpdate.RemoveAllListeners();
            //OnEffectsVolumeUpdate.RemoveAllListeners();
            GameManager.sInstance.GetInGameMenu().OnPause -= PauseAudio;
        }

        private void PauseAudio(bool isPaused)
        {
            if (isPaused)
            {
                _gameAudioMixer.SetFloat("MasterVolume", -80);
            }
            else
            {
                _gameAudioMixer.SetFloat("MasterVolume", 0);
            }
        }

        /*private void EffectsVolume(ref EventContext context, in OnEffectsVolumeUpdate e)
        {
            _gameAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(e.Volume) * 20);
            SaveData data = SaveSystem.LocalData;
            data.SoundfxVolume = e.Volume;
            SaveSystem.Save();
        }

        private void MusicVolume(ref EventContext context, in OnMusicVolumeUpdate e)
        {
            _gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(e.Volume) * 20);
            SaveData data = SaveSystem.LocalData;
            data.MusicVolume = e.Volume;
            SaveSystem.Save();
        }*/

        public void Dispose()
		{ }
    }
}