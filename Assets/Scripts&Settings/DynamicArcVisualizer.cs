using UnityEngine;

public class DynamicArcVisualizer : MonoBehaviour
{
    public int segments = 50;
    public float radius = 5.0f;
    public float width = 1.00f;
    public Material arcMaterial;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = arcMaterial;
        lineRenderer.positionCount = segments + 1;
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
    }

    public void UpdateArc(Vector3 vector1, Vector3 vector2, Vector3 startPosition, float angle)
    {
        // Calculate normal vector
        Vector3 normal = Vector3.Cross(vector1, vector2);
        if (normal == Vector3.zero)
        {
            // Vectors are parallel, cannot compute normal vector
            lineRenderer.enabled = false; // Disable the line renderer to hide the arc
            return;
        }
        else
        {
            normal = normal.normalized;
            lineRenderer.enabled = true; // Enable the line renderer
        }

        // starting Direction Starting From Vector1
        Vector3 startDir = vector1.normalized;

        // Calculate the position of each point
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / (float)segments;
            float currentAngle = t * angle;
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, normal);
            Vector3 point = rotation * startDir * radius;
            lineRenderer.SetPosition(i, startPosition + point);
        }
    }
}