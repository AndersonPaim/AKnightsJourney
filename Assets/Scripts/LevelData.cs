using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public delegate void SetLevelIndexHandler(LevelSettings settings);
    public SetLevelIndexHandler OnSetLevelSettings;

    [SerializeField] private LevelSettings[] _levelSettings;

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
            OnSetLevelSettings?.Invoke(_levelSettings[i]);
        }
    }
}
