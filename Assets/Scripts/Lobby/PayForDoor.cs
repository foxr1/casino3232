using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayForDoor : MonoBehaviour
{
    float distanceToObject;
    float maximumDistance = 15f; // Closest distance you can be to picking up the object
    private bool mouseOver = false, doorOpened = false;
    public GameObject payCanvas;

    private GameObject player;
    private GameObject guide;
    private int noOfCoins = 0;

    public GameObject triggerVolume;
    public string door;
    private int doorNo;

    private void Start()
    {
        player = GameObject.Find("Player");
        if (player != null) guide = player.transform.GetChild(0).GetChild(0).gameObject;
        if (door.Equals("Rummy"))
        {
            doorNo = 0;
        } 
        else if (door.Equals("Puzzle"))
        {
            doorNo = 1;
        }

        doorOpened = PlayerController.openedDoors[doorNo];
    }

    private void FixedUpdate()
    {
        if (guide != null)
        {
            distanceToObject = Vector3.Distance(transform.position, guide.transform.position);

            if (mouseOver && distanceToObject <= maximumDistance && !doorOpened)
            {
                payCanvas.SetActive(true);

                if (Input.GetKey(KeyCode.E))
                {
                    if (door.Equals("Rummy"))
                    {
                        noOfCoins = player.GetComponent<PlayerController>().GetCoinCount();
                        if (noOfCoins >= 100)
                        {
                            noOfCoins -= 100;
                            player.GetComponent<PlayerController>().SetCoinCount(noOfCoins);
                            OpenDoor(0);
                        }
                        else
                        {
                            StartCoroutine(player.GetComponent<PlayerController>().ShowMessage("You do not have enough money."));
                        }
                    }
                    else if (door.Equals("Puzzle"))
                    {
                        if (player.GetComponent<PlayerController>().GetKey())
                        {
                            player.GetComponent<PlayerController>().SetKey(false);
                            OpenDoor(1);
                        }
                        else
                        {
                            StartCoroutine(player.GetComponent<PlayerController>().ShowMessage("You do not have the key."));
                        }
                    }
                }
            }
            else if (doorOpened)
            {
                payCanvas.SetActive(false);
                OpenDoor(doorNo);
            }
            else
            {
                payCanvas.SetActive(false);
            }
        }
    }

    private void OpenDoor(int door)
    {
        triggerVolume.GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<BoxCollider>().enabled = false;
        doorOpened = true;
        PlayerController.openedDoors[door] = true;
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
