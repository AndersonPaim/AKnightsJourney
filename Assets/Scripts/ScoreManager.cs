using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public delegate void SetScoreHandler(float stars);
    public SetScoreHandler OnSetScore;

    [SerializeField] private float _score;
    [SerializeField] private float _scoreTarget1;
    [SerializeField] private float _scoreTarget2;
    [SerializeField] private float _scoreTarget3;

    public float starsScore;

    private void Start()
    {
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.OnSetScore += SetScore;
        GameManager.sInstance.playerController.OnDeath += Death;
        Coin.OnCollectCoin += CollectCoin;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.OnSetScore += SetScore;
        GameManager.sInstance.playerController.OnDeath -= Death;
        Coin.OnCollectCoin -= CollectCoin;
    }

    private void SaveScore()
    {
        SaveData data = SaveSystem.localData;

        if (starsScore > data.stars[SceneController.currentScene - 1])
        {
            data.stars[SceneController.currentScene - 1] = starsScore;
        }

        SaveSystem.Save();
    }

    private void CollectCoin()
    {
        _score++;
    }

    private void SetScore()
    {
        if (_score < _scoreTarget1)
        {
            starsScore = 0;
        }
        else if(_score >= _scoreTarget1 && _score < _scoreTarget2)
        {
            starsScore = 1;
        }
        else if (_score >= _scoreTarget2 && _score < _scoreTarget3)
        {
            starsScore = 2;
        }
        else
        {
            starsScore = 3;
        }
       
        OnSetScore?.Invoke(starsScore);
        SaveScore();
    }

    private void Death()
    {
        _score -= 5;
    }

}
