using System.Collections;
using System.Collections.Generic;
using Coimbra;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    [SerializeField] private LevelSettings[] _levelSettings;
    [SerializeField] private SceneController _sceneController;

    [SerializeField] private GameObject _levelObject;
    [SerializeField] private GameObject _gridObject;

    private List<GameObject> _levels = new List<GameObject>();

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(_levels.Count > 0)
        {
            for (int i = 0; i < _levelSettings.Length; i++)
            {
                _levels[i].Dispose(true);
            }

            _levels.Clear();
        }


        SaveSystem.levelCount = _levelSettings.Length;

        for (int i = 0; i < _levelSettings.Length; i++)
        {
            GameObject levelObject = Instantiate(_levelObject);
            levelObject.transform.parent = _gridObject.transform;
            LevelHighscore levelHighscore = levelObject.GetComponent<LevelHighscore>();
            levelHighscore.Setup(_levelSettings[i], _sceneController);
            _levels.Add(levelObject);
        }
    }
}
