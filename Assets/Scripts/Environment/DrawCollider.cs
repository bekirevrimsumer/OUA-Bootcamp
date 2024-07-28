using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class DrawCollider : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;
    private Vector3[] corners;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        lineRenderer.positionCount = 8;
        lineRenderer.loop = true;

        UpdateColliderBounds();
    }

    private void UpdateColliderBounds()
    {
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size / 2;

        corners = new Vector3[8];
        corners[0] = center + new Vector3(size.x, size.y, size.z);
        corners[1] = center + new Vector3(size.x, size.y, -size.z);
        corners[2] = center + new Vector3(size.x, -size.y, -size.z);
        corners[3] = center + new Vector3(size.x, -size.y, size.z);
        corners[4] = center + new Vector3(-size.x, size.y, size.z);
        corners[5] = center + new Vector3(-size.x, size.y, -size.z);
        corners[6] = center + new Vector3(-size.x, -size.y, -size.z);
        corners[7] = center + new Vector3(-size.x, -size.y, size.z);

        lineRenderer.positionCount = 16;
        lineRenderer.SetPositions(new Vector3[] {
            transform.TransformPoint(corners[0]), transform.TransformPoint(corners[1]),
            transform.TransformPoint(corners[1]), transform.TransformPoint(corners[2]),
            transform.TransformPoint(corners[2]), transform.TransformPoint(corners[3]),
            transform.TransformPoint(corners[3]), transform.TransformPoint(corners[0]),
            transform.TransformPoint(corners[4]), transform.TransformPoint(corners[5]),
            transform.TransformPoint(corners[5]), transform.TransformPoint(corners[6]),
            transform.TransformPoint(corners[6]), transform.TransformPoint(corners[7]),
            transform.TransformPoint(corners[7]), transform.TransformPoint(corners[4]),
            transform.TransformPoint(corners[0]), transform.TransformPoint(corners[4]),
            transform.TransformPoint(corners[1]), transform.TransformPoint(corners[5]),
            transform.TransformPoint(corners[2]), transform.TransformPoint(corners[6]),
            transform.TransformPoint(corners[3]), transform.TransformPoint(corners[7]),
            
        });
    }
}
