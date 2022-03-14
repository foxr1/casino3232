using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RummyTable : MonoBehaviour
{
    float distanceToObject;
    float maximumDistance = 15f; // Closest distance you can be to picking up the object
    private bool mouseOver = false;
    public GameObject guide;
    public string gameName;
    public Canvas canvas;

    private void FixedUpdate()
    {
        distanceToObject = Vector3.Distance(transform.position, guide.transform.position);

        if (mouseOver && distanceToObject <= maximumDistance)
        {
            canvas.GetComponent<Canvas>().enabled = true;

            if (Input.GetKey(KeyCode.E))
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(gameName);
            }
        } 
        else
        {
            canvas.GetComponent<Canvas>().enabled = false;
        }
    }

    void OnMouseEnter()
    {
        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
    }
}
