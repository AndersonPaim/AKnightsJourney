using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelSettings", menuName = "LevelSettings")]
public class LevelSettings : ScriptableObject
{
    [Tooltip("the name of the scene to be loaded")]
    public string levelScene;
    [Tooltip("the number of the level to be showed on the ui")]
    public int levelNumber;
    public bool isBoss;
}
