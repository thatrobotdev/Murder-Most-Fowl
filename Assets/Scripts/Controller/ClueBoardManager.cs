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
    private Canvas m_Canvas;
    [SerializeField]
    private RectTransform m_BackgroundTransform;

    private ClueObjectUI m_SelectedObj;

    private float zoom;

    private void Awake() {
        InitializeSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Canvas = GetComponent<Canvas>();

        //InputController.Instance.ToggleClueBoard += ToggleCanvas;
        //InputController.Instance.OnScrollCB += OnScroll;

        zoom = 1.0f;
        ToggleCanvas(false);

        m_SelectedObj = null;

        //m_EventSystem.scrollWheel.action.performed +=
        //m_EventSystem.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleCanvas(bool toggle) {
        m_Canvas.enabled = toggle;
    }

    void OnScroll(float scroll)
    {
        float e = 0f;
        if (scroll > 0) {
            e = 0.05f;
        } else if (scroll < 0) {
            e = -0.05f;
        }
        m_BackgroundTransform.localScale += Vector3.one * e;

        if (m_BackgroundTransform.localScale.x < 1f) {
            m_BackgroundTransform.localScale = Vector3.one;
        }
        if (m_BackgroundTransform.localScale.x > 2f) {
            m_BackgroundTransform.localScale = Vector3.one * 2f;
        }
    }
}
