using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private GameObject _tutorialScreen;
    [SerializeField] private Button _closeButton;

    private bool _isShowingTutorial = false;

    private void Start()
    {
        SaveData data = SaveSystem.Load();

        if(data.tutorial)
        {
            data.tutorial = false;
            SaveSystem.Save();
            _closeButton.onClick.AddListener(CloseMenu);
            Cursor.lockState = CursorLockMode.None;
            _tutorialScreen.SetActive(true);
            Time.timeScale = 0;
            _timerUI.Pause();
        }
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
    }

    private void CloseMenu()
    {
        _timerUI.UnPause();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        _tutorialScreen.SetActive(false);
    }
}
