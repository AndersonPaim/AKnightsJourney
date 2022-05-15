using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputListener : MonoBehaviour
{

    public delegate void InputHandler(InputData inputData);
    public InputHandler OnInput;

    public delegate void PauseHandler();
    public PauseHandler OnPause;

    [SerializeField] private float _movementY;
    [SerializeField] private float _movementX;
    [SerializeField] private Vector2 _movementXY;

    [SerializeField] private bool _jump;
    [SerializeField] private bool _dash;
    [SerializeField] private bool _attack;
    [SerializeField] private bool _run;
    [SerializeField] private bool _walk;

    [SerializeField] private bool _inputsPaused = true;

    private PlayerInputActions _input;
    private InputData _inputData;

    public void PauseInput(bool isPaused)
    {
        _inputsPaused = isPaused;
        _run = false;
        _walk = false;
        _attack = false;
        _dash = false;
        _jump = false;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if(!_inputsPaused)
        {
            CreateInputStruct();
        }

        OnInput?.Invoke(_inputData);
        _inputData = new InputData();
        Movement();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        _input.Player.Jump.performed -= ctx => Jump(ctx);
        _input.Player.Dash.performed -= ctx => Dash(ctx);
        _input.Player.Run.performed -= ctx => Run(ctx);
        _input.Player.Run.canceled -= ctx => Run(ctx);
        _input.Player.Pause.performed -= _ => Pause();
        _input.Player.Attack.performed -= ctx => Attack(ctx);
    }

    private void Initialize()
    {
        _input = new PlayerInputActions();
        _input.Player.Jump.performed += ctx => Jump(ctx);
        _input.Player.Dash.performed += ctx => Dash(ctx);
        _input.Player.Run.performed += ctx => Run(ctx);
        _input.Player.Run.canceled += ctx => Run(ctx);
        _input.Player.Pause.performed += _ => Pause();
        _input.Player.Attack.performed += ctx => Attack(ctx);
    }

    private void CreateInputStruct()
    {
        _inputData.Dash = _dash;
        _inputData.Jump = _jump;
        _inputData.Run = _run;

        _inputData.MovementY = _movementY;
        _inputData.MovementX = _movementX;

        _inputData.Walk = _walk;
        _inputData.Attack = _attack;

        if (_jump)
        {
            _jump = false;
        }

        if (_dash)
        {
            _dash = false;
        }

        if (_attack)
        {
            _attack = false;
        }
    }

    private void Movement()
    {
        _movementY = _input.Player.MovementY.ReadValue<float>();
        _movementX = _input.Player.MovementX.ReadValue<float>();

        _movementXY = _input.Player.MovementXY.ReadValue<Vector2>();

        if(_movementXY.x != 0 || _movementXY.y != 0)
        {
            _movementY = _movementXY.y;
            _movementX = _movementXY.x;
        }

        if (_movementY != 0 || _movementX != 0)
        {
            _walk = true;
        }
        else
        {
            _walk = false;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _jump = true;
        }
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _dash = true;
        }
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _attack = true;
        }
    }

    private void Run(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _run = true;
        }
        else if (ctx.canceled)
        {
            _walk = true;
            _run = false;
        }
    }

    private void Pause()
    {
        OnPause?.Invoke();
    }
}
