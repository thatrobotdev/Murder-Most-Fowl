using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;

public class CameraController : Singleton<CameraController>
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float cameraSensitivity = 0.5f;
    [SerializeField]
    private float zoomSensitivity = 0.5f;
    [SerializeField]
    private float panSensitivity = 5f;

    [SerializeField]
    private float distanceFrom = 5.0f;
    [SerializeField]
    private LayerMask _hitMask;


    private CinemachineBrain _cinemachineBrain;
    private CinemachineVirtualCamera _cinemachineCam;
    private Ray _cameraRayOut;
    private RaycastHit2D _closestHit;

    private Transform _cameraTransform;
    private Vector3 _defaultPos;
    private Vector3 _currentPos;
    private Vector3 _panMove;
    private bool _panCamera;
    
    private float _distanceFromInitial;

    public Camera MainCamera
    {
        get => mainCamera;
        private set => mainCamera = value;
    }
    public Transform CameraTransform {
        get => mainCamera.transform;
    }
    public Ray CameraRay {
        get => _cameraRayOut;
    }
    
    public RaycastHit2D ClosestHit
    {
        get => _closestHit;
        private set => _closestHit = value;
    }
    public LayerMask HitMask
    {
        get => _hitMask;
        set => _hitMask = value;
    }

    public event Action<GameObject> ClickAction;
    public event Action<GameObject> PointerEnterAction;
    public event Action<GameObject> PointerExitAction;

    private void Awake()
    {
        InitializeSingleton();
        _cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
        //SeparateCameraObject();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCamera();
    }

    private void LateUpdate()
    {
        // _cinemachineCam.m_Lens.OrthographicSize = distanceFrom;
        // uiCamera.orthographicSize = distanceFrom;
    }
    private void OnEnable()
    {
        CoroutineUtils.ExecuteAfterEndOfFrame(ActivateCamera, this);
    }
    private void OnDisable()
    {
        DeactivateCamera();
    }

    private void ActivateCamera() {
        //InputController.Instance.RightHold += StartRotateCamera;
        //InputController.Instance.RightCancel += StopRotateCamera;
        InputController.Instance.LeftClick += ScreenClick;
        InputController.Instance.Hover += Hover;
    }

    private void DeactivateCamera() {
        //InputController.Instance.RightHold -= StartRotateCamera;
        //InputController.Instance.RightCancel -= StopRotateCamera;
        InputController.Instance.LeftClick -= ScreenClick;
        InputController.Instance.Hover -= Hover;
    }

    private void SetCamera()
    {
        _cinemachineCam = _cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        _distanceFromInitial = _cinemachineCam.m_Lens.OrthographicSize;
        distanceFrom = _distanceFromInitial;

        _panCamera = false;
    }

    public void StartPanCamera()
    {
        InputController.Instance.MouseMove += PanCamera;
        _panCamera = true;
    }
    public void StopPanCamera()
    {
        InputController.Instance.MouseMove -= PanCamera;
        _panCamera = false;
    }
    public void PanCamera(Vector2 mouseDelta)
    {

    }
    public void Hover(Vector2 screenPos)
    {
        Debug.Log("Hover Camera");
        //_cameraUIRayOut = MainCamera.WorldToViewportPoint()
        _cameraRayOut = MainCamera.ScreenPointToRay(screenPos);
        RaycastHit2D hit = SetHit(_cameraRayOut);
        if (!hit.transform)
        {
            if (_closestHit.transform) {
                PointerEnterAction?.Invoke(null);
                PointerExitAction?.Invoke(_closestHit.transform.gameObject);
                _closestHit = hit;
            }
            return;
        }
        if (_closestHit.transform && (hit.transform.gameObject.Equals(_closestHit.transform.gameObject) || _closestHit.transform.gameObject.layer == LayerMask.GetMask("UI")))
        {
            return;
        }
        PointerEnterAction?.Invoke(hit.transform.gameObject);
        if (_closestHit.transform)
        {
            PointerExitAction?.Invoke(_closestHit.transform.gameObject);
        }
        _closestHit = hit;
    }

    private RaycastHit2D SetHit(Ray ray)
    {
        //GraphicRaycaster raycaster;
        //raycaster.Raycast();
        RaycastHit2D[] cameraRayHits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, HitMask.value);
        float closestDistance = Mathf.Infinity;
        RaycastHit2D hit = new();
        string test = "";
        int x = 1;

        float ray_z = ray.origin.z;
        foreach (RaycastHit2D cameraRayHit in cameraRayHits)
        {
            Transform rayTransform = cameraRayHit.transform;
            float angle = Vector3.Angle(ray.direction, rayTransform.up);
            float dot = Vector3.Dot(ray.direction, rayTransform.up);

            float cameraRay_z = cameraRayHit.transform.position.z;
            float cameraRayDist = Math.Abs(cameraRay_z - ray_z);

            test += (x + ". " + cameraRayHit.transform.gameObject.name + "; Distance: " + cameraRayHit.distance + "; Dot: " + dot + " ||| ");
            x++;
            if (cameraRayDist < closestDistance && cameraRayDist > mainCamera.nearClipPlane)
            {
                hit = cameraRayHit;
                closestDistance = cameraRayDist;
            }
        }
        //Debug.Log(test);
        return hit;
    }

    public void ScreenClick()
    {
        if (_closestHit.Equals(new RaycastHit2D()))
        {
            ClickAction?.Invoke(null);
            Debug.Log($"Click gameobject: null");
            return;
        }
        GameObject hitGO = _closestHit.transform.gameObject;
        ClickAction?.Invoke(hitGO);

        Debug.Log(hitGO);
    }

    private void SeparateCameraObject()
    {
        transform.GetChild(0).parent = null;
    }

    public void ChangePosition(Vector3 selectedObject)
    {
        _currentPos = selectedObject;
    }

    public Vector3 GetCursorWorldPos() {
        return MainCamera.ScreenToWorldPoint(new Vector3(InputController.Instance.ScreenPosition.x, InputController.Instance.ScreenPosition.x, MainCamera.nearClipPlane));
    }
}
