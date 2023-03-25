using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;

    [SerializeField] private GameObject _controlsMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _levelSelection;
    [SerializeField] private GameObject _equipmentMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _shopMenu;
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayButton(string scene)
    {
        //_mainMenu.SetActive(false);
        //_levelSelection.SetActive(true);
        _sceneController.SetScene(scene);
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
        _equipmentMenu.SetActive(false);
        _shopMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void EquipmentButton()
    {
        _mainMenu.SetActive(false);
        _equipmentMenu.SetActive(true);
    }

    public void ShopButton()
    {
        _mainMenu.SetActive(false);
        _shopMenu.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LevelSelection(string scene)
    {
        _sceneController.SetScene(scene);
    }

    private void OnEnable()
    {
        SaveData data = SaveSystem.Load();
        _coinsText.text = data.coins.ToString();
    }
}
