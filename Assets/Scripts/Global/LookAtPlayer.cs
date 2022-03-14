using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject player;
    private Animator anim;
    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
    }

    // Rotate head to always be facing player with code adpated from https://answers.unity.com/questions/161053/making-an-object-rotate-to-face-another-object.html
    void OnAnimatorIK()
    {
        var lookPos = playerPos - transform.localPosition;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        anim.SetBoneLocalRotation(HumanBodyBones.Neck, rotation);
    }
}
