using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static int currentScene;
    public static void SetScene(string scene)
    {
        SceneManager.LoadScene(scene);
        GetCurrentScene();
    }
    
    public static void GetCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentScene = scene.buildIndex;
    }

    public static void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SetScene(scene.name);
    }
}
