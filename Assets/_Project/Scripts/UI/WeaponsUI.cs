using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class WeaponsUI : MonoBehaviour
{
    [SerializeField] private GameObject _equipedObject;
    [SerializeField] private GameObject _lockImage;
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private Image _image;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _equipButton;
    [SerializeField] private int _itemID;
    [SerializeField] private int _price;

    [SerializeField] private EquipmentMenu _equipmentMenu;
    private WeaponData _weaponData;

    private void Start()
    {
        Initialize();
        SetupEvents();
    }

    private void OnEnable()
    {
        //gameObject.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f);
    }

    private void OnDisable()
    {
        //gameObject.transform.DOScale(Vector3.zero, 0.2f);
    }

    private void Initialize()
    {
        SaveData data = SaveSystem.localData;

        if(data.weaponsUnlocked[_itemID])
        {
            _lockImage.SetActive(false);
        }
        else
        {
            _equipButton.gameObject.SetActive(false);

            if(data.coins < _price)
            {
                _buyButton.interactable = false;
            }
        }

        if(data.weaponEquiped == _itemID)
        {
            _equipedObject.SetActive(true);
        }
    }

    private void SetupEvents()
    {
        _buyButton.onClick.AddListener(BuyWeapon);
        _equipButton.onClick.AddListener(EquipWeapon);
    }

    private void EquipWeapon()
    {
        _equipedObject.SetActive(true);
        _equipmentMenu.EquipWeapon(_itemID);
    }

    public void DesequipWeapon()
    {
        _equipedObject.SetActive(false);
    }

    private void BuyWeapon()
    {
        SaveData data = SaveSystem.localData;
        data.weaponsUnlocked[_itemID] = true;
        data.coins -= _price;
        SaveSystem.Save();
        _lockImage.SetActive(false);
        _equipButton.gameObject.SetActive(true);
        _equipmentMenu.UpdateCoins();
        Initialize();
    }
}