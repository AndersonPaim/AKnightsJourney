using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WeaponsUI : MonoBehaviour
{
    [SerializeField] private GameObject _equipedObject;
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private EquipmentMenu _equipmentMenu;
    private WeaponData _weaponData;

    public void SetupUI(WeaponData weapon, bool isEquiped, EquipmentMenu equipmentMenu, bool unlocked)
    {
        if(!unlocked)
        {
            Destroy(_button.gameObject);
        }

        _equipmentMenu = equipmentMenu;
        _weaponData = weapon;

        SaveData data = SaveSystem.localData;

        foreach(bool weapons in data.weaponsUnlocked)
        {
            _weaponName.text = weapon.name;
            _image.sprite = weapon.image;

            if(isEquiped)
            {
                _equipedObject.SetActive(true);
            }
        }

        _button.onClick.AddListener(EquipWeapon);
    }

    public void EquipWeapon()
    {
        _equipedObject.SetActive(true);
        _equipmentMenu.EquipWeapon(_weaponData.weaponNumber);
    }

    public void DesequipWeapon()
    {
        _equipedObject.SetActive(false);
    }
}