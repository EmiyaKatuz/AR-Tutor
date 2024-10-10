using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ParallelogramManager : MonoBehaviour
{
    [Header("Parallelogram Settings")] public Material doubleSidedWhiteMaterial;

    private List<ParallelogramGenerator> parallelograms = new List<ParallelogramGenerator>();

    public void GenerateParallelogram(Transform object1, Transform object2)
    {
        if (doubleSidedWhiteMaterial == null)
        {
            Debug.LogError("Double Sided White Material is not assigned in ParallelogramManager.");
            return;
        }

        GameObject parallelogramGO =
            ParallelogramGenerator.CreateParallelogram(object1, object2, doubleSidedWhiteMaterial, this.transform);
        ParallelogramGenerator generator = parallelogramGO.GetComponent<ParallelogramGenerator>();

        if (generator != null)
        {
            parallelograms.Add(generator);
        }
    }

    public void EnableAllParallelograms()
    {
        foreach (var pg in parallelograms)
        {
            pg.EnableGeneration();
        }
    }

    public void DisableAllParallelograms()
    {
        foreach (var pg in parallelograms)
        {
            pg.DisableGeneration();
        }
    }

    public void DeleteAllParallelograms()
    {
        foreach (var pg in parallelograms)
        {
            Destroy(pg.gameObject);
        }

        parallelograms.Clear();
    }
}