using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{

    public delegate void SetRespawnPositionHandler(Vector3 pos);
    public SetRespawnPositionHandler OnSetRespawnPosition;

    public delegate void CheckpointHandler(int currentCheckpoint);
    public CheckpointHandler OnCheckpoint;

    public delegate void LastCheckpointHandler();
    public LastCheckpointHandler OnLastCheckpoint;

    [SerializeField] private int _currentCheckpoint = -1;

    [SerializeField] private GameObject[] _checkpoints;

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
        GameManager.sInstance.GetPlayerController().OnSetCheckpoint += SetCheckpoint;
        GameManager.sInstance.OnGetRespawnPosition += SetRespawnPosition;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.GetPlayerController().OnSetCheckpoint -= SetCheckpoint;
        GameManager.sInstance.OnGetRespawnPosition -= SetRespawnPosition;
    }

    private void SetRespawnPosition()
    {
        OnSetRespawnPosition?.Invoke(_checkpoints[_currentCheckpoint].transform.position);
    }

    private void SetCheckpoint()
    {
        _currentCheckpoint++;

        OnCheckpoint?.Invoke(_currentCheckpoint);

        if (_currentCheckpoint >= _checkpoints.Length - 1)
        {
            OnLastCheckpoint?.Invoke();
        }
    }
}
