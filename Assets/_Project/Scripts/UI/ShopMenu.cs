using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Coimbra;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private List<WeaponShopData> _weaponItensData = new List<WeaponShopData>();
    [SerializeField] private ShopItemUI _weaponUI;
    [SerializeField] private Transform _weaponUIPosition;
    [SerializeField] private TextMeshProUGUI _coinsText;

    private List<GameObject> _shopItensList = new List<GameObject>();

    private void OnEnable()
    {
        RefreshMenu();
    }

    private void OnDisable()
    {
        ShopItemUI.OnBuyWeapon -= RefreshMenu;
    }

    private void RefreshMenu()
    {
        foreach(GameObject item in _shopItensList)
        {
            DestroyImmediate(item);
        }

        UpdateCoinsUI();
        SetupUI();
    }

    private void SetupUI()
    {
        ShopItemUI.OnBuyWeapon += RefreshMenu;

        SaveData data = SaveSystem.localData;

        for(int i = 0; i < data.weaponsUnlocked.Count; i++)
        {
            if(!data.weaponsUnlocked[i])
            {
                ShopItemUI weapon = Instantiate(_weaponUI, _weaponUIPosition);
                _shopItensList.Add(weapon.gameObject);
                weapon.SetupUI(_weaponItensData[i - 1]);
            }
        }
    }

    private void UpdateCoinsUI()
    {
        SaveData data = SaveSystem.Load();
        _coinsText.text = data.coins.ToString();
    }
}
