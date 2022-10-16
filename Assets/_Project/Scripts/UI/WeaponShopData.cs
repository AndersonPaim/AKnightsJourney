using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponShopData", menuName = "WeaponShopData")]
public class WeaponShopData : ScriptableObject
{
    public float price;
    public string itemName;
    public Sprite image;
    public int itemID;
}
