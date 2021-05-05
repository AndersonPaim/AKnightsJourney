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

    [SerializeField] private float _movement;

    [SerializeField] private bool _jump;
    [SerializeField] private bool _dash;
    [SerializeField] private bool _run;
    [SerializeField] private bool _walk;

    private PlayerInputActions _input;

    private InputData _inputData;

    void Awake()
    {
        Initialize();
    }

    void Update()
    { 
        CreateInputStruct();
        OnInput?.Invoke(_inputData);
        _inputData = new InputData();
        Movement();
    }

    void OnEnable()
    {
        _input.Enable();
    }

    void OnDisable()
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
    }

    private void Initialize()
    {
        _input = new PlayerInputActions();
        _input.Player.Jump.performed += ctx => Jump(ctx);
        _input.Player.Dash.performed += ctx => Dash(ctx);
        _input.Player.Run.performed += ctx => Run(ctx);
        _input.Player.Run.canceled += ctx => Run(ctx);
        _input.Player.Pause.performed += _ => Pause();
    }

    private void CreateInputStruct()
    {
        
        if (_jump)
        {
            _inputData.Jump = _jump;
            _jump = false;
        }
        else
        {
            _inputData.Jump = _jump;
        }

        if (_dash)
        {
            _inputData.Dash = _dash;
            _dash = false;
        }
        else
        {
            _inputData.Dash = _dash;
        }

        _inputData.Run = _run;
        _inputData.Movement = _movement;
        _inputData.Walk = _walk;
    }

    private void Movement()
    {
        _movement = _input.Player.Movement.ReadValue<float>();
        if (_movement != 0)
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
