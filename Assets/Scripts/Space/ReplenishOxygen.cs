using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishOxygen : MonoBehaviour
{
    public OxygenBar oxygenBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            oxygenBar.SetOxygen(1f);
        }
    }
}
