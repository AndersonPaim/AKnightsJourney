using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private int _playerID;

    private void Start()
    {
        _playerID = GameManager.sInstance.GetPlayerController().transform.GetInstanceID();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetInstanceID() == _playerID)
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetInstanceID() == _playerID)
        {
            other.transform.parent = null;
        }
    }
}
