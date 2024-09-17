using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VectorBehaviour : MonoBehaviour
{
    [SerializeField]
    int color = 0;

    KeyCode[][] keys = { 
        new KeyCode[]{ KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D },
        new KeyCode[]{ KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L },
        new KeyCode[]{ KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow } 
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(keys[color][0]))
        {
            transform.Rotate(0, 1, 0);
        }
        if (Input.GetKey(keys[color][1]))
        {
            transform.Rotate(0, -1, 0);
        }
        if (Input.GetKey(keys[color][2]))
        {
            transform.Rotate(1, 0, 0);
        }
        if (Input.GetKey(keys[color][3]))
        {
            transform.Rotate(-1, 0, 0);
        }
    }
}
