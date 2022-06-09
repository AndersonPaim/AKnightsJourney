using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public delegate void LoadingProgressHandler(float progress);
    public static LoadingProgressHandler OnUpdateProgress;

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

    private static IEnumerator LoadASync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while(!operation.isDone) //update loading screen progress while is loading
        {
            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f); //convert progress to % numbers
            OnUpdateProgress?.Invoke(loadingProgress);

            yield return null;
        }
    }
}
