using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public Vector3 startPosition;
    public Quaternion startRotation;
    private float speed = 4f;

    // Get start position to return back to when exiting from various modes
    private void Start()
    {
        startPosition = GetComponentInChildren<Camera>().gameObject.transform.position;
        startRotation = GetComponentInChildren<Camera>().gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly spin around centre of terrain
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
