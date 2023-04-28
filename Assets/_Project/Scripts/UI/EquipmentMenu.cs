using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EquipmentMenu : MonoBehaviour
{
    [SerializeField] private List<WeaponsUI> _weaponUIList = new List<WeaponsUI>();
    [SerializeField] private TextMeshProUGUI _coinsText;

    private void OnEnable()
    {
        UpdateCoins();
    }

    public void UpdateCoins()
    {
        SaveData data = SaveSystem.Load();
        _coinsText.text = data.coins.ToString();
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