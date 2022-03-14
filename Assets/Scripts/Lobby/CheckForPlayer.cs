using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForPlayer : MonoBehaviour
{
    private GameObject player;
    public bool canSeePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(player))
        {
            canSeePlayer = true;
        }
        else
        {
            canSeePlayer = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canSeePlayer = false;
    }
}
