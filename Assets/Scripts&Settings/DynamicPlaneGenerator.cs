using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DynamicPlaneGenerator : MonoBehaviour
{
    [SerializeField] private GameObject vector1Object; // The object of the first vector
    [SerializeField] private GameObject vector2Object; // The object of the second vector
    [SerializeField] private float fixedLength = 14.0f; // Fixed length and width

    private MeshFilter meshFilter;
    private Mesh mesh;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    void Update()
    {
        // Get the start, direction, and end points of the vector
        Vector3 origin1 = vector1Object.transform.position;
        Vector3 direction1 = vector1Object.transform.forward;
        Vector3 end1 = origin1 + direction1 * fixedLength;

        Vector3 origin2 = vector2Object.transform.position;
        Vector3 direction2 = vector2Object.transform.forward;
        Vector3 end2 = origin2 + direction2 * fixedLength;

        // Check whether the vector is connected
        if (AreVectorsConnected(origin1, end1, origin2, end2))
        {
            // Get coincident endpoints
            Vector3 sharedPoint = GetSharedPoint(origin1, end1, origin2, end2);

            // Calculate the normalized vector with a fixed length of value
            Vector3 vector1 = direction1.normalized * fixedLength;
            Vector3 vector2 = direction2.normalized * fixedLength;

            // Compute the four vertices of the plane
            Vector3[] vertices = new Vector3[4];
            vertices[0] = sharedPoint;
            vertices[1] = sharedPoint + vector1;
            vertices[2] = sharedPoint + vector2;
            vertices[3] = sharedPoint + vector1 + vector2;

            // Define a triangular index
            int[] triangles = new int[]
            {
                0, 1, 2,
                2, 1, 3
            };

            // Update the grid
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Activate MeshRenderer
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            // If the vector is not connected, the plane is hidden
            mesh.Clear();
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Check whether the vector is connected
    bool AreVectorsConnected(Vector3 origin1, Vector3 end1, Vector3 origin2, Vector3 end2)
    {
        float epsilon = 0.1f; // The permissible margin of error
        return (Vector3.Distance(origin1, origin2) < epsilon) ||
               (Vector3.Distance(origin1, end2) < epsilon) ||
               (Vector3.Distance(end1, origin2) < epsilon) ||
               (Vector3.Distance(end1, end2) < epsilon);
    }

    // Get coincident endpoints
    Vector3 GetSharedPoint(Vector3 origin1, Vector3 end1, Vector3 origin2, Vector3 end2)
    {
        float epsilon = 0.001f;
        if (Vector3.Distance(origin1, origin2) < epsilon) return origin1;
        if (Vector3.Distance(origin1, end2) < epsilon) return origin1;
        if (Vector3.Distance(end1, origin2) < epsilon) return end1;
        if (Vector3.Distance(end1, end2) < epsilon) return end1;

        return Vector3.zero;
    }
}