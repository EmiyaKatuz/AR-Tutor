using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.CodeDom;
using UnityEngine.UIElements;

public enum Mode {
    ADD,
    MINUS,
    DOT,
    PROJECT,
    PTL,
    PTP,
    CROSS,
    TRIPLE
}

[ExecuteInEditMode]
public class ResultBehaviour : MonoBehaviour {
    [SerializeField] GameObject redArrow;
    [SerializeField] GameObject blueArrow;
    [SerializeField] GameObject greenArrow;
    [SerializeField] Material greenColor;
    [SerializeField] Material yellowColor;
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
    [SerializeField] private UnityEngine.UI.Button leftButton;
    [SerializeField] private UnityEngine.UI.Button rightButton;
    [SerializeField] private PlaneManager planeManager;
    [SerializeField] private Mode modeTest;
    [SerializeField] private GameObject arc;
    private float redLength = 0.7f;
    private float blueLength = 1.2f;
    private int greenLength = 14;
    private int LENGTH = 14;


    [System.Serializable]
    public struct VectorPair {
        public GameObject vector1Object;
        public GameObject vector2Object;
    }

    [SerializeField] private List<VectorPair> vectorPairs = new List<VectorPair>();
    private GameObject _arcInstance;
    private List<GameObject> dashedLineInstances = new List<GameObject>();
    private Mode mode;
    private string outputText;
    private int currentStep;
    public FunctionData CurrentFunctionData { get; private set; }
    public bool test = false;

    private void Start() {
        if (leftButton != null)
            leftButton.onClick.AddListener(OnLeftButtonClick);
        if (rightButton != null)
            rightButton.onClick.AddListener(OnRightButtonClick);
        if (activeAR) {
            // redArrow.
            redArrow = redArrowActual;
            blueArrow = blueArrowActual;
            greenArrow = greenArrowActual;
        }
    }

    public void UpdateResult(FunctionData data) {
        CurrentFunctionData = data;
        // Update mode
        mode = CurrentFunctionData.mode;
        currentStep = 0;
        UpdateButtonsVisibility();
        // Reset positions and initializations
        ResetPosition();
        DisableDashedLine();
        // Disable or reset elements as needed
        if (CurrentFunctionData.mode != Mode.PTP && CurrentFunctionData.mode != Mode.CROSS &&
            CurrentFunctionData.mode != Mode.TRIPLE) {
            foreach (var t in parallelograms) {
                t.SetActive(false);
            }
        }

        // Destroy existing arcVisualizer instance
        if (_arcInstance != null) {
            Destroy(_arcInstance);
            _arcInstance = null;
        }

        // Perform initial calculation
        CalculateResult();
    }

