using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ShopItemUI : MonoBehaviour
{
    public static Action OnBuyWeapon;

    [SerializeField] private TextMeshProUGUI _weaponsPrice;
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private EquipmentMenu _equipmentMenu;
    private WeaponData _weaponData;
    private int _itemID;
    private float _price;

    public void SetupUI(WeaponShopData data)
    {
        _weaponsPrice.text = data.price.ToString();
        _weaponName.text = data.itemName;
        _image.sprite = data.image;
        _itemID = data.itemID;
        _price = data.price;

        _button.onClick.AddListener(BuyWeapon);
    }

    public void BuyWeapon()
    {
        if(!CanBuy())
        {
            return;
        }

        Debug.Log("CAN BUY");

        SaveData data = SaveSystem.localData;
        data.weaponsUnlocked[_itemID] = true;
        data.coins -= _price;
        SaveSystem.Save();
        _weaponsPrice.text = "UNLOCKED";
        _button.onClick.RemoveAllListeners();
        OnBuyWeapon?.Invoke();
    }

    private bool CanBuy()
    {
        SaveData data = SaveSystem.Load();

        if(data.coins >= _price)
        {
            return true;
        }
        else
        {
            Debug.Log("CANT BUY");
            return false;
        }
    }
}