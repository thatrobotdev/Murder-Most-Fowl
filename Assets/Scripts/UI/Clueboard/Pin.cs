using System;
using System.Collections.Generic;
using Clues;
using UnityEngine;
using UnityEngine.EventSystems;

// using namespace UI.Clueboard;

public class Pin : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private UILineRenderer _lineRenderer;

    private Pin _connected;
    
    private void Awake()
    {
        _lineRenderer = GetComponentInChildren<UILineRenderer>();
    }
    
    bool _isDragging;

    private bool _lrNeedSetParent = true;
    private void SetLineRendererParent()
    {
        if (!_lrNeedSetParent) return;
        _lineRenderer.rectTransform.SetParent(ClueBoardManager.Instance.StringRenderers);
        _lrNeedSetParent = false;
    }

    void Update()
    {
        SetLineRendererParent();
        _lineRenderer.transform.position = transform.position;
        if (!_isDragging && _connected != null)
        {
            SetLineRendererEnd(_connected.transform.position);
        }
    }

    void SetLineRendererEnd(Vector2 endPos)
    {
        endPos = (endPos - (Vector2) transform.position) / ClueBoardManager.Instance.BoardTransform.localScale;
        _lineRenderer.SetEndPoint(endPos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetLineRendererEnd(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        _connected = null;
        
        foreach (var raycastResult in raycastResults)
        {
            var pin = raycastResult.gameObject.GetComponent<Pin>();

            if (pin != null)
            {
                _connected = pin;
                break;
            }
        }

        if (_connected == null)
        {
            _lineRenderer.SetEndPoint(Vector2.zero);
        }
    }
}