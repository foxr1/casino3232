using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    // Adapted from code found at https://www.youtube.com/watch?v=Gtw7VyuMdDc
    public GameObject[] oxygenTanks;
    public GameObject oxygenBar;
    private Vector3 startPos;
    private Transform bar;
    float oxygen = 1f;
    public Image iceImage;
    public AudioSource freezingSound;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Space")))
        {
            freezingSound.Play();
            oxygenBar.SetActive(true);
            bar = transform.GetChild(5).GetChild(5).GetChild(3);
        }
        else
        {
            oxygenBar.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Space")))
        {
            oxygen -= 0.001f;
            if (oxygen > 0)
            {
                SetSize(oxygen);
            }
            else if (oxygen <= 0)
            {
                ResetStage("You ran out of oxygen.");
            }
        }
    }

    private void SetSize(float sizeNormalised)
    {
        bar.localScale = new Vector3(sizeNormalised, 1f);
        Color color = iceImage.color;
        color.a = 1 - sizeNormalised;
        iceImage.color = color;
        freezingSound.volume = 1 - sizeNormalised;
    }

    public void SetOxygen(float amount)
    {
        oxygen = amount;
    }

    public void ResetStage(string message) {
        oxygen = 1f;
        GetComponent<CharacterController>().enabled = false;
        transform.position = startPos;
        GetComponent<CharacterController>().enabled = true;
        StartCoroutine(GetComponent<PlayerController>().ShowMessage(message));

        foreach (GameObject tank in oxygenTanks)
        {
            tank.SetActive(true);
        }
    }
}
