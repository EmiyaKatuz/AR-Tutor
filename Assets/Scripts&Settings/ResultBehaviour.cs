using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;


public class ResultBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshPro textObject;
    [SerializeField] TextMeshPro testObject;
    [SerializeField] GameObject redArrow;
    [SerializeField] GameObject blueArrow;
    [SerializeField] GameObject normalArrow;
    [SerializeField] GameObject point;
    [SerializeField] GameObject[] parallelograms = new GameObject[6];
    [SerializeField] GameObject dashedLinePrefab; // Added: Dashed Prefab reference
    [SerializeField] GameObject arcVisualizer;

    private List<GameObject> dashedLineInstances = new List<GameObject>();
    public bool test = false;
    private string outputText;
    public FunctionData CurrentFunctionData { get; private set; }

    private void Start()
    {
        arcVisualizer.SetActive(false);
    }

    public void UpdateResult(FunctionData data)
    {
        CurrentFunctionData = data;

        if (CurrentFunctionData == null)
        {
            Debug.LogWarning("FunctionData is null.");
            return;
        }

        // Reset positions and initializations
        ResetPosition();

        // Disable or reset elements as needed
        if (CurrentFunctionData.mode != 1 && CurrentFunctionData.mode != 6 && CurrentFunctionData.mode != 7)
        {
            foreach (var t in parallelograms)
            {
                t.SetActive(false);
            }
        }

        DisableDashedLine();

        // Perform initial calculation
        CalculateResult();
    }

    // Update is called once per frame
    private void CalculateResult()
    {
        Vector3 redVector = redArrow.transform.forward;
        Vector3 blueVector = blueArrow.transform.forward;
        Vector3 greenVector = transform.forward;
        int mode = CurrentFunctionData.mode;
        string text = "";
        DisableDashedLine();
        switch (mode)
        {
            case 0: // Dot Product
                text = "Dot Product:\n" + Math.Round(Vector3.Dot(redVector, blueVector), 2);
                point.SetActive(false);
                normalArrow.SetActive(false);
                arcVisualizer.SetActive(true);
                break;
            case 1: // Cross Product + Parallelogram
                for (int i = 1; i < parallelograms.Length; i++)
                {
                    parallelograms[i].SetActive(false);
                }

                parallelograms[0].SetActive(true);
                greenVector = Vector3.Cross(blueVector, redVector);
                text = "Area:\n" + Math.Round(Vector3.Magnitude(greenVector), 2);
                point.SetActive(false);
                arcVisualizer.SetActive(true);
                break;
            case 2: // Vector Addition
                blueArrow.transform.localPosition = redArrow.transform.forward * 14;
                greenVector = redVector + blueVector;
                point.SetActive(false);
                normalArrow.SetActive(false);
                arcVisualizer.SetActive(true);
                break;
            case 3: // Vector Subtraction
                transform.localPosition = blueArrow.transform.forward * 14;
                greenVector = redVector - blueVector;
                point.SetActive(false);
                normalArrow.SetActive(false);
                arcVisualizer.SetActive(true);
                break;
            case 4: // Projection
                greenVector = Vector3.Project(redVector, blueVector);
                // Calculate the endpoint position of redArrow
                Vector3 redArrowEndPoint = redArrow.transform.position +
                                           redArrow.transform.forward * 14;
                // Calculate the endpoint positions of the projection vectors
                Vector3 projectionEndPoint =
                    redArrow.transform.position + greenVector * 14;
                // Enable and update dotted lines
                EnableDashedLine(redArrowEndPoint, projectionEndPoint);
                arcVisualizer.SetActive(true);
                break;
            case 5: // Point to line
                // Getting the start and direction of a line
                Vector3 lineOrigin = blueArrow.transform.position;
                Vector3 lineDirection = blueArrow.transform.forward.normalized;
                // Calculate the endpoint position of redArrow
                Vector3 redArrowPoint = redArrow.transform.position + redArrow.transform.forward * 14;
                // Set the position of point to the endpoint of redArrow.
                point.transform.position = redArrowPoint;
                // Get the position of point
                Vector3 pointPosition = point.transform.position;
                // Calculate the vector from the starting point of the line to the point
                Vector3 originToPoint = pointPosition - lineOrigin;
                // Project the above vectors in the direction of the line to get the projection vector on the line
                Vector3 projection = Vector3.Project(originToPoint, lineDirection);
                // Calculate the coordinates of the point on the line that is closest to the point
                Vector3 closestPointOnLine = lineOrigin + projection;
                // Calculate the shortest distance from a point to a line
                float distance = Vector3.Distance(pointPosition, closestPointOnLine);
                // Update display text to show distance information
                text = "Distance: " + Math.Round(distance, 2);
                // Draws a dotted line from point to the nearest point on the line.
                EnableDashedLine(pointPosition, closestPointOnLine);

                // Get the length of blueArrow
                float blueArrowLength = blueArrow.transform.localScale.z * 14;
                // Calculate the start and end points of blueArrow
                Vector3 blueArrowStart = blueArrow.transform.position;
                Vector3 blueArrowEnd = blueArrowStart + blueArrow.transform.forward * blueArrowLength;
                // Calculate the vector from the projected point to blueArrowStart.
                Vector3 startToProjection = closestPointOnLine - blueArrowStart;
                // Calculate the direction vector and length of blueArrow.
                Vector3 blueArrowDirection = (blueArrowEnd - blueArrowStart).normalized;
                float blueArrowMagnitude = Vector3.Distance(blueArrowStart, blueArrowEnd);
                // Calculate the length of the projection point on the blueArrow.
                float projectionLength = Vector3.Dot(startToProjection, blueArrowDirection);
                // Determine if the projected point is within the model range of blueArrow
                bool isProjectionOnSegment = projectionLength >= 0 && projectionLength <= blueArrowMagnitude;
                if (!isProjectionOnSegment)
                {
                    // Draw the dotted line from the projection point to blueArrowStart.
                    EnableDashedLine(closestPointOnLine, blueArrowStart);
                }

                // Do not modify the orientation or position of the redArrow.
                // Hide unnecessary elements
                normalArrow.SetActive(false);
                foreach (var t in parallelograms)
                {
                    t.SetActive(false);
                }

                break;
            case 6: // Parallelepiped
                foreach (var t in parallelograms)
                {
                    t.SetActive(true);
                }

                float volume = Math.Abs(Vector3.Dot(Vector3.Cross(redVector, blueVector), greenVector));
                text = "Volume:\n" + Math.Round(volume, 2);
                break;
            case 7: // Point to plane
                parallelograms[0].SetActive(true);
                for (int i = 1; i < parallelograms.Length; i++)
                {
                    parallelograms[i].SetActive(false);
                }

                Vector3 greenVectorEndPoint = transform.position + transform.forward * 14;
                point.transform.position = greenVectorEndPoint;
                // Calculate the normal vector to the plane and a point on the plane.
                Vector3 planeNormal = Vector3.Cross(redVector, blueVector).normalized;
                Vector3 planePoint = redArrow.transform.position;
                // Calculate the distance from the point to the plane
                float distanceToPlane = Vector3.Dot(planeNormal, point.transform.position - planePoint);
                // Calculate the closest point on the plane to the point
                Vector3 closestPointOnPlane = point.transform.position - distanceToPlane * planeNormal;
                // Update display text to show distance information
                text = "Distance:\n" + Math.Round(Math.Abs(distanceToPlane), 2);
                // Draw the dotted line from the point to the nearest point on the plane
                EnableDashedLine(point.transform.position, closestPointOnPlane);

                // Planar basis vectors and lengths
                Vector3 planeBasisVector1 = redArrow.transform.forward.normalized;
                Vector3 planeBasisVector2 = blueArrow.transform.forward.normalized;
                float length1 = redArrow.transform.localScale.z * 14;
                float length2 = blueArrow.transform.localScale.z * 14;
                // Calculate the vector of the projected point with respect to the plane origin
                Vector3 vectorToProjection = closestPointOnPlane - planePoint;
                // Compute the square length of the basis vectors
                float length1Sqr = length1 * length1;
                float length2Sqr = length2 * length2;
                // Calculation of coefficients
                float coeff1 = Vector3.Dot(vectorToProjection, planeBasisVector1 * length1) / length1Sqr;
                float coeff2 = Vector3.Dot(vectorToProjection, planeBasisVector2 * length2) / length2Sqr;
                // Determine if the projected point is inside a parallelogram
                bool isProjectionInsidePlane = coeff1 is >= 0 and <= 1 && coeff2 is >= 0 and <= 1;
                // Calculate the center of the plane
                Vector3 planeCenter = planePoint + (planeBasisVector1 * (length1 * 0.5f)) +
                                      (planeBasisVector2 * (length2 * 0.5f));
                // If the point of projection is out of the plane, draw an extension line from the point of projection to the center of the plane
                if (!isProjectionInsidePlane)
                {
                    EnableDashedLine(closestPointOnPlane, planeCenter);
                }

                // Hide unnecessary elements
                normalArrow.SetActive(false);
                point.SetActive(true);
                break;
        }

        outputText = text;
        CurrentFunctionData.output = text;
        Vector3 magnitude = transform.localScale;
        magnitude.z = Vector3.Magnitude(greenVector);
        if (test)
        {
            testObject.text = "Difference: " +
                              Math.Round(Vector3.Magnitude(transform.forward.normalized - greenVector.normalized), 2);
        }
        else
        {
            transform.forward = greenVector;
            transform.localScale = magnitude;
        }

        normalArrow.transform.forward = Vector3.Cross(blueVector, redVector);
        normalArrow.transform.localScale = new Vector3(1, 1, 0.1f);
    }

    void Update()
    {
        if (CurrentFunctionData == null)
        {
            // No data to process, exit early
            return;
        }

        CalculateResult();
    }

    public string GetOutput()
    {
        return outputText;
    }

    void EnableDashedLine(Vector3 startPoint, Vector3 endPoint)
    {
        GameObject dashedLineInstance = Instantiate(dashedLinePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lr = dashedLineInstance.GetComponent<LineRenderer>();
        if (lr)
        {
            lr.SetPosition(0, startPoint);
            lr.SetPosition(1, endPoint);
        }

        dashedLineInstance.SetActive(true);
        dashedLineInstances.Add(dashedLineInstance);
    }

    // Added: Disable dashed lines
    void DisableDashedLine()
    {
        foreach (GameObject dashedLineInstance in dashedLineInstances)
        {
            if (dashedLineInstance)
            {
                Destroy(dashedLineInstance);
            }
        }

        dashedLineInstances.Clear();
    }

    void ResetPosition()
    {
        redArrow.transform.localPosition = Vector3.zero;
        blueArrow.transform.localPosition = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }
}