    // Update is called once per frame
    private void CalculateResult() {
        Vector3 redVector = redArrow.transform.forward;
        Vector3 blueVector = blueArrow.transform.forward;
        Vector3 greenVector = greenArrow.transform.forward;

        if (test & modeTest != null) {
            mode = modeTest;
            // Debug.Log(mode);
        }

        float magnitudes = Vector3.Magnitude(redVector) * Vector3.Magnitude(blueVector);
        float dot = Vector3.Dot(redVector, blueVector);
        float angle = (float)(Math.Acos(dot / magnitudes) * 180 / Math.PI);
        Vector3 cross = Vector3.Cross(blueVector, redVector);

        topText.text = "";
        bottomText.text = "";

        // Disable everything to select specific things to show
        DisableDashedLine();
        foreach (var t in parallelograms) {
            t.SetActive(false);
        }

        /*
        if (planeManager != null)
        {
            planeManager.ClearPlanes();
        }
        */
        point.SetActive(false);
        greenArrow.SetActive(false);
        redArrow.SetActive(true);
        normalArrow.SetActive(false);
        ResetPosition(); // PUTS ALL VECTORS AT THE ORIGIN

        arc.SetActive(false);

        if (currentStep == -1) {
            if (mode == Mode.ADD) {
                bottomText.text = "Please place TWO vectors with one base to one end.";
            }
            else if (mode == Mode.TRIPLE) {
                bottomText.text = "Please place THREE vectors with bases next to each other.";
            }
            else {
                bottomText.text = "Please place TWO vectors with bases next to each other.";
            }
        }

        else {
            switch (mode) {
                case Mode.ADD:
                    //planeManager.ClearPlanes();
                    greenVector = redVector * redLength + blueVector * blueLength;

                    greenArrow.transform.localPosition = redArrow.transform.parent.localPosition;
                    // greenArrow.transform.localScale = new Vector3(1,1,1);
                    greenArrow.SetActive(true);
                    break;

                case Mode.MINUS:
                    greenArrow.transform.localPosition = blueArrow.transform.parent.localPosition + blueLength * blueVector *7f;
                    // Debug.Log(blueLength * blueVector);
                    greenVector = redVector * redLength - blueVector * blueLength;
                    greenArrow.SetActive(true);
                    break;

                case Mode.DOT:
                    VisualizeArc();
                    topText.text = "Angle: " + Math.Round(angle, 2) + "Degrees\nDot: " + Math.Round(dot, 2);
                    switch (currentStep)
                    {
                        case 0:
                            bottomText.text = "";
                            break;
                        case 1:
                            bottomText.text = "Dot products are Scalars.";
                            break;
                        case 2:
                            bottomText.text =
                                "Move the vectors to make the smallest dot product. What angle creates this?";
                            break;
                        case 3:
                            bottomText.text =
                                "Move the vectors to make the largest dot product. What angle creates this?";
                            break;
                        case 4:
                            bottomText.text = "What trig function acts like this? sin, cos or tan?";
                            break;
                        case 5:
                            bottomText.text = "Red.Blue=|Red||Blue|cos(Angle)";
                            break;
                    }
                    break;

                case Mode.PROJECT:
                    Vector3 proj = Vector3.Project(redVector, blueVector);
                    proj.z *= redLength;
                    greenVector = proj;
                    greenArrow.transform.localPosition = blueArrow.transform.position;
                    Vector3 redArrowEndPoint = redArrow.transform.parent.position + redVector * redLength * 13f;
                    Vector3 projectionEndPoint = greenArrow.transform.localPosition + proj * redLength * 18f;
                    //Debug.Log(redArrowEndPoint);
                    //Vector3 projectionEndPoint = redArrow.transform.parent.position + greenVector * redLength;
                    normalArrow.transform.forward = greenVector;
                    VisualizeArc();
                    // Debug.Log(redVector);
                    // Debug.Log(blueVector);
                    // Debug.Log(greenVector);
                    topText.text = "Angle: " + Math.Round(angle, 2) + "Degrees\nDot: " + Math.Round(dot, 2);
                    switch(currentStep)
                    {
                        case 0:
                            bottomText.text = "|Green| = |Blue| cos(Angle)";
                            break;
                        case 1:
                            greenArrow.SetActive(true);
                            EnableDashedLine(redArrowEndPoint, projectionEndPoint);
                            bottomText.text = "|Green| = (Red · Blue) / |Red|";
                            break;
                        case 2:
                            topText.text = "Red must be normalised.";
                            greenArrow.SetActive(true);
                            normalArrow.SetActive(true);
                            bottomText.text = "Green = |Green| * Red.Normalise";
                            break;
                        case 3:
                            greenArrow.SetActive(true);
                            normalArrow.SetActive(true);
                            bottomText.text = "Move the vectors to make the shortest projection.";
                            break;
                        case 4:
                            greenArrow.SetActive(true);
                            normalArrow.SetActive(true);
                            bottomText.text = "Move the vectors to make the longest projection.";
                            break;
                        case 5:
                            greenArrow.SetActive(true);
                            normalArrow.SetActive(true);
                            bottomText.text = "The dot product calculates how \"aligned\" one vector is to another.";
                            break;
                    }
                    break;
                case Mode.PTL:
                    point.SetActive(true);
                    // Getting the start and direction of a line
                    Vector3 lineOrigin = blueArrow.transform.position;
                    Vector3 lineDirection = blueVector.normalized;
                    // Calculate the endpoint position of redArrow
                    Vector3 redArrowPoint = redArrow.transform.position + redVector * redLength;
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
                    // float blueArrowLength = blueArrow.transform.localScale.z * blueLength;
                    // Calculate the start and end points of blueArrow
                    Vector3 blueArrowStart = blueArrow.transform.position;
                    Vector3 blueArrowEnd = blueArrowStart + blueVector * blueLength;
                    // Calculate the vector from the projected point to blueArrowStart.
                    Vector3 startToProjection = closestPointOnLine - blueArrowStart;
                    Vector3 blueArrowDirection = (blueArrowEnd - blueArrowStart).normalized;
                    float blueArrowMagnitude = Vector3.Distance(blueArrowStart, blueArrowEnd);
                    // Calculate the length of the projection point on the blueArrow.
                    float projectionLength = Vector3.Dot(startToProjection, blueArrowDirection);
                    // Determine if the projected point is within the model range of blueArrow
                    bool isProjectionOnSegment = projectionLength >= 0 && projectionLength <= blueArrowMagnitude;
                    if (!isProjectionOnSegment) {
                        // Draw the dotted line from the projection point to blueArrowStart.
                        EnableDashedLine(closestPointOnLine, blueArrowStart);
                    }

                    switch (currentStep)
                    {
                        case 0:
                            bottomText.text = "Vector drawn from origin to point.";
                            break;
                        case 1:
                            bottomText.text = "Vector projected along line.";
                            break;
                        case 2:
                            bottomText.text = "One vector subtracted from the other.";
                            break;
                        case 3:
                            bottomText.text = "Magnitude of vector taken.";
                            break;
                    }

                    // Do not modify the orientation or position of the redArrow.
                    // Hide unnecessary elements
                    redArrow.SetActive(false);
                    normalArrow.SetActive(false);
                    foreach (var t in parallelograms) {
                        t.SetActive(false);
                    }

                    break;


                case Mode.PTP:

                    switch (currentStep)
                    {
                        case 0:
                            bottomText.text = "Plane normal vector drawn";
                            break;
                        case 1:
                            bottomText.text = "Vector drawn from plane point to other point";
                            break;
                        case 2:
                            bottomText.text = "Dot of normal and this vector taken.";
                            break;
                        case 3:
                            bottomText.text = "Absolute value of dot taken.";
                            break;
                    }
                    parallelograms[0].SetActive(true);
                    for (int i = 1; i < parallelograms.Length; i++) {
                        parallelograms[i].SetActive(false);
                    }

                    /*
                    vectorPairs.Clear();
                    // Here you define the vector pair that needs to generate the plane
                    vectorPairs.Add(new VectorPair { vector1Object = redVector, vector2Object = blueVector });
                    // Create a plane
                    if (planeManager != null)
                    {
                        foreach (var pair in vectorPairs)
                        {
                            planeManager.CreatePlane(pair.vector1Object, pair.vector2Object);
                        }
                    }
                    */
                    point.SetActive(true);
                    // Calculate the normal vector to the plane and a point on the plane.
                    Vector3 planeNormal = Vector3.Cross(redVector, blueVector).normalized;
                    Vector3 planePoint = redArrow.transform.position;
                    Vector3 greenVectorEndPoint = greenArrow.transform.position + greenVector * LENGTH;
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
                    float length1 = redArrow.transform.localScale.z * redLength;
                    float length2 = blueArrow.transform.localScale.z * blueLength;
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
                    if (!isProjectionInsidePlane) {
                        EnableDashedLine(closestPointOnPlane, planeCenter);
                    }

                    break;
                case Mode.CROSS:
                    greenVector = cross;
                    // Update greenArrow
                    greenArrow.transform.forward = greenVector.normalized;
                    greenArrow.transform.localPosition = (redArrow.transform.position + blueArrow.transform.position) / 2f;
                    greenArrow.transform.localScale = new Vector3(1, 1, greenVector.magnitude * LENGTH);
                    VisualizeArc();
                    normalArrow.transform.position = (redArrow.transform.position + blueArrow.transform.position) / 2f;
                    topText.text = "Angle: " + Math.Round(angle, 2) + "Degrees\nCross magnitude:" + cross.magnitude;
                    switch (currentStep) { 

                        case 0:
                            bottomText.text = "";
                            break;
                        case 1:
                            normalArrow.SetActive(true);
                            bottomText.text = "Cross products are Vectors.";
                            break;
                        case 2:
                            normalArrow.SetActive(true);
                            greenArrow.SetActive(true);
                            bottomText.text = "Cross product is a vector parallel to the normal.";
                            break;
                        case 3:
                            normalArrow.SetActive(true);
                            greenArrow.SetActive(true);
                            parallelograms[0].SetActive(true);
                            /*
                            vectorPairs.Clear();
                            // Here you define the vector pair that needs to generate the plane
                            vectorPairs.Add(new VectorPair { vector1Object = redArrow, vector2Object = blueArrow });
                            // Create a plane
                            if (planeManager != null)
                            {
                                foreach (var pair in vectorPairs)
                                {
                                    planeManager.CreatePlane(pair.vector1Object, pair.vector2Object);
                                }
                            }
                            */
                            bottomText.text = "Area = |Red x Blue|";
                            break;

                        case 4:
                            greenArrow.SetActive(true);
                            bottomText.text =
                                "Move the vectors to make the smallest cross product. What angle creates this?";
                            break;
                        case 5:
                            greenArrow.SetActive(true);
                            bottomText.text =
                                "Move the vectors to make the largest cross product. What shape is formed?";
                            break;
                        case 6:
                            greenArrow.SetActive(true);
                            bottomText.text = "What trig function acts like this? sin, cos or tan?";
                            break;
                        case 7:
                            greenArrow.SetActive(true);
                            bottomText.text = "Swap the vectors around. What happens to the cross product?";
                            break;
                        case 8:
                            greenArrow.SetActive(true);
                            bottomText.text = "RedxBlue = |Red| |Blue| sin(Angle) * Normal";
                            break;
                    }

                    break;

                case Mode.TRIPLE:
                    VisualizeArc();
                    greenArrow.SetActive(true);
                    topText.text = "Angle: " + Math.Round(angle, 2) + "Degrees\nCross magnitude:" + cross.magnitude;
                    switch (currentStep)
                    {
                        case 0:
                            bottomText.text = "";
                            break;
                        case 1:
                            bottomText.text = "The normal is orthogonal to the red and blue vectors.";
                            break;
                        case 2: // same as case 1 except it's not normalised
                            bottomText.text = "The purple arrow is now the cross product.";
                            normalArrow.SetActive(true);
                            break;
                        case 3:
                            bottomText.text = "Area of a parallelogram is the magnitude of the cross product.";
                            normalArrow.SetActive(true);
                            parallelograms[0].SetActive(true);
                            /*
                            vectorPairs.Clear();
                            // Here you define the vector pair that needs to generate the plane
                            vectorPairs.Add(new VectorPair { vector1Object = redVector, vector2Object = blueVector });
                            // Create a plane
                            if (planeManager != null)
                            {
                                foreach (var pair in vectorPairs)
                                {
                                    planeManager.CreatePlane(pair.vector1Object, pair.vector2Object);
                                }
                            }
                            */
                            break;
                        case 4:
                            bottomText.text = "Volume = (Red x Blue).Yellow";
                            normalArrow.SetActive(true);
                            for (int i = 0; i < parallelograms.Length; i++) {
                                parallelograms[i].SetActive(true);
                            }

                            float volume = Math.Abs(Vector3.Dot(cross, greenVector));
                            topText.text = "Volume:\n" + Math.Round(volume, 2);
                            break;
                        case 5:
                            normalArrow.SetActive(true);
                            for (int i = 0; i < parallelograms.Length; i++) {
                                parallelograms[i].SetActive(true);
                            }

                            bottomText.text = "Move the vectors to make the smallest volume. What shape is formed?";
                            break;
                        case 6:
                            normalArrow.SetActive(true);
                            for (int i = 0; i < parallelograms.Length; i++) {
                                parallelograms[i].SetActive(true);
                            }

                            bottomText.text =
                                "Align the green vector with the magenta normal vector. What shape is formed?";
                            break;
                        case 7:
                            normalArrow.SetActive(true);
                            for (int i = 0; i < parallelograms.Length; i++) {
                                parallelograms[i].SetActive(true);
                            }
                            bottomText.text = "Volume of a paralellepiped is a.(bxc)";
                            break;
                    }

                    break;
            }
        }

        normalArrow.transform.forward = cross;
        
        if (!(mode == Mode.TRIPLE && currentStep == 2)) // normalise normal arrow unless it is being the cross product
        {
            normalArrow.transform.localScale = new Vector3(1, 1, 0.1f);
        } else
        {
            normalArrow.transform.localScale = new Vector3(1, 1, cross.magnitude);
        }

        Vector3 magnitude = greenArrow.transform.localScale;
        magnitude.z = Vector3.Magnitude(greenVector);

        if (mode == Mode.TRIPLE) //if the green arrow is bound to a tangible object, make it yellow.
        {
            for (int i = 0; i < greenArrow.transform.childCount; i++) {
                greenArrow.transform.GetChild(i).GetComponent<MeshRenderer>().material = yellowColor;
            }
        }
        else {
            for (int i = 0; i < greenArrow.transform.childCount; i++) {
                greenArrow.transform.GetChild(i).GetComponent<MeshRenderer>().material = greenColor;
            }

            greenArrow.transform.forward = greenVector;
            greenArrow.transform.localScale = magnitude;
        }
    }

