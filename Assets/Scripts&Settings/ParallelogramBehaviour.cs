using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallelogramBehaviour : MonoBehaviour {
    [SerializeField] GameObject redArrow;
    [SerializeField] GameObject blueArrow;
    [SerializeField] GameObject greenArrow;
    [SerializeField] int face = 0;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        transform.position = greenArrow.transform.position;
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4] {
            Vector3.zero,

            redArrow.transform.parent.position + redArrow.transform.forward * redArrow.transform.localScale.z,

            blueArrow.transform.parent.position + blueArrow.transform.forward * blueArrow.transform.localScale.z,

            greenArrow.transform.position + greenArrow.transform.forward * greenArrow.transform.localScale.z
        };
        Vector3[] moreVertices = new Vector3[4] {
            vertices[1] + vertices[2],
            vertices[2] + vertices[3],
            vertices[1] + vertices[3],
            vertices[1] + vertices[2] + vertices[3]
        };
        Vector3[] allVertices = new Vector3[8];
        for (int i = 0; i < 4; i++) {
            vertices[i] *= 14;
            moreVertices[i] *= 14;
            allVertices[i] = vertices[i];
            allVertices[i + 4] = moreVertices[i];
        }

        mesh.vertices = allVertices;

        int[][] triangles = {
            new int[] { 0, 1, 4, 0, 4, 2 },
            new int[] { 0, 1, 6, 0, 6, 3 },
            new int[] { 0, 2, 5, 0, 5, 3 },
            new int[] { 3, 6, 7, 3, 5, 7 },
            new int[] { 2, 4, 7, 2, 7, 5 },
            new int[] { 1, 4, 7, 1, 7, 6 }
        };
        mesh.triangles = triangles[face];


        MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent("MeshFilter");
        meshFilter.mesh = mesh;
    }
}