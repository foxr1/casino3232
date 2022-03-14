using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicVolume : MonoBehaviour
{
    private AudioSource lobbyMusic;

    private void Start()
    {
        lobbyMusic = GameObject.Find("Dome").GetComponent<AudioSource>();
    }

    public void ChangeVolume(float volume)
    {
        lobbyMusic.volume = volume;
    }
}
