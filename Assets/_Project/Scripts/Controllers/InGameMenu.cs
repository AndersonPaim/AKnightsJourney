using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

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
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private AudioMixer _gameMixer;

    private bool _isPaused = false;
    private bool _canResume = true;
    private bool _settingsOpen = false;

    public void OpenSettings()
    {
        _settingsOpen = true;
        _gameMixer.SetFloat("effectsVolume", -80);
    }

    public void CloseSettings()
    {
        _settingsOpen = false;
    }

    private void Start()
    {
        SetupDelegates();
        Resume();

        SaveData data = SaveSystem.Load();
        _gameMixer.SetFloat("effectsVolume", Mathf.Log10(data.soundfxVolume) * 20);
        _gameMixer.SetFloat("musicVolume", Mathf.Log10(data.musicVolume) * 20);
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
        GameManager.sInstance.GetScoreManager().OnGetCoin += GetCoin;
        GameManager.sInstance.OnFinish += FinishScreen;
        GameManager.sInstance.OnGameOver += GameOver;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.GetInputListener().OnPause -= PauseInput;
        GameManager.sInstance.GetInputListener().OnBack -= BackInput;
        GameManager.sInstance.GetScoreManager().OnSetScore -= SetScore;
        GameManager.sInstance.GetScoreManager().OnGetCoin -= GetCoin;
        GameManager.sInstance.OnFinish += FinishScreen;
        GameManager.sInstance.OnGameOver -= GameOver;
    }

    private void GetCoin(float coins)
    {
        _coinsText.text = coins.ToString();
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
        _gameMixer.SetFloat("effectsVolume", -80);
        OnPause?.Invoke(true);
        _isPaused = true;
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameOver()
    {
        _gameMixer.SetFloat("effectsVolume", -80);
        OnPause?.Invoke(true);
        _canResume = false;
        Time.timeScale = 0;
        _gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void FinishScreen()
    {
        _gameMixer.SetFloat("effectsVolume", -80);
        OnPause?.Invoke(true);
        _canResume = false;
        Time.timeScale = 0;
        _finishMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        if(_settingsOpen)
        {
            _settingsMenu.SetActive(false);
            _settingsOpen = false;
            return;
        }

        _gameMixer.SetFloat("effectsVolume", 0);
        OnPause?.Invoke(false);
        _isPaused = false;
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
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

    public void SetScore(float stars, float kills, float killsTarget, float coins, float coinsTarget, float lifes, float lifesTarget)
    {
        _finishMenu.GetComponent<FinishScreenUI>().Setup(stars, kills, killsTarget, coins, coinsTarget, lifes, lifesTarget);

        switch (stars)
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
