using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputController : Singleton<InputController>
{
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField]
    private EventSystem eventSystem;
    [SerializeField]
    private InputSystemUIInputModule uiInputModule;

    private InputActionMap _currentActionMap;
    
    private InputActionMap _mainControls;
    private InputActionMap _clueBoardControls;
    private InputActionMap _uiControls;

    private InputAction _rightClickAction;
    private InputAction _leftClickAction;
    private InputAction _cursorAction;
    private InputAction _deltaCursorAction;
    private InputAction _cancelAction;
    private InputAction _moveAction;
    private InputAction _openClueBoardAction;

    private InputAction _closeClueBoardAction;
    private InputAction _scrollClueBoardAction;

    private Vector2 _mouseDelta;
    private Vector2 _screenPosition;
    private float _scrollDelta;
    private float _pressTime;
    private float _rightClickTime;
    private float _leftClickTime;
    private bool _moveCamera;
    private bool _panCamera;

    public Vector2 ScreenPosition {
        get { return _screenPosition; }
    }

    //Events
    public event Action RightClick;
    public event Action RightHold;
    public event Action RightCancel;
    public event Action LeftClick;
    public event Action LeftHold;
    public event Action Cancel;
    public event Action<Vector2> Hover;
    public event Action<Vector2> MouseMove;

    public event Action<bool> ToggleClueBoard;
    public event Action<float> OnScrollCB;

    private void Awake() {
        InitializeSingleton();

        //Main Controls
        SetMainControls();
        InitializeMainControls();

        //Clue Board Controls
        SetClueBoardControls();
        InitializeClueBoardControls();

        //UI Controls
        //uiInputModule.scrollWheel.action.performed += _closeClueBoardAction.performed;

        //Overall Controls
        _mainControls.Enable();
        _clueBoardControls.Disable();
        _currentActionMap = _mainControls;
    }
    // Start is called before the first frame update
    void Start()
    {
        _screenPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        OnHover();
    }

    private void SetMainControls() {
        _mainControls = inputActions.FindActionMap("MainControls");

        _rightClickAction = _mainControls.FindAction("RightClick");
        _leftClickAction = _mainControls.FindAction("LeftClick");
        _cursorAction = _mainControls.FindAction("Cursor");
        _deltaCursorAction = _mainControls.FindAction("DeltaCursor");
        _cancelAction = _mainControls.FindAction("Cancel");
        _moveAction = _mainControls.FindAction("Move");
        _openClueBoardAction = _mainControls.FindAction("OpenClueBoard");

        _pressTime = 1.0f;
    }
    private void InitializeMainControls() {
        _rightClickAction.started += OnRightClickStarted;
        _rightClickAction.performed += OnRightClickPerformed;
        _rightClickAction.canceled += OnRightClickCanceled;
        _leftClickAction.started += OnLeftClickStarted;
        _leftClickAction.canceled += OnLeftClickCanceled;

        _deltaCursorAction.performed += OnDeltaCursor;

        _openClueBoardAction.performed += OnOpenClueBoard;
    }

    private void SetClueBoardControls() {
        _clueBoardControls = inputActions.FindActionMap("ClueBoardControls");

        _closeClueBoardAction = _clueBoardControls.FindAction("CloseClueBoard");
        _scrollClueBoardAction = _clueBoardControls.FindAction("ScrollWheel");
    }
    private void InitializeClueBoardControls() {
        _closeClueBoardAction.performed += OnCloseClueBoard;
        _scrollClueBoardAction.performed += OnScrollPerformed;

        ToggleClueBoard += ToggleControls;
    }

    private void OnEnable() {
        inputActions.FindActionMap(_currentActionMap.id).Enable();
    }

    private void OnDisable() {
        inputActions.FindActionMap(_currentActionMap.id).Disable();
    }

    private void OnDeltaCursor(InputAction.CallbackContext inputValue) {
        _mouseDelta = inputValue.ReadValue<Vector2>();
        MouseMove?.Invoke(_mouseDelta);
    }

    private void OnCancel(InputValue inputValue) {
        Cancel?.Invoke();
    }

    private void OnRightClickStarted(InputAction.CallbackContext context) {
        _rightClickTime = Time.time;
    }
    private void OnRightClickPerformed(InputAction.CallbackContext context) {
        RightHold?.Invoke();
    }
    private void OnRightClickCanceled(InputAction.CallbackContext context) {
        if (Time.time - _rightClickTime >= _pressTime) {
            RightCancel?.Invoke();
            return;
        }
        RightClick?.Invoke();
        Debug.Log("Just a right click!!!");
    }
    private void OnLeftClickStarted(InputAction.CallbackContext context) {
        _leftClickTime = Time.time;
    }
    private void OnLeftClickCanceled(InputAction.CallbackContext context) {
        if (Time.time - _leftClickTime >= _pressTime) {
            return;
        }
        Debug.Log("Just a click!!!");
        LeftClick?.Invoke();
    }
    private void OnHover()
    {
        _screenPosition = _cursorAction.ReadValue<Vector2>();
        Hover?.Invoke(_screenPosition);
    }


    private void OnOpenClueBoard(InputAction.CallbackContext context) {
        ToggleClueBoard?.Invoke(true);
    }

    private void OnCloseClueBoard(InputAction.CallbackContext context) {
        ToggleClueBoard?.Invoke(false);
    }

    public void ToggleControls(bool enabled) {
        if (enabled) {
            ToggleActionMap(_clueBoardControls);
        } else {
            ToggleActionMap(_mainControls);
        }
    }

    private void OnScrollPerformed(InputAction.CallbackContext context) {
        //Debug.Log(context.ReadValue<Vector2>().y);
        OnScrollCB?.Invoke(context.ReadValue<Vector2>().y);
    }

    private void ToggleActionMap(InputActionMap actionMap) {
        if (actionMap.enabled) {
            return;
        }

        _currentActionMap.Disable();
        _currentActionMap = actionMap;
        _currentActionMap.Enable();
    }

}
