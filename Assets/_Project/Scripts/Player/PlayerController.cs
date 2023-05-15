using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.VFX;
using UnityEngine.Rendering.Universal;
using Coimbra.Services;
using _Project.Scripts.Managers;
using Unity.Services.Analytics;

public class PlayerController : MonoBehaviour, IDamageable
{
    public delegate void SetCheckpointHandler();
    public SetCheckpointHandler OnSetCheckpoint;

    public delegate void PlayerInputHandler(PlayerData playerData);
    public PlayerInputHandler OnPlayerInput;

    public delegate void DeathHandler();
    public DeathHandler OnDeath;

    public delegate void TakeDamageHandler();
    public TakeDamageHandler OnTakeDamage;

    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private PlayerBalancer _playerBalancer;

    [SerializeField] private GameObject _dashCollider;
    [SerializeField] private List<VisualEffect> _slashEffects;
    [SerializeField] private SoundEffect _attackSFX;
    [SerializeField] private SoundEffect _damageSFX;

    private float _jumpsCount = 2;
    private float _movementX = 0;
    private float _movementY = 0;
    private float _velocity = 0;
    private float _lastDirection;

    private bool _isGrounded = true;
    private bool _canDash = false;
    private bool _canJump = true;
    private bool _isDashing = false;
    private bool _isAttacking = false;
    private bool _isWalking = false;
    private bool _isRunning = false;
    private bool _isJumping = false;
    private bool _isDoubleJumping = false;
    private bool _isPaused = false;
    private bool _isVulnerable = true;
    private bool _isTrapVulnerable = true;
    private bool _canMove = true;
    private bool _isDead = false;
    private bool _isClimbing = false;

    private Rigidbody _rb;

    private Quaternion _playerRotationLeft;
    private Quaternion _playerRotationRight;

    private PlayerData _playerData;
    private IAudioPlayer _audioPlayer;

    private CinemachineBasicMultiChannelPerlin _cameraNoise;

    public bool IsVulnerable => _isVulnerable;
    public bool IsJumping => _isJumping;
    public bool IsDoubleJumping => _isDoubleJumping;
    public bool IsHanging {get ; set;}
    public float LastDirection => _lastDirection;

    public void TakeDamage(float damage, GameObject attacker)
    {
        if(!_isVulnerable)
        {
            return;
        }

        _audioPlayer.PlayAudio(_damageSFX, transform.position);

        Vector3 knockbackDirection = new Vector3(0, 0, gameObject.transform.position.z - attacker.transform.position.z);
        _rb.velocity = new Vector3(0, 0, knockbackDirection.z) * 40;

        StartCoroutine(VurnerabilityDelay());
        OnTakeDamage?.Invoke();
        Knockback(attacker);
        StartCoroutine(HitStop());

        DamageAnalytics(attacker);
    }

    public void SlimeJump()
    {
        Debug.Log("SLIME JUMP");
        Vector3 moveVector = new Vector3(0, _playerBalancer.jumpForce * 3, 0);
        Vector3 vel = _rb.velocity;
        vel = vel + moveVector;
        _rb.velocity = Vector3.ClampMagnitude(vel, _playerBalancer.jumpForce);
    }

