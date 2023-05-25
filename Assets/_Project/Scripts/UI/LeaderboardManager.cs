using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;
using Coimbra;

public class LeaderboardsData
{
    public LeaderboardData[] leaderboard;
}

public class LeaderboardData
{
    public string playerId;
    public string playerName;
    public string rank;
    public string score;
}

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject _leaderboardPos;
    [SerializeField] private LeaderboardUI _myLeaderboard;
    [SerializeField] private LeaderboardUI _firstLeaderboard;
    [SerializeField] private LeaderboardUI _secondLeaderboard;
    [SerializeField] private LeaderboardUI _thirdLeaderboard;
    [SerializeField] private LeaderboardUI _othersLeaderboard;
    [SerializeField] private List<Button> _filterButtons = new List<Button>();
    [SerializeField] private Sprite _filterEnabledImage;
    [SerializeField] private Sprite _filterDisabledImage;

    private List<LeaderboardUI> _leaderboardList = new List<LeaderboardUI>();

    public void ChangeFilter(string filter)
    {
        foreach(Button button in _filterButtons)
        {
            button.GetComponent<Image>().sprite = _filterDisabledImage;
        }

        //clickedButton.GetComponent<Image>().sprite = _filterEnabledImage;

        if(filter == "LEVEL1")
        {
            _filterButtons[0].GetComponent<Image>().sprite = _filterEnabledImage;
        }
        else if(filter == "LEVEL2")
        {
            _filterButtons[1].GetComponent<Image>().sprite = _filterEnabledImage;
        }
        else if(filter == "LEVEL3")
        {
            _filterButtons[2].GetComponent<Image>().sprite = _filterEnabledImage;
        }
        else if(filter == "LEVEL4")
        {
            _filterButtons[3].GetComponent<Image>().sprite = _filterEnabledImage;
        }

        foreach(LeaderboardUI leaderboard in _leaderboardList)
        {
            leaderboard.gameObject.Dispose(false);
        }

        _leaderboardList.Clear();

        CreateLeaderboard(filter);
        GetPlayerScore(filter);
    }

    private void Start()
    {
        StartAsync();
    }

    private async Task StartAsync()
    {
        var options = new InitializationOptions();
        options.SetEnvironmentName("testing");
        await UnityServices.InitializeAsync(options);
        await CreateLeaderboard("LEVEL1");
        await GetPlayerScore("LEVEL1");
        //GetPlayerScore();
    }

    private async UniTask GetPlayerScore(string filter)
    {
        _myLeaderboard.Clear();
        LeaderboardEntry scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(filter);
        _myLeaderboard.Setup(scoreResponse);
    }

    private async UniTask CreateLeaderboard(string filter)
    {
        LeaderboardScoresPage scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(filter);

        foreach(LeaderboardEntry entry in scoresResponse.Results)
        {
            if(entry.Rank == 0)
            {
                LeaderboardUI leaderboardUI = Instantiate(_firstLeaderboard, _leaderboardPos.transform);
                leaderboardUI.Setup(entry);
                _leaderboardList.Add(leaderboardUI);
            }
            else if(entry.Rank == 1)
            {
                LeaderboardUI leaderboardUI = Instantiate(_secondLeaderboard, _leaderboardPos.transform);
                leaderboardUI.Setup(entry);
                _leaderboardList.Add(leaderboardUI);
            }
            else if(entry.Rank == 2)
            {
                LeaderboardUI leaderboardUI = Instantiate(_thirdLeaderboard, _leaderboardPos.transform);
                leaderboardUI.Setup(entry);
                _leaderboardList.Add(leaderboardUI);
            }
            else
            {
                LeaderboardUI leaderboardUI = Instantiate(_othersLeaderboard, _leaderboardPos.transform);
                leaderboardUI.Setup(entry);
                _leaderboardList.Add(leaderboardUI);
            }
        }
    }

}
