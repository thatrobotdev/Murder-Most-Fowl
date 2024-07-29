using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using System;
using Unity.VisualScripting;

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

    private float _distanceFromInitial;
    [SerializeField]
    private float distanceFrom = 5.0f;

    private CinemachineBrain _cinemachineBrain;
    private CinemachineVirtualCamera _cinemachineCam;
    private Ray _cameraRayOut;
    private RaycastHit _closestHit;
    private Transform _cameraTransform;
    private Vector3 _defaultPos;
    private Vector3 _currentPos;
    private Vector3 _panMove;
    private bool _panCamera;
    private LayerMask _hitMask;

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
    
    public RaycastHit ClosestHit
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
    public event Action<GameObject> HoverAction;

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
        DeactivateCamera();
        ActivateCamera();
    }

    private void LateUpdate()
    {
        // _cinemachineCam.m_Lens.OrthographicSize = distanceFrom;
        // uiCamera.orthographicSize = distanceFrom;
    }
    private void OnEnable()
    {
        if (InputController.Instance != null) {
            DeactivateCamera();
            ActivateCamera();
        }
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
        _cameraRayOut = MainCamera.ScreenPointToRay(screenPos);
        RaycastHit hit = SetHit(_cameraRayOut);
        if (hit.Equals(new RaycastHit()))
        {
            HoverAction?.Invoke(null);
            return;
        }
        if (hit.transform.gameObject.Equals(_closestHit.transform.gameObject))
        {
            return;
        }
        _closestHit = hit;
        HoverAction?.Invoke(_closestHit.transform.gameObject);
    }

    private RaycastHit SetHit(Ray ray)
    {
        RaycastHit[] cameraRayHits = Physics.RaycastAll(ray, Mathf.Infinity, HitMask);
        float closestDistance = Mathf.Infinity;
        RaycastHit hit = new();
        string test = "";
        int x = 1;
        foreach (RaycastHit cameraRayHit in cameraRayHits)
        {
            Transform rayTransform = cameraRayHit.transform;
            float angle = Vector3.Angle(ray.direction, rayTransform.up);
            float dot = Vector3.Dot(ray.direction, rayTransform.up);

            test += (x + ". " + cameraRayHit.transform.gameObject.name + "; Distance: " + cameraRayHit.distance + "; Dot: " + dot + " ||| ");
            x++;
            if (cameraRayHit.distance < closestDistance && cameraRayHit.distance > mainCamera.nearClipPlane)
            {
                hit = cameraRayHit;
                closestDistance = cameraRayHit.distance;
            }
        }
        //Debug.Log(test);
        return hit;
    }

    public void ScreenClick()
    {
        if (_closestHit.Equals(new RaycastHit()))
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
