using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineZone : MonoBehaviour
{

    [Header("Virtual Camera")]
    public CinemachineVirtualCamera CurrentVirtualCamera;

    [Header("Collider")]
    public LayerMask TriggerMask;
    public bool UseConfiner = false;
    public float ConfinerSize = 1;
    private static float _defaultSize = 13; 

    [Header("State")]
    public bool CurrentState = false;
    public bool StateVisited = false;
    private Coroutine _coroutine;

    [Header("Activation")]
    public List<GameObject> ActivationObjects;

    [Header("Debug")]
    public bool DrawGizmos = true;
    public Color GizmoColor = Color.green;

    protected GameObject _confinerObject;
    protected Vector3 _gizmoSize;

    protected Collider _collider;
    protected Collider _confinerCollider;
    protected BoxCollider _boxCollider;
    protected SphereCollider _sphereCollider;
    protected CinemachineConfiner _cinemachineConfiner;

    protected virtual void Awake()
    {
        AlwaysInitialization();
        if (!Application.isPlaying)
        {
            return;
        }
        Initialization();
    }

    protected virtual void AlwaysInitialization()
    {
        InitializeCollider();
    }

    protected virtual void Initialization()
    {
        if (UseConfiner)
        {
            SetupConfinerGameObject();
        }

        foreach (GameObject go in ActivationObjects)
        {
            go.SetActive(false);
        }
    }

    protected virtual void Start()
    {
        if (!Application.isPlaying)
        {
            return;
        }
    }

    protected virtual void InitializeCollider()
    {
        _collider = GetComponent<Collider>();
        _boxCollider = GetComponent<BoxCollider>();
        _sphereCollider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    protected virtual void SetupConfiner(CinemachineVirtualCamera virtualCamera)
    {
        CopyCollider();
        _confinerObject.transform.localPosition = Vector3.zero;

        _cinemachineConfiner = virtualCamera.gameObject.GetComponent<CinemachineConfiner>();
        _cinemachineConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
        _cinemachineConfiner.m_ConfineScreenEdges = true;

        if (_boxCollider != null)
        {
            _cinemachineConfiner.m_BoundingVolume = _boxCollider;
        }

        if (_sphereCollider != null)
        {
            _cinemachineConfiner.m_BoundingVolume = _sphereCollider;
        }
    }

    protected virtual void ManualSetupConfiner()
    {
        Initialization();
    }

    protected virtual void SetupConfinerGameObject()
    {
        Transform child = this.transform.Find("Confiner");
        if (child != null)
        {
            DestroyImmediate(child.gameObject);
        }

        _confinerObject = new GameObject();
        _confinerObject.transform.localPosition = Vector3.zero;
        _confinerObject.transform.SetParent(transform);
        _confinerObject.name = "Confiner";
    }

    protected virtual void CopyCollider()
    {
        if (_boxCollider != null)
        {
            BoxCollider boxCollider = _confinerObject.AddComponent<BoxCollider>();
            boxCollider.size = _boxCollider.size;
            boxCollider.center = _boxCollider.center;
            boxCollider.isTrigger = true;
        }

        if (_sphereCollider != null)
        {
            SphereCollider sphereCollider = _confinerObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.center = _sphereCollider.center;
            sphereCollider.radius = _sphereCollider.radius;
        }
    }

    protected virtual void EnterZone()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = StartCoroutine(SmoothCameraDistance(ConfinerSize, false));
        SetupConfiner(CurrentVirtualCamera);
        CurrentState = true;
        StateVisited = true;

        foreach (GameObject go in ActivationObjects)
        {
            go.SetActive(true);
        }
    }

    protected virtual void ExitZone()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = StartCoroutine(SmoothCameraDistance(_defaultSize, true));
        CurrentState = false;
        foreach (GameObject go in ActivationObjects)
        {
            go.SetActive(false);
        }

        if (UseConfiner)
        {
            CurrentVirtualCamera.transform.GetComponent<CinemachineConfiner>().m_BoundingVolume = null;
            CurrentVirtualCamera.transform.GetComponent<CinemachineConfiner>().m_Damping = 4f;
        }
    }

    private IEnumerator SmoothCameraDistance(float targetDistance, bool isExitZone)
    {
        float currentDistance = CurrentVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        float elapsedTime = 0;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            CurrentVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = Mathf.Lerp(currentDistance, targetDistance, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if(isExitZone)
            CurrentVirtualCamera = null;
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if ((TriggerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            CurrentVirtualCamera = collider.transform.parent.Find("BaseCamera").GetComponent<CinemachineVirtualCamera>();
            EnterZone();
        }
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if ((TriggerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            ExitZone();
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (!DrawGizmos)
        {
            return;
        }

        Gizmos.color = GizmoColor;

        if ((_boxCollider != null) && _boxCollider.enabled)
        {
            _gizmoSize = _boxCollider.bounds.size;
            Gizmos.DrawCube(_boxCollider.bounds.center, _gizmoSize);
        }
        if (_sphereCollider != null && _sphereCollider.enabled)
        {
            Gizmos.DrawSphere(this.transform.position + _sphereCollider.center, _sphereCollider.radius);
        }
    }

    protected virtual void Reset()
    {
        GizmoColor = Color.green;
        GizmoColor.a = 0.2f;
    }
}
