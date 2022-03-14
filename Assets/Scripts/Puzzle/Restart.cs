using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    private Vector3 startPos = new Vector3(17.5f, 0, -4.5f); // Default position for player to spawn in relative to each level
    public GameObject currentRoom;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            for (int i = 0; i < currentRoom.transform.childCount; i++)
            {
                if (currentRoom.transform.GetChild(i).GetComponent<PickUpObject>() != null)
                {
                    currentRoom.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true; // Disable then re-enable to reset any previously applied force
                    currentRoom.transform.GetChild(i).position = currentRoom.transform.GetChild(i).GetComponent<PickUpObject>().startPos;
                    currentRoom.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                }
            }

            GetComponent<CharacterController>().enabled = false;
            transform.localPosition = startPos;
            GetComponent<CharacterController>().enabled = true;
        }
    }
}
