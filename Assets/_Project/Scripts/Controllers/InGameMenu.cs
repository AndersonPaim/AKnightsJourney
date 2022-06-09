using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public delegate void PauseHandler(bool isPaused);
    public PauseHandler OnPause;

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _finishMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject[] _starBg;
    [SerializeField] private GameObject[] _star;

    private bool _isPaused = false;
    private bool _canResume = true;

    private void Start()
    {
        SetupDelegates();
        Resume();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.GetInputListener().OnPause += PauseInput;
        GameManager.sInstance.GetInputListener().OnBack += BackInput;
        GameManager.sInstance.GetScoreManager().OnSetScore += SetScore;
        GameManager.sInstance.OnFinish += Finish;
        GameManager.sInstance.OnGameOver += GameOver;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.GetInputListener().OnPause -= PauseInput;
        GameManager.sInstance.GetInputListener().OnBack -= BackInput;
        GameManager.sInstance.GetScoreManager().OnSetScore -= SetScore;
        GameManager.sInstance.OnFinish += Finish;
        GameManager.sInstance.OnGameOver -= GameOver;
    }

    private void PauseInput()
    {
        if (_isPaused == false && _canResume == true)
        {
            Pause();
        }
        else if (_isPaused == true && _canResume == true)
        {
            Resume();
        }
    }

    private void BackInput()
    {
        if(_isPaused && _canResume)
        {
            Resume();
        }
    }

    public void Pause()
    {
        OnPause?.Invoke(true);
        _isPaused = true;
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameOver()
    {
        OnPause?.Invoke(true);
        _canResume = false;
        Time.timeScale = 0;
        _gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Finish()
    {
        OnPause?.Invoke(true);
        _canResume = false;
        Time.timeScale = 0;
        _finishMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        OnPause?.Invoke(false);
        _isPaused = false;
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Settings()
    {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _canResume = false;
    }

    public void BackButton()
    {
        _pauseMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _canResume = true;
    }

    public void Restart()
    {
        GameManager.sInstance.GetSceneController().RestartScene();
    }

    public void Quit()
    {
        GameManager.sInstance.GetSceneController().SetScene("Menu");
    }

    public void SetScore(float starsScore)
    {
        switch (starsScore)
        {
            case 0:
                _starBg[0].SetActive(true);
                _starBg[1].SetActive(true);
                _starBg[2].SetActive(true);
                break;
            case 1:
                _star[0].SetActive(true);
                _starBg[1].SetActive(true);
                _starBg[2].SetActive(true);
                break;
            case 2:
                _star[0].SetActive(true);
                _star[1].SetActive(true);
                _starBg[2].SetActive(true);
                break;
            case 3:
                _star[0].SetActive(true);
                _star[1].SetActive(true);
                _star[2].SetActive(true);
                break;
        }
    }
}
