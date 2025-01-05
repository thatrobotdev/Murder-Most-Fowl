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
    private InputSystemUIInputModule inputModule;

    private InputActionMap _currentActionMap;
    
    private InputActionMap _mainControls;
    //private InputActionMap _clueBoardControls;
    private InputActionMap _uiControls;

    private InputAction _pointAction;
    private InputAction _clickAction;
    private InputAction _rightClickAction;
    //private InputAction _deltaCursorAction;
    //private InputAction _cancelAction;
    //private InputAction _moveAction;
    //private InputAction _openClueBoardAction;

    //private InputAction _closeClueBoardAction;
    //private InputAction _scrollClueBoardAction;

    //private Vector2 _mouseDelta;
    //private Vector2 _screenPosition;
    //private float _scrollDelta;
    private float _pressTime;
    private float _clickTime;
    private float _rightClickTime;
    //private bool _moveCamera;
    //private bool _panCamera;

    //public Vector2 ScreenPosition {
    //    get { return _screenPosition; }
    //}

    ////Events
    public event Action Click;
    public event Action Hold;
    public event Action Cancel;
    public event Action RightClick;
    //public event Action RightHold;
    public event Action RightCancel;
    //public event Action<Vector2> Hover;
    //public event Action<Vector2> MouseMove;

    //public event Action<bool> ToggleClueBoard;
    //public event Action<float> OnScrollCB;

    private void Awake()
    {
        InitializeSingleton();

        SetControls();

        //Overall Controls
        _mainControls.Enable();
        //_clueBoardControls.Disable();
        //_currentActionMap = _mainControls;
    }
    //// Start is called before the first frame update
    //void Start()
    //{
    //    _screenPosition = Vector2.zero;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    OnHover();
    //}

    private void SetControls()
    {
        SetUIControls();
    }

    private void SetMainControls() {
        _mainControls = inputActions.FindActionMap("MainControls");

    //    _deltaCursorAction = _mainControls.FindAction("DeltaCursor");
    //    _cancelAction = _mainControls.FindAction("Cancel");
    //    _moveAction = _mainControls.FindAction("Move");
    //    _openClueBoardAction = _mainControls.FindAction("OpenClueBoard");

    //    _pressTime = 1.0f;
    //}

    //private void SetClueBoardControls() {
    //    _clueBoardControls = inputActions.FindActionMap("ClueBoardControls");

    //    _closeClueBoardAction = _clueBoardControls.FindAction("CloseClueBoard");
    //    _scrollClueBoardAction = _clueBoardControls.FindAction("ScrollWheel");
    //}
    //private void InitializeClueBoardControls() {
    //    _closeClueBoardAction.performed += OnCloseClueBoard;
    //    _scrollClueBoardAction.performed += OnScrollPerformed;

    //    ToggleClueBoard += ToggleControls;
    }

    private void SetUIControls()
    {
        _uiControls = inputActions.FindActionMap("UI");

        _rightClickAction = _uiControls.FindAction("RightClick");
        _clickAction = _uiControls.FindAction("Click");
        _pointAction = _uiControls.FindAction("Point");
    }

    //private void InitializeMainControls() {
    //    _openClueBoardAction.performed += OnOpenClueBoard;
    //}

    private void InitializeUIControls()
    {
        _rightClickAction.started += OnRightClickStarted;
        //_rightClickAction.performed += OnRightClickPerformed;
        _rightClickAction.canceled += OnRightClickCanceled;
        _clickAction.started += OnClickStarted;
        _clickAction.canceled += OnClickCanceled;
    }

    //private void OnEnable() {
    //    inputActions.FindActionMap(_currentActionMap.id).Enable();
    //}

    //private void OnDisable() {
    //    inputActions.FindActionMap(_currentActionMap.id).Disable();
    //}

    //private void OnDeltaCursor(InputAction.CallbackContext inputValue) {
    //    _mouseDelta = inputValue.ReadValue<Vector2>();
    //    MouseMove?.Invoke(_mouseDelta);
    //}

    //private void OnCancel(InputValue inputValue) {
    //    Cancel?.Invoke();
    //}
    private void OnClickStarted(InputAction.CallbackContext context)
    {
        _clickTime = Time.time;
    }
    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        if (Time.time - _clickTime >= _pressTime)
        {
            //TODO
            //Rework Hold to take into account both time and position
            //Probably should sit in Update or somewhere else
            return;
        }
        Debug.Log("Just a click!!!");
        Click?.Invoke();
    }

    private void OnRightClickStarted(InputAction.CallbackContext context)
    {
        _rightClickTime = Time.time;
    }
    //private void OnRightClickPerformed(InputAction.CallbackContext context)
    //{
    //    RightHold?.Invoke();
    //}
    private void OnRightClickCanceled(InputAction.CallbackContext context)
    {
        if (Time.time - _rightClickTime >= _pressTime)
        {
            RightCancel?.Invoke();
            return;
        }
        RightClick?.Invoke();
        Debug.Log("Just a right click!!!");
    }
    //private void OnHover()
    //{
    //    _screenPosition = _cursorAction.ReadValue<Vector2>();
    //    Hover?.Invoke(_screenPosition);
    //}


    //private void OnOpenClueBoard(InputAction.CallbackContext context) {
    //    ToggleClueBoard?.Invoke(true);
    //}

    //private void OnCloseClueBoard(InputAction.CallbackContext context) {
    //    ToggleClueBoard?.Invoke(false);
    //}

    //public void ToggleControls(bool enabled) {
    //    if (enabled) {
    //        ToggleActionMap(_clueBoardControls);
    //    } else {
    //        ToggleActionMap(_mainControls);
    //    }
    //}

    //private void OnScrollPerformed(InputAction.CallbackContext context) {
    //    //Debug.Log(context.ReadValue<Vector2>().y);
    //    OnScrollCB?.Invoke(context.ReadValue<Vector2>().y);
    //}

    //private void ToggleActionMap(InputActionMap actionMap) {
    //    if (actionMap.enabled) {
    //        return;
    //    }

    //    _currentActionMap.Disable();
    //    _currentActionMap = actionMap;
    //    _currentActionMap.Enable();
    //}

}
