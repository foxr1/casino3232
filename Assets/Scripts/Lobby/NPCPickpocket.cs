using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCPickpocket : MonoBehaviour
{
    float distanceToObject;
    float maximumDistance = 10f;
    private bool mouseOver = false, receivingCoins = false;
    public Canvas canvas;

    // Player variables
    private GameObject player;
    private GameObject guide;
    private int noOfCoins = 0;

    // NPC variables
    public GameObject triggerVolume;
    private bool canSeePlayer = false;
    private Material originalMaterial;
    public Material seenMaterial;
    private NPCWander wander;

    [SerializeField]
    private AudioSource spottedSound;

    private void Start()
    {
        player = GameObject.Find("Player");
        if (player != null) guide = player.transform.GetChild(0).GetChild(0).gameObject;
        originalMaterial = GetComponentInChildren<Renderer>().material;
        wander = GetComponent<NPCWander>();
    }

    private void FixedUpdate()
    {
        if (guide != null)
        {
            distanceToObject = Vector3.Distance(transform.position, guide.transform.position);

            // Show option to pickpocket NPC as long as player is close enough and crouching
            if (mouseOver && distanceToObject <= maximumDistance && player.GetComponent<PlayerController>().isCrouching)
            {
                canvas.GetComponent<Canvas>().enabled = true;
                PlayerController.canPickpocket = true;

                if (Input.GetKey(KeyCode.E))
                {
                    // Give money to player as long as player is not in view of NPC
                    if (!triggerVolume.GetComponent<CheckForPlayer>().canSeePlayer && !canSeePlayer)
                    {
                        if (!receivingCoins)
                        {
                            StartCoroutine(GiveCoins());
                        }
                    }
                    else if (!player.GetComponent<PlayerController>().GetUndetectPickpocket())
                    {
                        spottedSound.Play();
                        StartCoroutine(player.GetComponent<PlayerController>().ShowMessage("You have been spotted!"));
                        GetComponentInChildren<Renderer>().material = seenMaterial;
                        canSeePlayer = true;
                        wander.chasingPlayer = true;
                        StartCoroutine(SeePlayerTimer());
                    }
                }

            }
            else
            {
                canvas.GetComponent<Canvas>().enabled = false;
                PlayerController.canPickpocket = false;
            }
        }
        else
        {
            canvas.GetComponent<Canvas>().enabled = false;
        }
    }

    private IEnumerator GiveCoins()
    {
        receivingCoins = true;

        noOfCoins = player.GetComponent<PlayerController>().GetCoinCount();
        noOfCoins++;
        player.GetComponent<PlayerController>().SetCoinCount(noOfCoins);

        // The closer the player is to the NPC the less time it waits to take more money (When the player is closer to the NPC they pickpocket faster)
        yield return new WaitForSeconds(distanceToObject / 20);

        receivingCoins = false;
    }

    // If NPC catches player after pickpocketting, send them to "space"
    private void OnTriggerEnter(Collider other)
    {
        if (wander.chasingPlayer && other.gameObject.Equals(player))
        {
            SceneManager.LoadScene("Space");
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

    // Stop chasing player after 5 seconds
    IEnumerator SeePlayerTimer()
    {
        yield return new WaitForSeconds(5f);
        wander.chasingPlayer = false;
        canSeePlayer = false;
        GetComponentInChildren<Renderer>().material = originalMaterial;
    }
}
