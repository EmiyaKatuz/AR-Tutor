using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ParallelogramGenerator : MonoBehaviour
{
    public Transform cylinder1;
    public Transform cylinder2;

    public Material doubleSidedWhiteMaterial;

    private Mesh mesh;

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
    }

    void Update()
    {
        if (cylinder1 && cylinder2)
        {
            UpdateParallelogram();
        }
    }

    void UpdateParallelogram()
    {
        // 获取点的位置
        Vector3 A = GetTopCenter(cylinder1);
        Vector3 B = GetBottomCenter(cylinder1);
        Vector3 C = GetTopCenter(cylinder2);
        Vector3 D = GetBottomCenter(cylinder2);

        // 计算向量BA和向量DC
        Vector3 BA = A - B;
        Vector3 DC = C - D;
        Vector3 BD = D - B;
        // 为了生成平行四边形，选择一个共同的起点
        // 这里我们选择点B作为起点
        Vector3 origin = B;

        // 定义四个顶点
        Vector3 v0 = origin;
        Vector3 v1 = origin + BA;
        Vector3 v2 = origin + BD;
        Vector3 v3 = origin + BD + DC;
        Vector3 v4 = origin + BD + DC + BA;
        // 定义网格的顶点
        Vector3[] vertices = new Vector3[]
        {
            v0, v1, v2, v3, v4
        };

        // 定义三角形（双面）
        int[] triangles = new int[]
        {
            0, 1, 4, // 前面
            0, 2, 4, // 前面
            2, 3, 4, // 前面    
            0, 1, 4, // 后面
            0, 2, 4, // 后面
            2, 3, 4, // 后面 
        };

        // 分配网格数据
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    Vector3 GetBottomCenter(Transform cylinder)
    {
        // Unity默认圆柱体高度为2，考虑缩放
        float height = 2f * cylinder.localScale.y;
        return cylinder.position - cylinder.up * (height / 2f);
    }

    Vector3 GetTopCenter(Transform cylinder)
    {
        // Unity默认圆柱体高度为2，考虑缩放
        float height = 5f * cylinder.localScale.y;
        return cylinder.position + cylinder.up * (height / 2f);
    }

    public static GameObject CreateParallelogram(Transform cylinder1, Transform cylinder2, Material material,
        Transform parent = null)
    {
        GameObject parallelogram = new GameObject("Parallelogram");
        parallelogram.transform.parent = parent;

        ParallelogramGenerator generator = parallelogram.AddComponent<ParallelogramGenerator>();
        generator.cylinder1 = cylinder1;
        generator.cylinder2 = cylinder2;
        generator.doubleSidedWhiteMaterial = material;

        return parallelogram;
    }
}