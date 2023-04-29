using UnityEngine;
using Coimbra.Services;

namespace _Project.Scripts.Managers
{
    public interface IAudioPlayer : IService
    {
        void PlayAudio(SoundEffect audio, Vector3 position);
    }
}