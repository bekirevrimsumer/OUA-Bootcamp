using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class DrawCollider : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        lineRenderer.positionCount = 8;
        lineRenderer.loop = true;

        UpdateColliderBounds();
    }

    void Update()
    {
        UpdateColliderBounds();
    }

    private void UpdateColliderBounds()
    {
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;

        Vector3[] corners = new Vector3[8];
        corners[0] = transform.TransformPoint(center + new Vector3(size.x, size.y, size.z));
        corners[1] = transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z));
        corners[2] = transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z));
        corners[3] = transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z));
        corners[4] = transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z));
        corners[5] = transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z));
        corners[6] = transform.TransformPoint(center + new Vector3(-size.x, -size.y, -size.z));
        corners[7] = transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z));

        lineRenderer.SetPositions(new Vector3[] {
            corners[0], corners[1], corners[2], corners[3],
            corners[0], corners[4], corners[5], corners[1],
            corners[5], corners[6], corners[7], corners[4],
            corners[7], corners[6], corners[2], corners[3]
        });
    }
}
