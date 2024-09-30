using UnityEngine;

[ExecuteInEditMode]
public class CurvedLineRenderer : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;
    public LineRenderer lineRenderer;
    public int curveSegments = 20;
    public float curveHeight = 1.0f;
    public Color gizmoColor = Color.green;
    public float lineWidth = 0.1f;
    public float offsetDistance = 4.0f;


    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // width of the line
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        //
        lineRenderer.positionCount = curveSegments + 2;
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            //moving 4 unit
            Vector3 positionA = objectA.transform.position + objectA.transform.forward * offsetDistance;
            Vector3 positionB = objectB.transform.position + objectB.transform.forward * offsetDistance;

            Vector3 midpoint = (positionA + positionB) / 2;
            Vector3 curveOffset = GetZAxisCurveOffset(positionA, positionB);

            DrawCurvedLine(positionA, positionB, midpoint + curveOffset);
        }
    }

    private void OnDrawGizmos()
    {
        if (objectA == null || objectB == null)
        {
            return;
        }

        Vector3 positionA = objectA.transform.position + objectA.transform.forward * offsetDistance;
        Vector3 positionB = objectB.transform.position + objectB.transform.forward * offsetDistance;

        Vector3 midpoint = (positionA + positionB) / 2;

        Vector3 curveOffset = GetZAxisCurveOffset(positionA, positionB);

        Gizmos.color = gizmoColor;

        DrawGizmoCurvedLine(positionA, positionB, midpoint + curveOffset);
    }

    // Bending offset in the Z-axis direction
    Vector3 GetZAxisCurveOffset(Vector3 positionA, Vector3 positionB)
    {
        Vector3 directionZ = (objectA.transform.forward + objectB.transform.forward).normalized;
        return directionZ * curveHeight;
    }

    void DrawCurvedLine(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint)
    {
        // Drawing curves from start to end, bending through control points
        for (int i = 0; i <= curveSegments + 1; i++)
        {
            float t = (float)i / (float)(curveSegments + 1); // Proportion of curved segments

            Vector3 curvePoint = Mathf.Pow(1 - t, 2) * startPoint +
                                 2 * (1 - t) * t * controlPoint +
                                 Mathf.Pow(t, 2) * endPoint;

            lineRenderer.SetPosition(i, curvePoint);
        }
    }

    // In editor mode
    void DrawGizmoCurvedLine(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint)
    {
        Vector3 previousPoint = startPoint;
        for (int i = 1; i <= curveSegments + 1; i++)
        {
            float t = (float)i / (float)(curveSegments + 1);

            // position of a Bessel curve point
            Vector3 curvePoint = Mathf.Pow(1 - t, 2) * startPoint +
                                 2 * (1 - t) * t * controlPoint +
                                 Mathf.Pow(t, 2) * endPoint;

            // Calculating the position of a Bessel curve point
            Gizmos.DrawLine(previousPoint, curvePoint);
            previousPoint = curvePoint;
        }
    }
}
