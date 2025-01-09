using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClueObjectUI : MonoBehaviour, 
    IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //[SerializeField]
    //private Outline _outline;

    private Vector2 _offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = ClueBoardManager.Instance.HoldingPinTransform;
        ClueBoardManager.Instance.AddToBin(this);
        //CoroutineUtils.ExecuteAfterEndOfFrame(StartDelay, this);
    }

    void StartDelay()
    {
        ClueBoardManager.Instance.AddToBin(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select(bool select) {
        //_outline.enabled = select;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.pressPosition;
        Vector2 uiPos = transform.position;
        _offset = uiPos - mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _offset = Vector2.zero;
    }
}
