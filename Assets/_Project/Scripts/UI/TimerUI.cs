
using System;
using UnityEngine;
using TMPro;

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

        SaveSystem.Save();
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

            if (_currentTime != TimeSpan.Zero)
            {
                elapsedTime += _currentTime;
            }
            string timeText = string.Format("{0:00}:{1:00}:{2:000}", elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds);
            _timerText.SetText(timeText);
        }
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