using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ParallelogramGenerator : MonoBehaviour
{
    public Transform object1;
    public Transform object2;

    public Material doubleSidedWhiteMaterial;

    private bool _parallelogramIsEnabled = false;
    private Mesh mesh;
    private static int parallelogramCount = 0;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if (!doubleSidedWhiteMaterial)
        {
            Debug.LogError("Double Sided White Material is not assigned.");
        }
        else
        {
            GetComponent<MeshRenderer>().material = doubleSidedWhiteMaterial;
        }

        GetComponent<MeshRenderer>().enabled = _parallelogramIsEnabled;
        UpdateParallelogram();
    }

    void Update()
    {
        if (_parallelogramIsEnabled && object1 && object2)
        {
            UpdateParallelogram();
        }
    }

    public void EnableGeneration()
    {
        _parallelogramIsEnabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        UpdateParallelogram();
    }

    public void DisableGeneration()
    {
        _parallelogramIsEnabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    void UpdateParallelogram()
    {
        // Gets the location of the point
        Vector3 A = GetBottomCenter(object1);
        Vector3 B = GetTopCenter(object1);
        Vector3 C = GetBottomCenter(object2);
        Vector3 D = GetTopCenter(object2);

        Vector3 BA = A - B;
        Vector3 DC = C - D;
        Vector3 BD = D - B;

        Vector3 origin = B;

        Vector3 v0 = origin;
        Vector3 v1 = origin + BA;
        Vector3 v2 = origin + BD;
        Vector3 v3 = origin + BD + DC;
        Vector3 v4 = origin + BD + DC + BA;

        Vector3[] vertices = new Vector3[]
        {
            v0, v1, v2, v3, v4
        };

        int[] triangles = new int[]
        {
            0, 1, 4,
            0, 2, 4,
            2, 3, 4,
            0, 1, 4,
            0, 2, 4,
            2, 3, 4,
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    Vector3 GetBottomCenter(Transform obj)
    {
        Vector3 top = obj.position;
        foreach (Transform child in obj)
        {
            if (child.position.y > top.y)
            {
                top = child.position;
            }
        }

        return top;
    }

    Vector3 GetTopCenter(Transform obj)
    {
        Vector3 bottom = obj.position;
        foreach (Transform child in obj)
        {
            if (child.position.y < bottom.y)
            {
                bottom = child.position;
            }
        }

        return bottom;
    }

    public static GameObject CreateParallelogram(Transform object1, Transform object2, Material material,
        Transform parent = null)
    {
        parallelogramCount++;
        string name = $"Parallelogram_{parallelogramCount}";
        GameObject parallelogram = new GameObject(name);
        parallelogram.transform.parent = parent;

        ParallelogramGenerator generator = parallelogram.AddComponent<ParallelogramGenerator>();
        generator.object1 = object1;
        generator.object2 = object2;
        generator.doubleSidedWhiteMaterial = material;

        return parallelogram;
    }
}