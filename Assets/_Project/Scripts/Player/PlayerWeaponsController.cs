using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWeaponsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons = new List<GameObject>();

    private void Start()
    {
        SaveData data = SaveSystem.Load();

        _weapons[data.weaponEquiped].SetActive(true);
        Debug.Log("DATA: " + data.weaponsUnlocked);
    }
}