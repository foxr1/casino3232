using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public GameObject successItem;
    public bool inverse;

    public Material lockedMaterial, unlockedMaterial;

    private void Update()
    {
        if (successItem.GetComponent<PickUpObject>().isHolding && !inverse || !successItem.GetComponent<PickUpObject>().isHolding && inverse)
        {
            GetComponent<MeshRenderer>().material = unlockedMaterial;
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else if (!successItem.GetComponent<PickUpObject>().isHolding && !inverse || successItem.GetComponent<PickUpObject>().isHolding && inverse)
        {
            GetComponent<MeshRenderer>().material = lockedMaterial;
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
