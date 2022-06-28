using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWeaponsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons = new List<GameObject>();

    public void SetSword()
    {
        foreach(GameObject weapon in _weapons)
        {
            weapon.SetActive(false);
        }

        SaveData data = SaveSystem.Load();
        _weapons[data.weaponEquiped].SetActive(true);
    }

    private void Start()
    {
        SetSword();
    }


}