using UnityEngine;
using UnityEngine.EventSystems;

namespace Clues
{
    public class ClueObjectUI : MonoBehaviour, 
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IScrollHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Vector2 _offset;
        private bool _mouseDown;

        // Start is called before the first frame update
        private void Start()
        {
            transform.parent = ClueBoardManager.Instance.HoldingPinTransform;
            transform.localPosition = Vector3.zero;
            ClueBoardManager.Instance.AddToBin(this);
            _mouseDown = false;
        }

        // TODO: Never used. remove?
        private void StartDelay()
        {
            ClueBoardManager.Instance.AddToBin(this);
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

            transform.parent = ClueBoardManager.Instance.BoardTransform;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _offset = Vector2.zero;
        }

        public void OnScroll(PointerEventData eventData)
        {
            // TODO
            // Get resizing working for ClueObjects
            if (_mouseDown)
            {
                Debug.Log("Yippee!");
            }
            else
            {
                ClueBoardManager.Instance.OnScroll(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _mouseDown = false;
        }
    }
}
