using System.Collections.Generic;
using UnityEngine;

public class LightBeamReflector : MonoBehaviour
{
    public LightBeamSO LightBeamSO;
    public Vector3 InitialDirection;
    public int ReflectionMultiplier = 20;
    private LineRenderer _lineRenderer;
    private LightBeamTarget _target;
    private bool _isTargetHit = false;
    private bool _isMirrorHit = false;
    private bool _isPortalHit = false;
    private GameObject _portalLightBeam;
    private List<string> _hitObjects = new List<string>();

    void Start()
    {
        InitializeComponents();
        CalculateReflection(transform.position, InitialDirection);
    }

    void Update()
    {
        if(_isTargetHit) return;

        ProcessRaycast();
    }

    private void InitializeComponents()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.startColor = LightBeamSO.LightBeamColor;
        _lineRenderer.endColor = LightBeamSO.LightBeamColor;
    }

    private void ProcessRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("LightBeam")))
        {
            HandleHit(hit);
        }
        else
        {
            HandleNoHit();
        }
    }

    private void HandleHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Mirror"))
        {
            ProcessMirrorHit();
        }
        if (hit.collider.CompareTag("Target"))
        {
            HandleTargetHit(hit);
        }
        if (hit.collider.CompareTag("Portal"))
        {
            HandlePortalHit(hit);
        }
    }

    private void HandleNoHit()
    {
        ResetLineRenderer();

        if (_isMirrorHit)
        {
            _isMirrorHit = false;
        }

        // if (_isPortalHit)
        // {
        //     if (_portalLightBeam != null)
        //     {
        //         Debug.Log("Portal Light Beam Destroyed 3");
        //         Destroy(_portalLightBeam);
        //     }

        //     _isPortalHit = false;
        // }

        CalculateReflection(transform.position, InitialDirection);
    }

    private void ProcessMirrorHit()
    {
        ResetLineRenderer();
        CalculateReflection(transform.position, InitialDirection);
    }

    private void ResetLineRenderer()
    {
        _hitObjects.Clear();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
    }

    private void CalculateReflection(Vector3 startPosition, Vector3 direction, int reflectionCount = 0)
    {
        Ray ray = new Ray(startPosition, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("LightBeam")))
        {
            if (reflectionCount >= LightBeamSO.ReflectionLimit && hit.collider.CompareTag("Mirror"))
            {
                AddLineRendererPosition(startPosition + direction * ReflectionMultiplier);
                return;
            }

            ProcessHit(hit, direction, reflectionCount);
            
            if(!_hitObjects.Contains(hit.collider.tag))
                _hitObjects.Add(hit.collider.tag);
        }
        else
        {
            AddLineRendererPosition(startPosition + direction * ReflectionMultiplier);
        }
    }

    private void ProcessHit(RaycastHit hit, Vector3 direction, int reflectionCount)
    {
        if (hit.collider.CompareTag("Mirror"))
        {
            HandleMirrorReflection(hit, direction, ++reflectionCount);

            if(_portalLightBeam != null && !_hitObjects.Contains("Portal"))
            {
                Destroy(_portalLightBeam);
                _portalLightBeam = null;
                _isPortalHit = false;
            }
        }
        else if (hit.collider.CompareTag("Target"))
        {
            HandleTargetHit(hit);
        }
        else if (hit.collider.CompareTag("Portal"))
        {
            HandlePortalHit(hit);
        }
        else
        {
            ResetLineRenderer();
            AddLineRendererPosition(hit.point);

            if(_portalLightBeam != null)
            {
                Destroy(_portalLightBeam);
            }
        }
    }

    private void HandleMirrorReflection(RaycastHit hit, Vector3 direction, int reflectionCount)
    {
        _isMirrorHit = true;
        Vector3 reflectDirection = Vector3.Reflect(direction, hit.normal);

        AddLineRendererPosition(hit.point);
        CalculateReflection(hit.point, reflectDirection, reflectionCount);
    }

    private void HandleTargetHit(RaycastHit hit)
    {
        if (!_isTargetHit)
        {
            AddLineRendererPosition(hit.point);
            _target = hit.collider.GetComponent<LightBeamTarget>();

            if (_target.ColorType == LightBeamSO.LightBeamColorType)
            {
                _target.IsReached = true;
                SectionEvent.Trigger(SectionEventType.SectionCompleted);
                InteractEvent.Trigger(InteractEventType.MirrorExit);
                _isTargetHit = true;
            }
        }
    }

    private void HandlePortalHit(RaycastHit hit)
    {
        AddLineRendererPosition(hit.point);
        
        if (!_isPortalHit)
        {
            _isPortalHit = true;
            var endPortal = hit.transform.parent.Find("PortalEnd");

            if (_portalLightBeam == null)
                _portalLightBeam = Instantiate(gameObject, endPortal.position, transform.rotation);
        }
    }

    private void AddLineRendererPosition(Vector3 position)
    {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);
    }
}
