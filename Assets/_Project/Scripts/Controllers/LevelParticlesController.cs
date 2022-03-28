using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParticlesController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _checkpointParticle;
    [SerializeField] private ParticleSystem _respawnParticle;
    [SerializeField] private ParticleSystem _finishParticle;

    [SerializeField] private GameObject _finishGameObject;

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
        GameManager.sInstance.OnGetRespawnPosition += RespawnParticle;
        GameManager.sInstance.GetCheckPointManager().OnCheckpoint += CheckPointParticle;
        GameManager.sInstance.OnFinish += FinishParticle;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.OnGetRespawnPosition -= RespawnParticle;
        GameManager.sInstance.GetCheckPointManager().OnCheckpoint -= CheckPointParticle;
        GameManager.sInstance.OnFinish -= FinishParticle;
    }


    private void CheckPointParticle(int currentCheckpoint)
    {
        _checkpointParticle[currentCheckpoint].Play();
    }

    private void FinishParticle()
    {
        _finishGameObject.SetActive(true);
        _finishParticle.Play();
    }

    private void RespawnParticle()
    {
        _respawnParticle.Play();
    }

}
