using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private TextMeshProUGUI _loadingProgressText;

    private void Start()
    {
        SetupDelegates();
    }

    private void OnDestroy()
    {
        DestroyDelegates();
    }

    private void SetupDelegates()
    {
        SceneController.OnUpdateProgress += UpdateLoadingProgress;
        SceneController.OnStartLoading += StartLoading;
        SceneController.OnFinishLoading += FinishLoading;
    }

    private void DestroyDelegates()
    {
        SceneController.OnUpdateProgress -= UpdateLoadingProgress;
        SceneController.OnStartLoading -= StartLoading;
        SceneController.OnFinishLoading -= FinishLoading;
    }

    private void UpdateLoadingProgress(float progress)
    {
        Debug.Log("PROGRESS: " + progress);
        _loadingBar.value = progress * 100;
        _loadingProgressText.text = "Loading..." + ((int)(progress * 100)).ToString() + "%" ;
    }

    private void StartLoading()
    {
        _loadingScreen.SetActive(true);
    }

    private void FinishLoading()
    {
        _loadingScreen.SetActive(false);
    }
}
