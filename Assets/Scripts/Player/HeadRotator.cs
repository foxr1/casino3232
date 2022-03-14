using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotator : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public Animator anim;
    private float xRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90); // Can never over rotate to look behind the player on y-axis.
    }

    void OnAnimatorIK()
    {
        anim.SetBoneLocalRotation(HumanBodyBones.Head, Quaternion.Euler(-xRotation, 0f, 0f));
    }
}
