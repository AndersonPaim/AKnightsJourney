using UnityEngine;

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

    [SerializeField] private CheckpointManager checkpointManager;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private InGameMenu inGameMenu;

    [SerializeField] private InputListener inputListener;

    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private SettingsMenu settingsMenu;

    [SerializeField] private ObjectPooler objectPooler;

    [SerializeField] private SceneController sceneController;
    [SerializeField] private AnimationController animationController;

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
        playerController.OnTakeDamage += PlayerDamage;
        checkpointManager.OnSetRespawnPosition += SetPlayerRespawnPosition;
        checkpointManager.OnLastCheckpoint += Finish;
    }

    private void RemoveDelegates()
    {
        playerController.OnDeath -= PlayerDeath;
        playerController.OnTakeDamage -= PlayerDamage;
        checkpointManager.OnSetRespawnPosition -= SetPlayerRespawnPosition;
        checkpointManager.OnLastCheckpoint -= Finish;
    }

    public CheckpointManager GetCheckPointManager()
    {
        return checkpointManager;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public InGameMenu GetInGameMenu()
    {
        return inGameMenu;
    }

    public InputListener GetInputListener()
    {
        return inputListener;
    }

    public ScoreManager GetScoreManager()
    {
        return scoreManager;
    }

    public SettingsMenu GetSettingsMenu()
    {
        return settingsMenu;
    }

    public ObjectPooler GetObjectPooler()
    {
        return objectPooler;
    }

    public SceneController GetSceneController()
    {
        return sceneController;
    }

    public AnimationController GetAnimationController()
    {
        return animationController;
    }

    private void PlayerDamage()
    {
        _respawns--;

        if (_respawns == 0)
        {
            GameOver();
        }
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
        inGameMenu.FinishScreen();
        OnSetScore?.Invoke();
        OnFinish?.Invoke();
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
