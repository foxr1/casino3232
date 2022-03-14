using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Adapted from code found at: https://www.youtube.com/watch?v=xNHSGMKtlv4
public class ChangeMasterVolume : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    private void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            GetComponent<Slider>().value = GameObject.Find("Player").GetComponent<PlayerController>().GetVolumeSliderValue();
        }
    }

    public void SetLevel(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        PlayerController.volumeSliderValue = volume;
    }
}
