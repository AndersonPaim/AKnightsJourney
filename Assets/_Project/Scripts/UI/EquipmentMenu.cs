using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EquipmentMenu : MonoBehaviour
{
    [SerializeField] private List<WeaponsUI> _weaponUIList = new List<WeaponsUI>();

    public void EquipWeapon(int weaponNumber)
    {
        Debug.Log("WEAPON EQUIPED: " + weaponNumber);
        SaveData data = SaveSystem.localData;
        _weaponUIList[data.weaponEquiped].DesequipWeapon();
        data.weaponEquiped = weaponNumber;
        SaveSystem.Save();
    }
}