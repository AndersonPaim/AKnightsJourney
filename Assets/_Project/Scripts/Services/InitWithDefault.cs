using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public class InitWithDefault : MonoBehaviour
{
    [SerializeField] private GameObject _usernamePopUp;
    [SerializeField] private GameObject _usernameMenu;
    [SerializeField] private Button _updateUsernameButton;
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TextMeshProUGUI _usernameText;

    private async void Awake()
    {
        Time.timeScale = 1;
        var options = new InitializationOptions();
        options.SetEnvironmentName("testing");
        await UnityServices.InitializeAsync(options);
        List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        await SignInAnonymouslyAsync();
        await SignInAnonymouslyAsync();

        _updateUsernameButton.onClick.AddListener(UpdateUsername);
    }

    private async Task SignInAnonymouslyAsync()
    {
        if(AuthenticationService.Instance.IsAuthorized)
        {
            return;
        }

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            PlayerInfo info = await AuthenticationService.Instance.GetPlayerInfoAsync();

            DateTime? createdAt = info.CreatedAt;
            DateTime currentDateTime = DateTime.UtcNow;
            TimeSpan? time = createdAt - currentDateTime;

            if(time.Value.TotalSeconds > -10)
            {
                ShowUsernamePopUp();
            }
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }

        _usernameText.text = await AuthenticationService.Instance.GetPlayerNameAsync();
    }

    public void ShowUsernamePopUp()
    {
        _usernamePopUp.SetActive(true);
        _usernameMenu.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f);
    }

    private void UpdateUsername()
    {
        UpdateUsernameAsync();
    }

    private async UniTask UpdateUsernameAsync()
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(_usernameInput.text);

        _usernameMenu.transform.DOScale(Vector3.zero, 0.2f).OnComplete
        (
            ()=>_usernamePopUp.SetActive(false)
        );

        _usernameText.text = await AuthenticationService.Instance.GetPlayerNameAsync();
    }
}