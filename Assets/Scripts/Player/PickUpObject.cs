using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    // Adapted code from https://www.youtube.com/watch?v=IEV64CLZra8 tutorial

    // Picking up object variables
    float throwForce = 1500;
    Vector3 objectPosition;
    float distanceToObject;
    float maximumDistance = 15f; // Closest distance you can be to picking up the object
    public bool canHold = true;
    public GameObject item, tempParent;
    public bool isHolding = false;
    public bool uniform = true;

    // Changing scale variables
    Vector3 minScale = new Vector3(1f, 1f, 1f); // Restriction so size of object cannot be negative.

    // Colours
    public Color startColour;
    public Color mouseOverColour;
    public Color currentColor;
    bool mouseOver = false;
    float delay = 0.0f;

    public Vector3 startPos;
    public Transform parent;

    private void Start()
    {
        startPos = transform.position;
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update() 
    {
        distanceToObject = Vector3.Distance(item.transform.position, tempParent.transform.position);

        if (distanceToObject >= maximumDistance)
        {
            isHolding = false;
        }

        // Increase and decrease size of object on scroll wheel.
        if (Input.GetMouseButton(0) && isHolding)
        {
            float zoomValue = Input.GetAxis("Mouse ScrollWheel");

            if (zoomValue != 0)
            {
                if (uniform)
                {
                    item.transform.localScale += Vector3.one * zoomValue * 7;
                    item.transform.localScale = Vector3.Max(transform.localScale, minScale); // Implemented .Max function to avoid negative mass
                } 
                else
                {
                    item.transform.localScale += new Vector3(zoomValue * 7, 0, zoomValue * 7);
                    item.transform.localScale = Vector3.Max(transform.localScale, minScale); // Implemented .Max function to avoid negative mass
                }

                if (item.transform.localScale == new Vector3(1, 1, 1) || item.GetComponent<Rigidbody>().mass < 1) // Lock scale to be no less than 1x1x1 to avoid errors with small mass
                {
                    item.GetComponent<Rigidbody>().mass = 1;
                }
                else
                {
                    item.GetComponent<Rigidbody>().mass += zoomValue * 5;
                }
            }
        }

        if (isHolding)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            // Throw on right click.
            if (Input.GetMouseButtonDown(1))
            {
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }
        }
        else
        {
            objectPosition = item.transform.position;
            item.transform.SetParent(parent);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPosition;
        }

        if (mouseOver && !isHolding && distanceToObject <= maximumDistance)
        {
            delay += Time.deltaTime;
            currentColor = item.GetComponent<Renderer>().material.color;
            item.GetComponent<Renderer>().material.color = Color.Lerp(startColour, mouseOverColour, Mathf.PingPong(delay, 1));
        }
        else
        {
            item.GetComponent<Renderer>().material.SetColor("_Color", startColour);
        }
    }

    void OnMouseDown()
    {
        delay = 0f;
        if (distanceToObject <= maximumDistance)
        {
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
        }
    }

    void OnMouseUp()
    {
        isHolding = false;
    }

    void OnMouseEnter()
    {
        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
        delay = 0.0f;
    }

    public bool GetIsHolding()
    {
        return isHolding;
    }
}
