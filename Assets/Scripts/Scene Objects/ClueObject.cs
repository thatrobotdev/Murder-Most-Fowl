using UnityEngine;
using UnityEngine.EventSystems;

public class ClueObject : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    private SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        CoroutineUtils.ExecuteAfterEndOfFrame(EnableActions, this);
    }
    void OnDisable()
    {
        //CameraController.Instance.PointerEnterAction -= OnPointerEnter;
       // CameraController.Instance.PointerExitAction -= OnPointerExit;
    }

    void EnableActions()
    {
        //CameraController.Instance.PointerEnterAction += OnPointerEnter;
        //CameraController.Instance.PointerExitAction += OnPointerExit;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter!");
        _spriteRenderer.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit!");
        _spriteRenderer.color = Color.green;
    }
}
