using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// As camera for player is within a prefab, use this script
public class GetPlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
