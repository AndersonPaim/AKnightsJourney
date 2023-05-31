using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string username = "Player";
    public float coins = 0;
    public List<float> stars = new List<float>();
    public List<TimeSpan> time = new List<TimeSpan>();
    //audio
    public float musicVolume = 0.8f;
    public float soundfxVolume = 0.8f;
    //weapons
    public List<bool> weaponsUnlocked = new List<bool>();
    public int weaponEquiped = 0;
    public bool tutorial = true;
}

