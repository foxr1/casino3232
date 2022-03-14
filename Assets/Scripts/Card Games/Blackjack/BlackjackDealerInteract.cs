using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackDealerInteract : MonoBehaviour
{
    private GameObject player, guide;
    public GameObject canvas;
    private bool mouseOver, keyPurchased = false;
    private float distanceToObject, maximumDistance = 6f;
    public AudioSource itemPurchasedSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        guide = player.transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToObject = Vector3.Distance(transform.position, guide.transform.position);

        // Show option to buy key as long as player is close enough
        if (!keyPurchased && mouseOver && distanceToObject <= maximumDistance)
        {
            canvas.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                int coins = player.GetComponent<PlayerController>().GetCoinCount();

                if (coins > 250)
                {
                    itemPurchasedSound.Play();
                    player.GetComponent<PlayerController>().SetCoinCount(coins -= 250);
                    player.GetComponent<PlayerController>().SetKey(true);
                    StartCoroutine(player.GetComponent<PlayerController>().ShowMessage("You have received a key!"));
                    GetComponent<CapsuleCollider>().enabled = false;
                    keyPurchased = true;
                }
                else
                {
                    StartCoroutine(player.GetComponent<PlayerController>().ShowMessage("You do not have enough coins."));
                }
            }
        }
        else
        {
            canvas.SetActive(false);
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
