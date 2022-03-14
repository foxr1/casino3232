using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoFloor : MonoBehaviour
{
    public Material[] materials;
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += 0.01f;

        if (timer > 2f)
        {
            foreach (Transform section in transform)
            {
                int rndIndex = Random.Range(0, 5);

                section.gameObject.GetComponent<Renderer>().material = materials[rndIndex];
            }

            timer = 0;
        }
    }
}
