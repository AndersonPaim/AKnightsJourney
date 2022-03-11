using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _controlsMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _levelSelection;
    [SerializeField] private GameObject _settingsMenu;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayButton()
    {
        _mainMenu.SetActive(false);
        _levelSelection.SetActive(true);
    }

    public void ControlsButton()
    {
        _controlsMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void SettingsButton()
    {
        _settingsMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void BackButton()
    {
        _levelSelection.SetActive(false);
        _settingsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LevelSelection(string scene)
    {
        SceneController.SetScene(scene);
    }
}
