using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager sInstance;

    public LevelData levelData;

    private void Awake()
    {
        SaveSystem.Load();

        if (sInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            sInstance = this;
        }
    }


}
