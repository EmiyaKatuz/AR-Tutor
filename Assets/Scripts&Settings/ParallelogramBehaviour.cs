using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelogramBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject redArrow;
    [SerializeField]
    GameObject blueArrow;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            Vector3.zero,
            redArrow.transform.position + redArrow.transform.forward * redArrow.transform.localScale.z,
            redArrow.transform.position + redArrow.transform.forward * redArrow.transform.localScale.z
            + blueArrow.transform.position + blueArrow.transform.forward * blueArrow.transform.localScale.z,
            blueArrow.transform.position + blueArrow.transform.forward * blueArrow.transform.localScale.z

        };
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] *= 14;
        }
        mesh.vertices = vertices;

        int[] triangles = new int[6]
        {
            // lower left triangle
            0, 1, 2,
            // upper right triangle
            2, 3, 0
        };
        mesh.triangles = triangles;


        MeshFilter meshFilter = (MeshFilter) gameObject.GetComponent("MeshFilter");
        meshFilter.mesh = mesh;
    }
}
