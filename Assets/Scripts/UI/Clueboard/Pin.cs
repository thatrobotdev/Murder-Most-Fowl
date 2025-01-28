using System;
using UnityEngine;
using UnityEngine.EventSystems;

// using namespace UI.Clueboard;

public class Pin : MonoBehaviour, IDragHandler
{
    private Vector2 _offset;

    private Vector2 endPos;
    
    private UILineRenderer lineRenderer;

    private Canvas canvas;

    [SerializeField] private Transform positionedParent;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<UILineRenderer>();
        canvas = GetComponentInParent<Canvas>();
    }

    private bool _lrNeedSetParent = true;
    private void SetLineRendererParent()
    {
        if (!_lrNeedSetParent) return;
        lineRenderer.rectTransform.SetParent(ClueBoardManager.Instance.StringRenderers);
        _lrNeedSetParent = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetLineRendererParent();
        
        // print(eventData.position + _offset);
        lineRenderer.points[0] = Vector2.zero;
        lineRenderer.points[1] = (eventData.position - (Vector2) transform.position) / canvas.scaleFactor;
        
        lineRenderer.SetVerticesDirty();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.pressPosition;
        Vector2 uiPos = transform.position;
        _offset = uiPos - mousePos;
    }
}