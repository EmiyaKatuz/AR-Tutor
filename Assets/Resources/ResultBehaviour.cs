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
    int mode = 0;
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
            case 0:
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = Vector3.zero;
                text = "" + Vector3.Dot(redVector, blueVector);
                break;
            case 1:
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = Vector3.zero;
                greenVector = Vector3.Cross(blueVector, redVector);
                break;
            case 2:
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = redArrow.transform.forward * 14;
                transform.localPosition = Vector3.zero;
                greenVector = redVector + blueVector;
                break;
            case 3:
                redArrow.transform.localPosition = Vector3.zero;
                blueArrow.transform.localPosition = Vector3.zero;
                transform.localPosition = blueArrow.transform.forward * 14;
                greenVector = redVector - blueVector;
                break;
            
        }
        textObject.text = text;
        transform.forward = greenVector;
        Vector3 magnitude = transform.localScale;
        magnitude.z = Vector3.Magnitude(greenVector);
        transform.localScale = magnitude;
    }
}
