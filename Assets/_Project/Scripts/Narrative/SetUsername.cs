using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class SetUsername : MonoBehaviour
{
    [SerializeField] private Character _playerCharacter;

    private void Start()
    {
        SaveData data = SaveSystem.Load();
        _playerCharacter.SetStandardText(data.username);
    }
}
