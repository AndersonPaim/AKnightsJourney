using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager sInstance;

    public delegate void GetRespawnPositionHandler();
    public GetRespawnPositionHandler OnGetRespawnPosition;

    public delegate void GameOverHandler();
    public GameOverHandler OnGameOver;

    public delegate void FinishHandler();
    public FinishHandler OnFinish;

    public delegate void SetScoreHandler();
    public SetScoreHandler OnSetScore;

    [SerializeField] private int _respawns;

    public CheckpointManager checkpointManager;

    public PlayerController playerController;

    public InGameMenu inGameMenu;

    public InputListener inputListener;

    public ScoreManager scoreManager;

    public SettingsMenu settingsMenu;

    public ObjectPooler objectPooler;

    private void Awake()
    {
        if(sInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            sInstance = this;
        }
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        playerController.OnDeath += PlayerDeath;
        checkpointManager.OnSetRespawnPosition += SetPlayerRespawnPosition;
        checkpointManager.OnLastCheckpoint += Finish;
        SceneController.GetCurrentScene();
    }

    private void RemoveDelegates()
    {
        playerController.OnDeath -= PlayerDeath;
        checkpointManager.OnSetRespawnPosition -= SetPlayerRespawnPosition;
        checkpointManager.OnLastCheckpoint -= Finish;
    }
   

    private void PlayerDeath()
    {
        _respawns--;

        if (_respawns == 0)
        {
            GameOver();
        }
        else
        {
            OnGetRespawnPosition?.Invoke();
        }
    }

    private void SetPlayerRespawnPosition(Vector3 pos)
    {
        playerController.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }

    private void Finish()
    {
        OnSetScore?.Invoke();
        OnFinish?.Invoke();
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
    }

    private void SetLevelCount()
    {

    }
}
