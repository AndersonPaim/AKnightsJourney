using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<float> stars = new List<float>();
    //audio
    public float musicVolume = 0.8f;
    public float soundfxVolume = 0.8f;
    //weapons
    public List<bool> weaponsUnlocked = new List<bool>();
    public int weaponEquiped = 0;
}

