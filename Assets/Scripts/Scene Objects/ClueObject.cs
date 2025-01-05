using UnityEngine;
using UnityEngine.EventSystems;

public class ClueObject : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!_spriteRenderer)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData);
        _spriteRenderer.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteRenderer.color = Color.green;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData);
    }
}
