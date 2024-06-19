using UnityEngine;

public class LightReflection : MonoBehaviour
{
    public int MaxReflections = 5;
    private LineRenderer _lineRenderer;
    private Vector3 _oldMirrorPosition;
    private Quaternion _oldMirrorRotation;
    private Vector3 _initialDirection;

    void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, new Vector3(0, 0, 0));

        _initialDirection = transform.forward;
        CalculateReflection(transform.position, _initialDirection, MaxReflections);
    }

    void Update() 
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Mirror"))
            {
                if(_oldMirrorRotation != hit.transform.rotation || _oldMirrorPosition != hit.transform.position)
                {
                    _lineRenderer.positionCount = 1;
                    _lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                    CalculateReflection(transform.position, _initialDirection, MaxReflections);
                }
            }

            _oldMirrorPosition = hit.transform.position;
            _oldMirrorRotation = hit.transform.rotation;
        }
        else
        {
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
            CalculateReflection(transform.position, _initialDirection, MaxReflections);
        }
    }

    void CalculateReflection(Vector3 startPosition, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0) return;

        Ray ray = new Ray(startPosition, direction);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 hitPoint = hit.point;
            Vector3 reflectDirection = Vector3.Reflect(ray.direction, hit.normal);

            if (hit.collider.CompareTag("Target"))
            {
                return;
            }

            if (hit.collider.CompareTag("Mirror"))
            {
                var distance = Vector3.Distance(startPosition, hitPoint);
                AddLineRendererPosition(new Vector3(hitPoint.x, hitPoint.y, distance));
                CalculateReflection(hitPoint, reflectDirection, reflectionsRemaining - 1);
            }
        }
        else
        {
            Vector3 endPosition = startPosition + direction * 5;
            AddLineRendererPosition(endPosition);
        }
    }

    void AddLineRendererPosition(Vector3 position)
    {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);
    }
}
