using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EquipmentMenu : MonoBehaviour
{
    [SerializeField] private List<WeaponData> _weapons = new List<WeaponData>();
    [SerializeField] private WeaponsUI _weaponUI;
    [SerializeField] private Transform _weaponUIPosition;

    private List<WeaponsUI> _weaponUIList = new List<WeaponsUI>();

    private void Start()
    {
        SetupUI();
    }

    private void SetupUI()
    {
        SaveData data = SaveSystem.localData;

        for(int i = 0; i < data.weaponsUnlocked.Count; i++)
        {

            bool isEquiped = false;

            if(data.weaponEquiped == i)
            {
                isEquiped = true;
            }

            WeaponsUI weapon = Instantiate(_weaponUI, _weaponUIPosition);
            _weaponUIList.Add(weapon);

            if(data.weaponsUnlocked[i])
            {
                weapon.SetupUI(_weapons[i], isEquiped, this, true);
            }
            else
            {
                weapon.SetupUI(_weapons[i], isEquiped, this, false);
            }
        }
    }

    public void EquipWeapon(int weaponNumber)
    {
        Debug.Log("WEAPON EQUIPED: " + weaponNumber);
        SaveData data = SaveSystem.localData;
        _weaponUIList[data.weaponEquiped].DesequipWeapon();
        data.weaponEquiped = weaponNumber;
        SaveSystem.Save();
    }
}