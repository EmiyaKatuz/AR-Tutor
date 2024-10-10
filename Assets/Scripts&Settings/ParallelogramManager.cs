using UnityEngine;
using System.Collections.Generic;

public class ParallelogramManager : MonoBehaviour
{
    [Header("Parallelogram Settings")] public Material doubleSidedWhiteMaterial;

    private List<ParallelogramGenerator> parallelograms_new = new List<ParallelogramGenerator>();

    void Start()
    {
        if (!Application.isPlaying)
            return;
        foreach (Transform child in transform)
        {
            ParallelogramGenerator generator = child.GetComponent<ParallelogramGenerator>();
            if (generator != null)
            {
                parallelograms_new.Add(generator);
            }
        }
    }

    public bool HasParallelograms()
    {
        return parallelograms_new.Count > 0;
    }

    public void GenerateParallelogram(Transform object1, Transform object2)
    {
        if (!Application.isPlaying)
            return;
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
            parallelograms_new.Add(generator);
        }
    }

    public void EnableAllParallelograms()
    {
        foreach (var pg in parallelograms_new)
        {
            pg.EnableGeneration();
        }
    }

    public void DisableAllParallelograms()
    {
        foreach (var pg in parallelograms_new)
        {
            pg.DisableGeneration();
        }
    }

    public void DeleteAllParallelograms()
    {
        foreach (var pg in parallelograms_new)
        {
            if (pg)
            {
                Destroy(pg.gameObject);
            }
        }

        parallelograms_new.Clear();
    }

    public void EnableSelectedParallelograms(string selectedNames)
    {
        foreach (var pg in parallelograms_new)
        {
            if (pg)
            {
                if (selectedNames.Contains(pg.gameObject.name))
                {
                    pg.EnableGeneration();
                }
                else
                {
                    pg.DisableGeneration();
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        DeleteAllParallelograms();
    }

    void OnDisable()
    {
        DeleteAllParallelograms();
    }
}