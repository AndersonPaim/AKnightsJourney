using _Project.Scripts.Managers;
using Coimbra.Services;
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
    [SerializeField] private int _levelIndex;

    [SerializeField] private CheckpointManager checkpointManager;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private InGameMenu inGameMenu;

    [SerializeField] private InputListener inputListener;

    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private SettingsMenu settingsMenu;

    [SerializeField] private ObjectPooler objectPooler;

    [SerializeField] private TimerUI timerUI;

    [SerializeField] private SceneController sceneController;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private SoundEffect _finishSFX;
    [SerializeField] private SoundEffect _gameOverSFX;

    public int LevelIndex => _levelIndex;
    private IAudioPlayer _audioPlayer;

    private void Awake()
    {
        if(sInstance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            sInstance = this;
        }

        _audioPlayer = ServiceLocator.Get<IAudioPlayer>();

        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        BeeBoss.OnFinish += Finish;
        playerController.OnDeath += PlayerDeath;
        playerController.OnTakeDamage += PlayerDamage;
        checkpointManager.OnSetRespawnPosition += SetPlayerRespawnPosition;
        checkpointManager.OnLastCheckpoint += Finish;
    }

    private void RemoveDelegates()
    {
        BeeBoss.OnFinish -= Finish;
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
        timerUI.Finish();
        OnSetScore?.Invoke();
        OnFinish?.Invoke();
        _audioPlayer.PlayAudio(_finishSFX, transform.position);
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0;
        OnGameOver?.Invoke();
        _audioPlayer.PlayAudio(_gameOverSFX, transform.position);
    }
}
