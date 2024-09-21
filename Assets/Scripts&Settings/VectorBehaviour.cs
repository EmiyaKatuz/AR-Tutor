using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VectorBehaviour : MonoBehaviour
{
    [SerializeField] int color = 0;
    [SerializeField] bool isPoint = false;

    KeyCode[][] keys =
    {
        new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.X, KeyCode.C },
        new KeyCode[] { KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L, KeyCode.M, KeyCode.N },
        new KeyCode[]
            { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Comma, KeyCode.Period }
    };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector3 = new Vector3(0, 0, 0);
        if (Input.GetKey(keys[color][0]))
        {
            vector3.y += 1;
        }

        if (Input.GetKey(keys[color][1]))
        {
            vector3.y -= 1;
        }

        if (Input.GetKey(keys[color][2]))
        {
            vector3.x += 1;
        }

        if (Input.GetKey(keys[color][3]))
        {
            vector3.x -= 1;
        }

        if (Input.GetKey(keys[color][4]))
        {
            vector3.z += 1;
        }

        if (Input.GetKey(keys[color][5]))
        {
            vector3.z -= 1;
        }

        if (isPoint)
        {
            transform.position += vector3 * 0.1f;
        }
        else
        {
            transform.Rotate(vector3);
        }
    }
}