    void Update() {
        if (CurrentFunctionData == null) {
            return;
        }

        CalculateResult();
    }

    public string GetOutput() {
        return outputText;
    }

    void EnableDashedLine(Vector3 startPoint, Vector3 endPoint) {
        GameObject dashedLineInstance = Instantiate(dashedLinePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lr = dashedLineInstance.GetComponent<LineRenderer>();
        if (lr) {
            lr.SetPosition(0, startPoint);
            lr.SetPosition(1, endPoint);
        }

        dashedLineInstance.SetActive(true);
        dashedLineInstances.Add(dashedLineInstance);
    }

    // Added: Disable dashed lines
    void DisableDashedLine() {
        foreach (GameObject dashedLineInstance in dashedLineInstances) {
            if (dashedLineInstance) {
                DestroyImmediate(dashedLineInstance);
            }
        }

        dashedLineInstances.Clear();
    }

    void ResetPosition() {
        // Vector3 vector = new Vector3(3, 12, 6);
        // redArrow.transform.localPosition = vector;
        // blueArrow.transform.localPosition = vector;
        // greenArrow.transform.localPosition = vector;
        // normalArrow.transform.localPosition = vector;
    }

    private void VisualizeArc() {
        arc.SetActive(true);
    }

    private void OnLeftButtonClick() {
        if (currentStep > 0) {
            currentStep--;
            UpdateButtonsVisibility();
            CalculateResult();
        }
    }

    private void OnRightButtonClick() {

        if (currentStep < CurrentFunctionData.step) {
            currentStep++;
            UpdateButtonsVisibility();
            CalculateResult();
        }
    }

    private void UpdateButtonsVisibility() {
          if (leftButton)
              leftButton.gameObject.SetActive(currentStep > 0);
          if (rightButton)
              rightButton.gameObject.SetActive(currentStep < CurrentFunctionData.step);
    }
}