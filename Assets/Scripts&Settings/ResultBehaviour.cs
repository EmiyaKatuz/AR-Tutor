using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

public enum Mode
{
    ADD,
    MINUS,
    DOT,
    PROJECT,
    PTL,
    PTP,
    CROSS,
    TRIPLE
}

public class ResultBehaviour : MonoBehaviour
{
    [SerializeField] GameObject redArrow;
    [SerializeField] GameObject blueArrow;
    [SerializeField] GameObject greenArrow;

    [SerializeField] bool activeAR = false;
    [SerializeField] GameObject redArrowActual;
    [SerializeField] GameObject blueArrowActual;
    [SerializeField] GameObject greenArrowActual;

    [SerializeField] GameObject normalArrow;
    [SerializeField] GameObject point;
    [SerializeField] GameObject[] parallelograms = new GameObject[6];
    [SerializeField] GameObject dashedLinePrefab; // Added: Dashed Prefab reference
    [SerializeField] UnityEngine.UI.Text topText;
    [SerializeField] UnityEngine.UI.Text bottomText;
    [SerializeField] private GameObject arcVisualizerPrefab;
    [SerializeField] Mode mode;
    [SerializeField] int step;
    public bool test = false;
    private GameObject _arcInstance;
    private List<GameObject> dashedLineInstances = new List<GameObject>();

    private string outputText;
    public FunctionData CurrentFunctionData { get; private set; }

    private void Start() {
        if (activeAR) {
            redArrow = redArrowActual;
            blueArrow = blueArrowActual;
            greenArrow = greenArrowActual;
        }
    }

    public void UpdateResult(FunctionData data)
    {
        CurrentFunctionData = data;
        // Update mode
        mode = CurrentFunctionData.mode;
        // Reset positions and initializations
        ResetPosition();
        DisableDashedLine();
        // Disable or reset elements as needed
        if (CurrentFunctionData.mode != Mode.PTP && CurrentFunctionData.mode != Mode.CROSS &&
            CurrentFunctionData.mode != Mode.TRIPLE)
        {
            foreach (var t in parallelograms)
            {
                t.SetActive(false);
            }
        }

        // Destroy existing arcVisualizer instance
        if (_arcInstance != null)
        {
            Destroy(_arcInstance);
            _arcInstance = null;
        }

        // Perform initial calculation
        CalculateResult();
    }

