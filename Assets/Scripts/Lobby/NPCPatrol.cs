using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCPatrol : MonoBehaviour
{
    private GameObject player;
    public GameObject triggerVolume;
    private NPCWander wander;

    [SerializeField]
    private AudioSource spottedSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        wander = GetComponent<NPCWander>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerController.canPickpocket && triggerVolume.GetComponent<CheckForPlayer>().canSeePlayer && !player.GetComponent<PlayerController>().GetUndetectPickpocket())
        {
            StartCoroutine(player.GetComponent<PlayerController>().ShowMessage("The warden has seen you!"));
            wander.chasingPlayer = true;
            spottedSound.Play();
        }
    }

    // If Warden catches player after pickpocketting, send them to "space"
    private void OnTriggerEnter(Collider other)
    {
        if (wander.chasingPlayer && other.gameObject.Equals(player))
        {
            triggerVolume.SetActive(false);
            SceneManager.LoadScene("Space");
        }
    }
}
