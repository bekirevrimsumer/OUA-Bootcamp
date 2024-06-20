using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineZone : MonoBehaviour
{

    [Header("Virtual Camera")]
    public bool CameraStartsActive = true;
    public CinemachineVirtualCamera VirtualCamera;

    [Header("Collider")]
    public LayerMask TriggerMask;

    public bool UseConfiner = false;

    [Header("State")]
    public bool CurrentState = false;
    public bool StateVisited = false;

    [Header("Activation")]
    public List<GameObject> ActivationObjects;

    [Header("Debug")]
    public bool DrawGizmos = true;
    public Color GizmoColor = Color.green;

    protected GameObject _confinerObject;
    protected Vector3 _gizmoSize;

    protected Collider _collider;
    protected Collider _confinerCollider;
    protected Rigidbody _confinerRigidbody;
    protected BoxCollider _boxCollider;
    protected SphereCollider _sphereCollider;
    protected CinemachineConfiner _cinemachineConfiner;

    [Header("Top Down Camera")]
    public bool RequiresPlayerCharacter = true;
    protected CinemachineCameraController _cinemachineCameraController;
    protected CharacterController _character;

    protected virtual void Awake()
    {
        AlwaysInitialization();
        if (!Application.isPlaying)
        {
            return;
        }
        Initialization();

        if (Application.isPlaying)
        {
            _cinemachineCameraController = VirtualCamera.gameObject.GetComponentInChildren<CinemachineCameraController>();

            if (_cinemachineCameraController == null)
            {
                _cinemachineCameraController = VirtualCamera.gameObject.GetComponentInParent<CinemachineCameraController>();
            }
            if (_cinemachineCameraController == null)
            {
                _cinemachineCameraController = VirtualCamera.gameObject.AddComponent<CinemachineCameraController>();
            }
        }
    }

    protected virtual void AlwaysInitialization()
    {
        InitializeCollider();
    }

    protected virtual void Initialization()
    {
        if (VirtualCamera == null)
        {
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        if (VirtualCamera == null)
        {
            Debug.LogWarning("Sanal kamera childrenlarda bulunamadı: " + gameObject.name);
        }

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

        if (UseConfiner)
        {
            SetupConfiner();
        }

        StartCoroutine(EnableCamera(CameraStartsActive, 1));
    }

    protected virtual void InitializeCollider()
    {
        _collider = GetComponent<Collider>();
        _boxCollider = GetComponent<BoxCollider>();
        _sphereCollider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    protected virtual void SetupConfiner()
    {
        _confinerRigidbody = _confinerObject.AddComponent<Rigidbody>();
        _confinerRigidbody.useGravity = false;
        _confinerRigidbody.gameObject.isStatic = true;
        _confinerRigidbody.isKinematic = true;

        CopyCollider();
        _confinerObject.transform.localPosition = Vector3.zero;

        _cinemachineConfiner = VirtualCamera.gameObject.GetComponent<CinemachineConfiner>();
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
        SetupConfiner();
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

    protected virtual IEnumerator EnableCamera(bool state, int frames)
    {
        if (VirtualCamera == null)
        {
            yield break;
        }
        while (frames > 0)
        {
            frames--;
            yield return null;
        }

        VirtualCamera.enabled = state;

        if (state)
        {
            _cinemachineCameraController.FollowsAPlayer = true;
            _cinemachineCameraController.StartFollowing();
        }
        else
        {
            _cinemachineCameraController.StopFollowing();
            _cinemachineCameraController.FollowsAPlayer = false;
        }
    }

    protected virtual void EnterZone()
    {
        CurrentState = true;
        StateVisited = true;

        StartCoroutine(EnableCamera(true, 0));
        foreach (GameObject go in ActivationObjects)
        {
            go.SetActive(true);
        }
    }

    protected virtual void ExitZone()
    {
        CurrentState = false;
        StartCoroutine(EnableCamera(false, 0));
        foreach (GameObject go in ActivationObjects)
        {
            go.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if ((TriggerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
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
