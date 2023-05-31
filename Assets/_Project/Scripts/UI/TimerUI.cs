
using System;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Services.Leaderboards;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private DateTime _startTime;
    private TimeSpan _currentTime;

    private bool _isRunning;

    public void Finish()
    {
        _isRunning = false;
        SaveData data = SaveSystem.localData;

        if(data.time.Count <= GameManager.sInstance.LevelIndex)
        {
            data.time.Add(_currentTime);
        }
        else
        {
            if (_currentTime < data.time[GameManager.sInstance.LevelIndex])
            {
                data.time[GameManager.sInstance.LevelIndex] = _currentTime;
            }
        }

        AddLeaderboard();
        SaveSystem.Save();
    }

    public void Pause()
    {
        PauseTimer();
    }

    public void UnPause()
    {
        _startTime = DateTime.Now - _currentTime;
        _isRunning = true;
    }

    private void Start()
    {
        SaveData data = SaveSystem.localData;
        SetupEvents();
        _startTime = DateTime.Now;
        _isRunning = true;
    }

    private void OnDestroy()
    {
        DestroyEvents();
    }

    private void SetupEvents()
    {
        GameManager.sInstance.GetInputListener().OnPause += PauseTimer;
    }

    private void DestroyEvents()
    {
        GameManager.sInstance.GetInputListener().OnPause -= PauseTimer;
    }

    private void Update()
    {
        if (_isRunning)
        {
            TimeSpan elapsedTime = DateTime.Now - _startTime;
            _currentTime = elapsedTime;

            string timeText = string.Format("{0:00}:{1:00}:{2:000}", elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds);
            _timerText.SetText(timeText);
        }
    }

    private async UniTask AddLeaderboard()
    {
        string level = "";

        switch(GameManager.sInstance.LevelIndex)
        {
            case 0:
                level = "LEVEL1";
                break;
            case 1:
                level = "LEVEL2";
                break;
            case 2:
                level = "LEVEL3";
                break;
        }

        await LeaderboardsService.Instance.AddPlayerScoreAsync(level, _currentTime.TotalSeconds);
        //var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("my-first-leaderboard");
    }

    private void PauseTimer()
    {
        SaveData data = SaveSystem.localData;

        if (_isRunning)
        {
            _currentTime = DateTime.Now - _startTime;
            _isRunning = false;
        }
        else
        {
            _startTime = DateTime.Now - _currentTime;
            _isRunning = true;
        }
    }
}