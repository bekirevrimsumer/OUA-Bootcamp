using System.Collections.Generic;
using UnityEngine;

public class LightReflection : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Vector3 _initialDirection;
    private EnvironmentChanger _environmentChanger;
    private bool _isTargetHit = false;
    private bool _isMirrorHit = false;

    void Start()
    {
        InitializeComponents();
        CalculateReflection(transform.position, _initialDirection);
    }

    void Update()
    {
        ProcessRaycast();
    }

    private void InitializeComponents()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);

        _environmentChanger = GameObject.Find("EnvironmentChanger").GetComponent<EnvironmentChanger>();
        _initialDirection = transform.forward;
    }

    private void ProcessRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
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
            HandleTargetHit();
        }
    }

    private void HandleNoHit()
    {
        if (_isMirrorHit)
        {
            ResetLineRenderer();
            CalculateReflection(transform.position, _initialDirection);
            _isMirrorHit = false;
        }
    }

    private void ProcessMirrorHit()
    {
        ResetLineRenderer();
        CalculateReflection(transform.position, _initialDirection);
    }

    private void ResetLineRenderer()
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
    }

    private void CalculateReflection(Vector3 startPosition, Vector3 direction)
    {
        Ray ray = new Ray(startPosition, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        if (hits.Length > 0)
        {
            ProcessHits(hits, direction);
        }
        else
        {
            AddLineRendererPosition(startPosition + direction * 5);
        }
    }

    private void ProcessHits(RaycastHit[] hits, Vector3 direction)
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Mirror"))
            {
                HandleMirrorReflection(hit, direction);
                return;
            }
            else if (hit.collider.CompareTag("Target"))
            {
                HandleTargetHit();
                AddLineRendererPosition(hit.point);
            }
        }
    }

    private void HandleMirrorReflection(RaycastHit hit, Vector3 direction)
    {
        _isMirrorHit = true;
        Vector3 reflectDirection = Vector3.Reflect(direction, hit.normal);

        AddLineRendererPosition(hit.point);
        CalculateReflection(hit.point, reflectDirection);
    }

    private void HandleTargetHit()
    {
        if(!_isTargetHit)
        {
            LightReflectionEvent.Trigger(LightReflectionEventType.HitTarget);
            _environmentChanger.ChangeEnvironment();
            _isTargetHit = true;
        }
    }

    private void AddLineRendererPosition(Vector3 position)
    {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);
    }
}
