using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private float speed = 5f;
    private Quaternion randomRotation;

    private void Start()
    {
        randomRotation = Random.rotation;
        StartCoroutine(MoveToRandomRotation());
        StartCoroutine(SetRandomRotation());
    }

    IEnumerator MoveToRandomRotation()
    {
        while (true)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, randomRotation, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SetRandomRotation()
    {
        while (true)
        {
            randomRotation = Random.rotation;
            yield return new WaitForSeconds(1f);
        }
    }
}