    // Update is called once per frame
    private void CalculateResult()
    {
        Vector3 redVector = redArrow.transform.forward;
        Vector3 blueVector = blueArrow.transform.forward;
        Vector3 greenVector = greenArrow.transform.forward;

        float magnitudes = Vector3.Magnitude(redVector) * Vector3.Magnitude(blueVector);
        float dot = Vector3.Dot(redVector, blueVector);
        float angle = (float)Math.Acos(dot / magnitudes);
        Vector3 cross = Vector3.Cross(blueVector, redVector);

        // Disable everything to select specific things to show
        DisableDashedLine();
        foreach (var t in parallelograms)
        {
            t.SetActive(false);
        }

        point.SetActive(false);
        greenArrow.SetActive(false);
        normalArrow.SetActive(false);
        ResetPosition();
        normalArrow.transform.forward = Vector3.Cross(blueVector, redVector);
        normalArrow.transform.localScale = new Vector3(1, 1, 0.1f);

        switch (mode)
        {
            case Mode.ADD:
                blueArrow.transform.localPosition = redVector * 14;
                greenVector = redVector + blueVector;
                greenArrow.SetActive(true);
                break;

            case Mode.MINUS:
                greenArrow.transform.localPosition = blueVector * 14;
                greenVector = redVector - blueVector;
                greenArrow.SetActive(true);
                break;

            case Mode.DOT:
                VisualizeAngle(redVector, blueVector, redArrow.transform.position);
                switch (step)
                {
                    case 0:
                        topText.text = "Angle: " + Math.Round(angle, 2);
                        break;
                    case 1:
                        topText.text = "Result: " + Math.Round(dot, 2);
                        bottomText.text = "Dot products are Scalars.";
                        break;
                }

                break;

            case Mode.PROJECT:
                greenVector = Vector3.Project(redVector, blueVector);
                Vector3 redArrowEndPoint = redArrow.transform.position + redVector * 14;
                Vector3 projectionEndPoint = redArrow.transform.position + greenVector * 14;
                normalArrow.transform.forward = greenVector;
                VisualizeAngle(redVector, blueVector, redArrow.transform.position);
                switch (step)
                {
                    case 0:
                        topText.text = "Angle: " + Math.Round(angle, 2);
                        bottomText.text = "|Green|=|Blue|cos" + Math.Round(angle, 2);
                        break;
                    case 1:
                        topText.text = "Dot: " + Math.Round(dot, 2);
                        greenArrow.SetActive(true);
                        EnableDashedLine(redArrowEndPoint, projectionEndPoint);
                        bottomText.text = "|Green|=(Red.Blue)/|Red|";
                        break;
                    case 2:
                        topText.text = "Red must be normalised.";
                        greenArrow.SetActive(true);
                        normalArrow.SetActive(true);
                        bottomText.text = "Green=|Green|*Red.Normalise";
                        break;
                }

                break;

            case Mode.PTL:
                point.SetActive(true);
                // Getting the start and direction of a line
                Vector3 lineOrigin = blueArrow.transform.position;
                Vector3 lineDirection = blueVector.normalized;
                // Calculate the endpoint position of redArrow
                Vector3 redArrowPoint = redArrow.transform.position + redVector * 14;
                point.transform.position = redArrowPoint;
                // Get the position of point
                Vector3 originToPoint = point.transform.position - lineOrigin;
                Vector3 projection = Vector3.Project(originToPoint, lineDirection);
                Vector3 closestPointOnLine = lineOrigin + projection;
                // Calculate the shortest distance from a point to a line
                float distance = Vector3.Distance(point.transform.position, closestPointOnLine);
                // Update display text to show distance information
                topText.text = "Distance: " + Math.Round(distance, 2);
                EnableDashedLine(point.transform.position, closestPointOnLine);
                // Get the length of blueArrow
                float blueArrowLength = blueArrow.transform.localScale.z * 14;
                // Calculate the start and end points of blueArrow
                Vector3 blueArrowStart = blueArrow.transform.position;
                Vector3 blueArrowEnd = blueArrowStart + blueVector * blueArrowLength;
                // Calculate the vector from the projected point to blueArrowStart.
                Vector3 startToProjection = closestPointOnLine - blueArrowStart;
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

            case Mode.PTP:
                parallelograms[0].SetActive(true);
                for (int i = 1; i < parallelograms.Length; i++)
                {
                    parallelograms[i].SetActive(false);
                }

                point.SetActive(true);
                // Calculate the normal vector to the plane and a point on the plane.
                Vector3 planeNormal = Vector3.Cross(redVector, blueVector).normalized;
                Vector3 planePoint = redArrow.transform.position;
                Vector3 greenVectorEndPoint = greenArrow.transform.position + greenVector * 14;
                point.transform.position = greenVectorEndPoint;
                // Calculate the distance from the point to the plane
                float distanceToPlane = Vector3.Dot(planeNormal, point.transform.position - planePoint);
                // Update display text to show distance information
                topText.text = "Distance: " + Math.Abs(distanceToPlane).ToString("F2");
                // Draw the dotted line from the point to the nearest point on the plane
                Vector3 closestPointOnPlane = point.transform.position - distanceToPlane * planeNormal;
                EnableDashedLine(point.transform.position, closestPointOnPlane);
                // Planar basis vectors and lengths
                Vector3 planeBasisVector1 = redVector.normalized;
                Vector3 planeBasisVector2 = blueVector.normalized;
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

            case Mode.CROSS:
                greenVector = cross;
                // Update greenArrow
                greenArrow.transform.forward = greenVector.normalized;
                greenArrow.transform.localScale = new Vector3(1, 1, greenVector.magnitude * 14);
                // Activate the parallelogram needed
                parallelograms[0].SetActive(true);
                VisualizeAngle(redVector, blueVector, redArrow.transform.position);
                switch (step)
                {
                    case 0:
                        topText.text = "Angle: " + Math.Round(angle, 2);

                        break;
                    case 1:
                        normalArrow.SetActive(true);
                        bottomText.text = "Cross products are Vectors.";
                        break;
                    case 2:
                        normalArrow.SetActive(true);
                        greenArrow.SetActive(true);
                        break;
                    case 3:
                        normalArrow.SetActive(true);
                        greenArrow.SetActive(true);
                        parallelograms[0].SetActive(true);
                        bottomText.text = "Area = |Red x Blue|";
                        break;
                }

                break;

            case Mode.TRIPLE:

                greenArrow.SetActive(true);
                VisualizeAngle(redVector, blueVector, redArrow.transform.position);

                switch (step)
                {
                    case 0:
                        topText.text = "Angle: " + Math.Round(angle, 2);

                        break;
                    case 1:
                        normalArrow.SetActive(true);
                        break;
                    case 2:
                        normalArrow.SetActive(true);
                        parallelograms[0].SetActive(true);
                        break;
                    case 3:
                        float volume = Math.Abs(Vector3.Dot(cross, greenVector));
                        topText.text = "Volume:\n" + Math.Round(volume, 2);

                        normalArrow.SetActive(true);
                        foreach (var t in parallelograms)
                        {
                            t.SetActive(true);
                        }

                        break;
                }

                break;
        }

        Vector3 magnitude = greenArrow.transform.localScale;
        magnitude.z = Vector3.Magnitude(greenVector);
        if (test)
        {
            //testObject.text = "Difference: " +
            Math.Round(Vector3.Magnitude(greenArrow.transform.forward.normalized - greenVector.normalized), 2);
        }
        else
        {
            greenArrow.transform.forward = greenVector;
            greenArrow.transform.localScale = magnitude;
        }
    }

    void Update()
    {
        if (CurrentFunctionData == null)
        {
            // No data to process, exit early
            //return;
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
        greenArrow.transform.localPosition = Vector3.zero;
    }

    private void VisualizeAngle(Vector3 vector1, Vector3 vector2, Vector3 startPosition)
    {
        if (arcVisualizerPrefab)
        {
            if (_arcInstance)
            {
                Destroy(_arcInstance);
            }

            _arcInstance = Instantiate(arcVisualizerPrefab, startPosition, Quaternion.identity);
            DynamicArcVisualizer arcVisualizer = _arcInstance.GetComponent<DynamicArcVisualizer>();
            if (arcVisualizer)
            {
                arcVisualizer.SetRadius(2.0f);
                float angle = Vector3.Angle(vector1, vector2);
                arcVisualizer.UpdateArc(vector1, vector2, startPosition, angle);
            }
        }
    }
}