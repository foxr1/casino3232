using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSuccess : MonoBehaviour
{
    private GameObject player;
    private Restart restart;
    public GameObject roomToMoveTo;

    public Material keyMaterial, itemMaterial, playerMaterial;

    public AudioSource winSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        restart = GameObject.Find("Player").GetComponent<Restart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((GetComponent<MeshRenderer>().material.color == itemMaterial.color && other.gameObject.CompareTag("Item")) ||
            (GetComponent<MeshRenderer>().material.color == keyMaterial.color && other.gameObject.CompareTag("Key")) ||
            (GetComponent<MeshRenderer>().material.color == playerMaterial.color && other.gameObject.CompareTag("Player")))
        {
            winSound.Play();

            StartCoroutine(player.GetComponent<PlayerController>().ShowMessage(restart.currentRoom.name + " Complete"));

            player.transform.SetParent(roomToMoveTo.transform);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.localPosition = new Vector3(17.5f, 0, -10);
            player.GetComponent<CharacterController>().enabled = true;
            restart.currentRoom = roomToMoveTo;
            if (!other.CompareTag("Player"))
            {
                other.gameObject.SetActive(false); // Remove object that won the level to avoid duplicate wins being called.
            }

            // Award the player 500 coins for completing all the levels
            if (restart.currentRoom.name.Equals("All Levels"))
            {
                PlayerController.coinCount += 500;
                player.GetComponent<PlayerController>().SetCoinCount(PlayerController.coinCount);
            }
        }
    }
}
