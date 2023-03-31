using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    [SerializeField] private LevelSettings[] _levelSettings;
    [SerializeField] private SceneController _sceneController;

    [SerializeField] private GameObject _levelObject;
    [SerializeField] private GameObject _gridObject;

    private void Start()
    {
        Initialize();
    }

    private void Initialize(){
        SaveSystem.levelCount = _levelSettings.Length;

        for (int i = 0; i < _levelSettings.Length; i++)
        {
            GameObject levelObject = Instantiate(_levelObject);
            levelObject.transform.parent = _gridObject.transform;
            LevelHighscore levelHighscore = levelObject.GetComponent<LevelHighscore>();
            levelHighscore.Setup(_levelSettings[i], _sceneController);
        }
    }
}
