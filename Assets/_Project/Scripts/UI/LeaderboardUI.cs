using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankingText;
    [SerializeField] private TextMeshProUGUI _usernameText;
    [SerializeField] private TextMeshProUGUI _resultText;

    public void Clear()
    {
        _rankingText.text = "-";
        _resultText.text = "-";
    }

    public void Setup(LeaderboardEntry leaderboard)
    {
        int rank = leaderboard.Rank + 1;
        _rankingText.text = rank.ToString();
        _usernameText.text = leaderboard.PlayerName.ToString();

        TimeSpan timeSpan = TimeSpan.FromSeconds(leaderboard.Score);
        string timeFormat = string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        _resultText.text = timeFormat;
    }

}
