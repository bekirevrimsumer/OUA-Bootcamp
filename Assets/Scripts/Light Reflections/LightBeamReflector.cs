using UnityEngine;

public class LightBeamReflector : MonoBehaviour
{
    public LightBeamSO LightBeamSO;
    private LineRenderer _lineRenderer;
    private Vector3 _initialDirection;
    private bool _isTargetHit = false;
    private Target _target;
    private bool _isMirrorHit = false;
    private const int _reflectionMultiplier = 10;

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
        _lineRenderer.startColor = LightBeamSO.LightBeamColor;
        _lineRenderer.endColor = LightBeamSO.LightBeamColor;

        _initialDirection = transform.forward;
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

    private void CalculateReflection(Vector3 startPosition, Vector3 direction, int reflectionCount = 0)
    {
        if (reflectionCount >= LightBeamSO.ReflectionLimit)
        {
            AddLineRendererPosition(startPosition + direction * _reflectionMultiplier);
            return;
        }

        Ray ray = new Ray(startPosition, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("LightBeam")))
        {
            ProcessHit(hit, direction, reflectionCount);
        }
        else
        {
            AddLineRendererPosition(startPosition + direction * _reflectionMultiplier);
        }
    }

    private void ProcessHit(RaycastHit hit, Vector3 direction, int reflectionCount)
    {
        if (hit.collider.CompareTag("Mirror"))
        {
            HandleMirrorReflection(hit, direction, ++reflectionCount);
        }
        else if (hit.collider.CompareTag("Target"))
        {
            HandleTargetHit(hit);
            AddLineRendererPosition(hit.point);
        }
        else
        {
            ResetLineRenderer();
            AddLineRendererPosition(hit.point);
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
        if(!_isTargetHit)
        {
            _target = hit.collider.GetComponent<Target>();

            if(_target.colorType == LightBeamSO.LightBeamColorType)
            {
                Debug.Log("Target hit");
                LightReflectionEvent.Trigger(LightReflectionEventType.HitTarget);
                _isTargetHit = true;
            }
        }
    }

    private void AddLineRendererPosition(Vector3 position)
    {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);
    }
}
