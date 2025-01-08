using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class ClueBoardManager : Singleton<ClueBoardManager>
{
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private RectTransform _backgroundTransform;

    private ClueObjectUI _selectedObj;

    private float _zoom;
    private bool _activated;



    private void Awake() {
        InitializeSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();

        InputController.Instance.ToggleClueBoard += ToggleClueBoard;
        //InputController.Instance.OnScrollCB += OnScroll;

        _zoom = 1.0f;
        _activated = false;

        _selectedObj = null;

        CloseClueBoard();

        //m_EventSystem.scrollWheel.action.performed +=
        //m_EventSystem.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleClueBoard() {
        if (_activated)
        {
            CloseClueBoard();
        }
        else
        {
            OpenClueBoard();
        }
    }

    private void OpenClueBoard()
    {
        _activated = true;
        _canvas.enabled = _activated;
    }

    private void CloseClueBoard()
    {
        _activated = false;
        _canvas.enabled = _activated;
    }

    void OnScroll(float scroll)
    {
        float e = 0f;
        if (scroll > 0) {
            e = 0.05f;
        } else if (scroll < 0) {
            e = -0.05f;
        }
        _backgroundTransform.localScale += Vector3.one * e;

        if (_backgroundTransform.localScale.x < 1f) {
            _backgroundTransform.localScale = Vector3.one;
        }
        if (_backgroundTransform.localScale.x > 2f) {
            _backgroundTransform.localScale = Vector3.one * 2f;
        }
    }
}
