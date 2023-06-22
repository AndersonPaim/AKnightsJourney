using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public delegate void SetScoreHandler(float stars, float kills, float killsTarget, float coins, float coinsTarget, float lifes, float lifesTarget);
    public SetScoreHandler OnSetScore;
    public Action<float> OnGetCoin;

    [SerializeField] private float _score;
    [SerializeField] private float _coinsTarget;
    [SerializeField] private float _killsTarget;
    [SerializeField] private float _lifesTarget;

    private float _kills;
    private float _coins;
    private float _lifes;

    public float starsScore = 0;
    public int coinMultiplier = 1;

    public void SetCoinMultiplier(int multiplier)
    {
        coinMultiplier = multiplier;
    }

    public void SaveCoins()
    {
        SaveData data = SaveSystem.localData;
        data.coins += _score;
        SaveSystem.Save();
    }

    private void Start()
    {
        _lifes = _lifesTarget;
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.OnSetScore += SetScore;
        GameManager.sInstance.GetPlayerController().OnDeath += Death;
        EnemyMeleePatrolAI.OnDie += Kill;
        BeeBoss.OnFinish += Kill;
        FlyingEnemy.OnDie += Kill;
        Colletables.OnCollectItem += CollectItem;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.OnSetScore -= SetScore;
        GameManager.sInstance.GetPlayerController().OnDeath -= Death;
        EnemyMeleePatrolAI.OnDie -= Kill;
        FlyingEnemy.OnDie -= Kill;
        BeeBoss.OnFinish -= Kill;
        Colletables.OnCollectItem -= CollectItem;
    }

    private void SaveScore()
    {
        SaveCoins();

        SaveData data = SaveSystem.localData;

        if(data.stars.Count <= GameManager.sInstance.LevelIndex)
        {
            data.stars.Add(starsScore);
        }
        else
        {
            if (starsScore > data.stars[GameManager.sInstance.LevelIndex])
            {
                data.stars[GameManager.sInstance.LevelIndex] = starsScore;
            }
        }

        SaveSystem.Save();
    }

    private void CollectItem(int points)
    {
        _score += points * coinMultiplier;
        _coins++;
        OnGetCoin?.Invoke(_score);
    }

    private void SetScore()
    {
        if (_kills >= _killsTarget)
        {
            Debug.Log("ADD SCORE KILLS");
            starsScore++;
        }

        if(_coins >= _coinsTarget)
        {
            Debug.Log("ADD SCORE COINS");
            starsScore++;
        }

        if(_lifes >= _lifesTarget)
        {
            Debug.Log("ADD SCORE LIFE");
            starsScore++;
        }

        OnSetScore?.Invoke(starsScore, _kills, _killsTarget, _coins, _coinsTarget, _lifes, _lifesTarget);
        SaveScore();
    }

    private void Death()
    {
        _lifes--;
    }

    private void Kill()
    {
        _kills++;
    }

}
