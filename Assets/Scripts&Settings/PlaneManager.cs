using UnityEngine;
using System.Collections.Generic;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab; // A prefab used to generate a flat surface

    private List<DynamicPlaneGenerator> planeGenerators = new List<DynamicPlaneGenerator>();

    // Create a plane
    public void CreatePlane(GameObject vector1Object, GameObject vector2Object, float fixedLength = 5.0f)
    {
        if (planePrefab == null)
        {
            Debug.LogError("Plane Prefab is not assigned in PlaneManager.");
            return;
        }

        GameObject planeObject = Instantiate(planePrefab, Vector3.zero, Quaternion.identity, transform);
        DynamicPlaneGenerator planeGenerator = planeObject.GetComponent<DynamicPlaneGenerator>();

        if (planeGenerator != null)
        {
            planeGenerator.vector1Object = vector1Object;
            planeGenerator.vector2Object = vector2Object;
            planeGenerator.fixedLength = fixedLength;

            planeGenerators.Add(planeGenerator);
        }
        else
        {
            Debug.LogError("Plane Prefab does not have a DynamicPlaneGenerator component.");
            Destroy(planeObject);
        }
    }

    // Destroy all planes
    public void ClearPlanes()
    {
        foreach (var planeGenerator in planeGenerators)
        {
            if (planeGenerator != null)
            {
                Destroy(planeGenerator.gameObject);
            }
        }

        planeGenerators.Clear();
    }
}