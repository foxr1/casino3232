using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    private float speed = 2f;
    private Vector3 startPos, endPos, targetPos;

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x - 40, transform.position.y, transform.position.z);
        targetPos = endPos;
    }

    void Update()
    {
        if (V3Equal(transform.position, endPos))
        {
            targetPos = startPos;
        }
        else if (V3Equal(transform.position, startPos))
        {
            targetPos = endPos;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }

    /* Check if two vectors are approximately equal,
    adapted from https://answers.unity.com/questions/395513/vector3-comparison-efficiency-and-float-precision.html */
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.01;
    }
}
