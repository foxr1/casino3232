using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponentInChildren<OxygenBar>().ResetStage("You fell into the void of space.");
        }
    }
}
