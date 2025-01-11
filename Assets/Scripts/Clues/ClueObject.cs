using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class ClueObject : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField]
    private GameObject _clueObjectUI;

    [SerializeField] private bool _disappearOnClick = true;

    private SpriteRenderer _spriteRenderer;
    private bool _found;

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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click the clue!");
        if (!_found)
        {
            Instantiate(_clueObjectUI);
        }
        if (_disappearOnClick)
        {
            Collect();
        }
    }

    [YarnCommand("collect_clue")]
    public void Collect()
    {
        gameObject.SetActive(false);
    }
}
