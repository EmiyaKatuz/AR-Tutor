using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VectorAngleVisualizer : MonoBehaviour
{
    public GameObject vectorObject1;
    public GameObject vectorObject2;
    public GameObject arcVisualizerPrefab;
    public Text angleText; // Text UI for displaying angle information
    private GameObject arcInstance;

    void Start()
    {
        // Make sure arcVisualizerPrefab is set
        if (arcVisualizerPrefab == null)
        {
            Debug.LogError("Please specify arcVisualizerPrefab in the Inspector.");
        }
    }

    void Update()
    {
        // Get vector
        Vector3 vector1 = GetVectorFromGameObject(vectorObject1);
        Vector3 vector2 = GetVectorFromGameObject(vectorObject2);

        // Calculate the angle
        float angle = Vector3.Angle(vector1, vector2);

        // Display angle information
        UpdateAngleText(angle);

        // Visualize the angle
        VisualizeAngle(vector1, vector2, vectorObject1.transform.position, angle);
    }

    Vector3 GetVectorFromGameObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("GameObject is empty.");
            return Vector3.zero;
        }

        // Get the direction vector, such as transform.forward
        Vector3 direction = obj.transform.forward.normalized;

        // Get the length of the model
        float length = GetModelLength(obj);

        // Calculate the actual vector
        Vector3 vector = direction * length;

        return vector;
    }

    float GetModelLength(GameObject obj)
    {
        // Initialize merged bounding boxes
        Bounds combinedBounds = new Bounds(obj.transform.position, Vector3.zero);

        // Traverse all child objects and find the part with Renderer component
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            foreach (Renderer renderer in renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds); // Merge bounding boxes
            }

            // Assume that the length of the model is along the Z-axis
            return combinedBounds.size.z;
        }
        else
        {
            Debug.LogWarning(obj.name + "Without Renderer component, length cannot be calculated。");
            return 1.0f; // If no Renderer is found, use the default length
        }
    }

    void VisualizeAngle(Vector3 vector1, Vector3 vector2, Vector3 startPosition, float angle)
    {
        if (arcVisualizerPrefab != null)
        {
            // If there is already an instance, destroy it first
            if (arcInstance != null)
            {
                Destroy(arcInstance);
            }

            // Instantiate Arc Prefab
            arcInstance = Instantiate(arcVisualizerPrefab, startPosition, Quaternion.identity);

            // Get the arc's script and update the arc's shape and direction
            DynamicArcVisualizer arcVisualizer = arcInstance.GetComponent<DynamicArcVisualizer>();
            if (arcVisualizer != null)
            {
                // Set the radius of the arc (can be adjusted as needed)
                arcVisualizer.SetRadius(1.0f);

                // Update arc shape and direction
                arcVisualizer.UpdateArc(vector1, vector2, startPosition, angle);
            }
            else
            {
                Debug.LogError("Prefab is missing the DynamicArcVisualizer script.");
            }
        }
        else
        {
            Debug.LogWarning("Please specify an arcVisualizerPrefab to visualize the angle.");
        }
    }

    // Update the Text on the screen to display the angle information
    void UpdateAngleText(float angle)
    {
        if (angleText != null)
        {
            angleText.text = "Angle: " + angle.ToString("F2") + "°";
        }
        else
        {
            Debug.LogWarning("Please set angleText in Inspector.");
        }
    }
}