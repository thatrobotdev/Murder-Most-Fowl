using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClueBoardBin : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private List<ClueObjectUI> _clueList;

    private Transform _draggedClue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _clueList = new List<ClueObjectUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start Drag");
        if (_clueList.Count <= 0)
        {
            return;
        }

        ClueObjectUI clueObject = _clueList[0];
        _clueList.RemoveAt(0);

        clueObject.gameObject.transform.position = eventData.position;
        clueObject.gameObject.transform.parent = ClueBoardManager.Instance.BoardTransform;
        _draggedClue = clueObject.gameObject.transform;
    }
    public void OnDrag(PointerEventData eventData)
    {
        _draggedClue.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _draggedClue = null;
    }

    public void AddToBin(ClueObjectUI clueObject)
    {
        _clueList.Add(clueObject);
    }
}
