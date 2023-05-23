using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class LevelHighscore : MonoBehaviour
{
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;
    [SerializeField] private GameObject _starsBg;
    [SerializeField] private GameObject _bossIcon;

    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _fastestText;

    private LevelSettings _levelSettings;
    private SceneController _sceneController;
    private SaveData _data;

    public void Setup(LevelSettings levelSettings, SceneController sceneController)
    {
        Debug.Log("SETUP LEVEL");
        _levelSettings = levelSettings;
        _sceneController = sceneController;
        _data = SaveSystem.localData;

        if(levelSettings.isBoss)
        {
            _bossIcon.SetActive(true);
            _starsBg.SetActive(false);
        }

        SetLevelNumber();
        LoadHighscore();
        //gameObject.transform.DOScale(Vector3.one, 0.2f);
    }

    private void OnDisable()
    {
        //gameObject.transform.DOScale(Vector3.zero, 0.2f);
    }

    private void SetLevelNumber()
    {
        _levelNumberText.text = _levelSettings.levelNumber.ToString();
    }

    public void SetScene()
    {
        _sceneController.SetScene(_levelSettings.levelScene);
    }

    private void LoadHighscore()
    {
        if(SaveSystem.localData.stars.Count > _levelSettings.levelNumber)
        {
            TimeSpan time = _data.time[_levelSettings.levelNumber - 1];
            string timeText = string.Format("{0:00}:{1:00}:{2:000}", time.Minutes, time.Seconds, time.Milliseconds);
            _fastestText.text = timeText;

            switch (_data.stars[_levelSettings.levelNumber - 1])
            {
                case 1:
                    _star1.SetActive(true);
                    break;
                case 2:
                    _star1.SetActive(true);
                    _star2.SetActive(true);
                    break;
                case 3:
                    _star1.SetActive(true);
                    _star2.SetActive(true);
                    _star3.SetActive(true);
                    break;
            }
        }
    }
}
