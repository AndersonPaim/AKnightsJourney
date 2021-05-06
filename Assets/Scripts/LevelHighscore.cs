using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelHighscore : MonoBehaviour
{

    /// peguei no pulo... cada o "_" na frente ? :p

    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;

    [SerializeField] private TextMeshProUGUI _levelNumberText;

    private LevelSettings _levelSettings;

    void Awake()
    {
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        MainMenuManager.sInstance.levelData.OnSetLevelSettings += SetLevelSettings;
    }

    private void RemoveDelegates()
    {
        MainMenuManager.sInstance.levelData.OnSetLevelSettings -= SetLevelSettings;
    }

    private void SetLevelSettings(LevelSettings levelSettings)
    {
        MainMenuManager.sInstance.levelData.OnSetLevelSettings -= SetLevelSettings;
        _levelSettings = levelSettings;

        SetLevelNumber();
        LoadHighscore();
    }

    private void SetLevelNumber()
    {
        _levelNumberText.text = _levelSettings.levelNumber.ToString();
    }

    public void SetScene()
    {
        SceneController.SetScene(_levelSettings.levelScene);
    }

    private void LoadHighscore()
    {
        switch (SaveSystem.localData.stars[_levelSettings.levelNumber - 1])
        {
            case 1:
                star1.SetActive(true);
                break;
            case 2:
                star1.SetActive(true);
                star2.SetActive(true);
                break;
            case 3:
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(true);
                break;
         }
    }
}
