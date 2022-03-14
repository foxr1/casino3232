using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    // Executed in every scene to update the UI with the correct player variables so that they are saved between scenes
    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController>().SetCoinCount(PlayerController.coinCount);
            player.GetComponent<PlayerController>().SetKey(PlayerController.hasKey);
            player.GetComponent<PlayerController>().SetRunningShoes(PlayerController.runningShoes);
            player.GetComponent<PlayerController>().SetUndetectPickpocket(PlayerController.undetectPickpocket);
            player.GetComponent<PlayerController>().SetVolumeSliderValue(PlayerController.volumeSliderValue);
        }
    }
}