    private void DamageAnalytics(GameObject attacker)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "Level", GameManager.sInstance.LevelIndex + 1 },
            { "PositionX", transform.position.z },
            { "PositionY", transform.position.y },
            { "DamageSource", attacker.gameObject.name },
        };

        AnalyticsService.Instance.CustomData("Death", parameters);
        AnalyticsService.Instance.Flush();

        Debug.Log("SEND ANALYTICS: " + attacker.gameObject.name);
    }

    private void Start()
    {
        Initialize();

        Gravity();
    }

    float time = 0;

    private void Update()
    {
        CreatePlayerStruct();
        GroundCheck();
        OnPlayerInput?.Invoke(_playerData);
        _playerData = new PlayerData();

        if(!_isGrounded)
        {
            Vector3 vel = _rb.velocity;
            vel.y-= 18 * Time.deltaTime;
            _rb.velocity=vel;
        }
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.GetInputListener().OnInput += ReceiveInputs;
        GameManager.sInstance.OnGetRespawnPosition += DeathMovementDelay;
        GameManager.sInstance.GetInGameMenu().OnPause += PauseInputs;
        GameManager.sInstance.GetAnimationController().OnStartAttack += OnStartAttack;
        GameManager.sInstance.GetAnimationController().OnStopAttack += OnStopAttack;
        GameManager.sInstance.OnFinish += FinishGame;
    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.GetInputListener().OnInput -= ReceiveInputs;
        GameManager.sInstance.OnGetRespawnPosition -= DeathMovementDelay;
        GameManager.sInstance.GetInGameMenu().OnPause -= PauseInputs;
        GameManager.sInstance.GetAnimationController().OnStartAttack -= OnStartAttack;
        GameManager.sInstance.GetAnimationController().OnStopAttack -= OnStopAttack;
        GameManager.sInstance.OnFinish -= FinishGame;
    }

    private void Initialize()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _playerRotationLeft = Quaternion.Euler(0, 180, 0);
        _playerRotationRight = Quaternion.Euler(0, 0, 0);
        IsHanging = false;
        _playerData = new PlayerData();
        _audioPlayer = ServiceLocator.Get<IAudioPlayer>();
        _cameraNoise = _camera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        SetupDelegates();
    }

    private void PauseInputs(bool isPaused)
    {
        _isPaused = isPaused;
    }

    private void FinishGame()
    {
        _isDead = true;
    }

    private void OnStartAttack(int attack)
    {
        _audioPlayer.PlayAudio(_attackSFX, transform.position);
        _canJump = false;
        ResetForces();
        _playerBalancer.runSpeed *= 0.5f;
        _velocity *= 0.5f;

        _slashEffects[attack - 1].Play();
    }

    private void OnStopAttack(int attack)
    {
        _playerBalancer.runSpeed *= 2;
        _canJump = true;
    }

    private IEnumerator HitStop()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
    }

    private void Knockback(GameObject attacker)
    {
        Vector3 knockbackDirection = new Vector3(0, 0, gameObject.transform.position.z - attacker.transform.position.z);
        ResetForces();
        StartCoroutine(MoveCooldown());
        Debug.Log("KNOCKBACK: " + knockbackDirection.z);
        _rb.velocity = new Vector3(0, 0, knockbackDirection.z) * 20;
    }

    private void DeathMovementDelay()
    {
        _isPaused = true;
        _velocity = 0;
        _movementX = 0;
        ResetForces();
        StartCoroutine(MovementDelay());
    }

    private IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _isPaused = false;
    }

    private void ReceiveInputs(InputData inputData)
    {
        if (!_isPaused)
        {
            Movement(inputData.MovementX, inputData.MovementY);
            Jump(inputData.Jump);
            Dash(inputData.Dash);
            Run(inputData.Run);
            Attack(inputData.Attack);
        }
    }

    private void CreatePlayerStruct()
    {
        _playerData.Walk = _isWalking;
        _playerData.OnGround = _isGrounded;
        _playerData.Jump = _isJumping;
        _playerData.Dash = _isDashing;
        _playerData.DoubleJump = _isDoubleJumping;
        _playerData.Movement = _movementX;
        _playerData.Velocity = _velocity;
        _playerData.Attack = _isAttacking;
        _playerData.Hanging = IsHanging;
        _playerData.Climbing = _isClimbing;

        _isClimbing = false;
    }


    private void Movement(float directionX, float directionY)
    {
        _movementX = directionX;
        _movementY = directionY;

        Vector3 vel = _rb.velocity;

        PlayerRotation(directionX);
        SetVelocity(directionX);

        if(!_canMove)
        {
            return;
        }

        if (directionX != 0 && !_isDashing && !IsHanging)
        {
            vel.z = directionX * _playerBalancer.speedMultiplier * _velocity;
            _rb.velocity = vel;

            _lastDirection = directionX;
        }
        else if(directionX == 0 && !_isDashing && !IsHanging)
        {
            vel.z = _lastDirection * _playerBalancer.speedMultiplier * _velocity;

            _rb.velocity = vel;
        }
    }

    private void SetVelocity(float direction)
    {
        if (direction != 0 && _isDashing == false)
        {
            if (_isRunning)
            {
                if (_velocity < _playerBalancer.runSpeed)
                {
                    _velocity += Time.deltaTime * _playerBalancer.acceleration;
                }
            }
            else
            {
                if (_velocity <= _playerBalancer.walkSpeed)
                {
                    _velocity += Time.deltaTime * _playerBalancer.acceleration;
                    if (_velocity > _playerBalancer.walkSpeed)
                    {
                        _velocity = _playerBalancer.walkSpeed;
                    }
                }
                else
                {
                    _velocity -= Time.deltaTime * _playerBalancer.deceleration;
                }
            }
        }
        else if (direction == 0 && _isDashing == false)
        {
            if (_velocity > 0)
            {
                _velocity -= Time.deltaTime * _playerBalancer.deceleration;
            }
            else if (_velocity <= 0)
            {
                _velocity = 0;
            }
        }
    }

    private void PlayerRotation(float direction)
    {
        if(direction < 0 && !IsHanging)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _playerRotationLeft, 1f);
            _isWalking = true;
        }
        else if(direction > 0 && !IsHanging)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _playerRotationRight, 1f);
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
    }

    private void Run(bool isRunning)
    {
        if (isRunning)
        {
            if (_isWalking)
            {
                _isRunning = true;
            }
        }
        else
        {
            _isRunning = false;
        }
    }

    private void ResetForces()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void Jump(bool isJumping)
    {
        if (isJumping)
        {
            if(IsHanging)
            {
                _isClimbing = true;
            }

            if(!_canJump)
            {
                return;
            }

            if (_jumpsCount > 0)
            {
                if(_jumpsCount == 1)
                {
                    _isDoubleJumping = true;
                }
                ResetForces();

                Vector3 moveVector = new Vector3(0, _playerBalancer.jumpForce, 0);
                Vector3 vel = _rb.velocity;
                vel = vel + moveVector;
                _rb.velocity = Vector3.ClampMagnitude(vel, _playerBalancer.jumpForce);
                StartCoroutine(JumpCount());
            }
        }
    }

    private void Attack(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    private IEnumerator JumpCount()
    {
        yield return new WaitForFixedUpdate();
        _jumpsCount--; //a verifica��o de ch�o anulava esse comando | arrumar depois
        _isJumping = true;
    }

    private IEnumerator MoveCooldown()
    {
        _canMove = false;
        yield return new WaitForSeconds(0.1f);
        _canMove = true;
    }

    private void Dash(bool isDashing)
    {
        if (isDashing && _canDash)
        {
            Vector3 dir = Vector3.forward * _lastDirection;
            dir.y = _movementY;

            if(_movementX > 0)
            {
                dir.z = 1;
            }
            else if(_movementX < 0)
            {
                dir.z = -1;
            }

            _rb.AddRelativeForce((dir * _playerBalancer.dashForce) * dir.z, ForceMode.Impulse);
            //transform.DOMove(transform.position + (dir * 5), 0.2f).OnComplete(StopDash);
            StartCoroutine(StopDash(_playerBalancer.dashTime));
            Physics.IgnoreLayerCollision(8, 10, true);
            _isVulnerable = false;
            _dashCollider.SetActive(true);
            _canDash = false;
            _isDashing = true;
            CameraShake(5, 0.1f);
        }
    }

    private IEnumerator StopDash(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ResetForces();
        CameraShake(0, 0);
        Physics.IgnoreLayerCollision(8, 10, false);
        _isVulnerable = true;
        _isDashing = false;
        _dashCollider.SetActive(false);
    }

    private IEnumerator VurnerabilityDelay()
    {
        _isVulnerable = false;
        yield return new WaitForSeconds(1);
        _isVulnerable = true;
    }

    private IEnumerator TrapVurnerabilityDelay()
    {
        _isTrapVulnerable = false;
        yield return new WaitForSeconds(1);
        _isTrapVulnerable = true;
    }

    private void CameraShake(float amplitude, float frequency)
    {
        //_camera.transform.DOComplete();
        //_camera.transform.DOShakePosition(0.2f, 0.5f, 14, 90, false, true);
        _cameraNoise.m_AmplitudeGain = amplitude;
        _cameraNoise.m_FrequencyGain = frequency;

    }

    private void Gravity()
    {
        Physics.gravity = _playerBalancer.gravity;
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.3f);

        if (_isGrounded)
        {
            _canDash = true;
            _jumpsCount = 2;
            _isJumping = false;
            _isDoubleJumping = false;
        }
    }

    private void TrapDeath()
    {
        if(!_isTrapVulnerable)
        {
            return;
        }

        StartCoroutine(TrapVurnerabilityDelay());
        OnDeath?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        LayerMask checkpointLayer = LayerMask.NameToLayer("Checkpoints");

        if (other.gameObject.layer == checkpointLayer)
        {
            OnSetCheckpoint?.Invoke();
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        LayerMask trapsLayer = LayerMask.NameToLayer("Traps");

        if (collision.collider.gameObject.layer == trapsLayer)
        {
            TrapDeath();
            DamageAnalytics(collision.gameObject);
        }
    }
}
