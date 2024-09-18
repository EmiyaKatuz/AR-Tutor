using UnityEngine;
using TMPro;

public class ResultBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshPro textObject;
    [SerializeField] TextMeshPro testObject;
    [SerializeField] GameObject redArrow;
    [SerializeField] GameObject blueArrow;
    [SerializeField] GameObject normalArrow;
    [SerializeField] GameObject parallelogram;
    [SerializeField] GameObject dashedLinePrefab; // Added: Dashed Prefab reference
    private GameObject _dashedLineInstance; // Added: Example of a dotted line

    public int mode = 0;
    public bool test = false;

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

        ResetPosition();
        if (mode != 1)
        {
            parallelogram.SetActive(false);
        }
        DisableDashedLine();
        switch (mode)
        {
            case 0: // Dot Product
                text = "" + Vector3.Dot(redVector, blueVector);
                break;
            case 1: // Cross Product + Parallelogram
                parallelogram.SetActive(true);
                greenVector = Vector3.Cross(blueVector, redVector);
                text = "" + Vector3.Magnitude(greenVector);
                break;
            case 2: // Vector Addition
                blueArrow.transform.localPosition = redArrow.transform.forward * 14;
                greenVector = redVector + blueVector;
                break;
            case 3: // Vector Subtraction
                transform.localPosition = blueArrow.transform.forward * 14;
                greenVector = redVector - blueVector;
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
                break;
            case 5: // Point to line
                break;
            case 6: // Parallelepiped
                break;
            case 7: // Point to plane
                break;
        }

        textObject.text = text;
        Vector3 magnitude = transform.localScale;
        magnitude.z = Vector3.Magnitude(greenVector);

        if (test)
        {
            float difference = (int) (100 * Vector3.Magnitude(transform.forward.normalized - greenVector.normalized)) / 100.0f;
            testObject.text = "Difference: " + difference;
        } else { 
            transform.forward = greenVector;
            transform.localScale = magnitude;
        }
        

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

    void ResetPosition()
    {
        redArrow.transform.localPosition = Vector3.zero;
        blueArrow.transform.localPosition = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }
}