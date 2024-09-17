using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultBehaviour : MonoBehaviour
{
    [SerializeField]
    TextMeshPro textObject = null;
    [SerializeField]
    GameObject redArrow = null;
    [SerializeField]
    GameObject blueArrow = null;
    [SerializeField]
    GameObject normalArrow = null;
    [SerializeField]
    GameObject parallelogram = null;
    public int mode = 0;
    // Start is called before the first frame update
    void Start()
    {
        
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
                break;
            case 1: // Cross Product + Parallelogram
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = Vector3.zero;
                parallelogram.SetActive(true);
                greenVector = Vector3.Cross(blueVector, redVector);
                text = "" + Vector3.Magnitude(greenVector);
                break;
            case 2: // Vector Addition
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = redArrow.transform.forward * 14;
                transform.localPosition = Vector3.zero;
                parallelogram.SetActive(false);
                greenVector = redVector + blueVector;
                break;
            case 3: // Vector Subtraction
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = blueArrow.transform.forward * 14;
                parallelogram.SetActive(false);
                greenVector = redVector - blueVector;
                break;
            case 4: // Projection
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                greenVector = Vector3.Project(redVector, blueVector);
                parallelogram.SetActive(false);
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
}
