using UnityEngine;
using TMPro;

public class ResultBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshPro textObject;
    [SerializeField] GameObject redArrow;
    [SerializeField] GameObject blueArrow;
    [SerializeField] GameObject normalArrow;
    [SerializeField] GameObject parallelogram;
    [SerializeField] GameObject dashedLinePrefab; // Added: Dashed Prefab reference
    private GameObject _dashedLineInstance; // Added: Example of a dotted line

    public int mode;

    private void Start()
    {
        if (dashedLinePrefab)
        {
            _dashedLineInstance = Instantiate(dashedLinePrefab, Vector3.zero, Quaternion.identity);
            _dashedLineInstance.SetActive(false); // Initially hide the dotted line
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 redVector = redArrow.transform.forward;
        Vector3 blueVector = blueArrow.transform.forward;
        Vector3 greenVector = transform.forward;
        string text = textObject.text;
        switch (mode)
        {
            case 0: // Dot Product
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = Vector3.zero;
                parallelogram.SetActive(false);
                text = "" + Vector3.Dot(redVector, blueVector);
                DisableDashedLine(); // Added: Disabled the dashed line
                break;
            case 1: // Cross Product + Parallelogram
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = Vector3.zero;
                parallelogram.SetActive(true);
                greenVector = Vector3.Cross(blueVector, redVector);
                text = "" + Vector3.Magnitude(greenVector);
                DisableDashedLine(); // Added: Disabled the dashed line
                break;
            case 2: // Vector Addition
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = redArrow.transform.forward * 14;
                transform.localPosition = Vector3.zero;
                parallelogram.SetActive(false);
                greenVector = redVector + blueVector;
                DisableDashedLine(); // Added: Disabled the dashed line
                break;
            case 3: // Vector Subtraction
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = blueArrow.transform.forward * 14;
                parallelogram.SetActive(false);
                greenVector = redVector - blueVector;
                DisableDashedLine(); // Added: Disabled the dashed line
                break;
            case 4: // Projection
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                greenVector = Vector3.Project(redVector, blueVector);
                parallelogram.SetActive(false);

                // Calculate the endpoint position of redArrow
                Vector3 redArrowEndPoint = redArrow.transform.position +
                                           redArrow.transform.forward * 14;

                // Calculate the endpoint positions of the projection vectors
                Vector3 projectionEndPoint =
                    redArrow.transform.position + greenVector * 14;

                // Enable and update dotted lines
                EnableDashedLine(redArrowEndPoint, projectionEndPoint);
                break;
        }

        textObject.text = text;

        transform.forward = greenVector;
        Vector3 magnitude = transform.localScale;
        magnitude.z = Vector3.Magnitude(greenVector);
        transform.localScale = magnitude;

        normalArrow.transform.forward = Vector3.Cross(blueVector, redVector);
        normalArrow.transform.localScale = new Vector3(1, 1, 0.1f);
    }

    void EnableDashedLine(Vector3 startPoint, Vector3 endPoint)
    {
        if (_dashedLineInstance)
        {
            LineRenderer lr = _dashedLineInstance.GetComponent<LineRenderer>();
            if (lr)
            {
                lr.SetPosition(0, startPoint);
                lr.SetPosition(1, endPoint);
            }

            _dashedLineInstance.SetActive(true);
        }
    }

    // Added: Disable dashed lines
    void DisableDashedLine()
    {
        if (_dashedLineInstance)
        {
            _dashedLineInstance.SetActive(false);
        }
    }
}