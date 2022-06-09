using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static Action OnStartLoading;
    public static Action OnFinishLoading;
    public delegate void LoadingProgressHandler(float progress);
    public static LoadingProgressHandler OnUpdateProgress;

    public static int currentScene;
    public void SetScene(string scene)
    {
        OnStartLoading?.Invoke();
        StartCoroutine(LoadASync(scene));
        GetCurrentScene();
    }

    public void GetCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentScene = scene.buildIndex;
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SetScene(scene.name);
    }

    private IEnumerator LoadASync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while(!operation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);
            OnUpdateProgress?.Invoke(loadingProgress);

            yield return null;
        }

        OnFinishLoading?.Invoke();
    }
}
