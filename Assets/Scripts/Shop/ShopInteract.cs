using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteract : MonoBehaviour
{
    private PlayerController player;

    [SerializeField]
    private AudioSource itemPurchasedSound;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void UndetectablePickpocketing()
    {
        
        if (player.GetUndetectPickpocket())
        {
            StartCoroutine(player.ShowMessage("You already have Undetectable Pickpocketing!"));
        }
        else if (player.GetCoinCount() < 500)
        {
            StartCoroutine(player.ShowMessage("You do not have enough coins."));
        }
        else if (player.GetCoinCount() >= 500)
        {
            player.SetCoinCount(player.GetCoinCount() - 500);

            StartCoroutine(player.ShowMessage("You purchased the Undetectable Pickpocketing!"));
            player.SetUndetectPickpocket(true);
            itemPurchasedSound.Play();
        }
    }

    public void RunningShoes()
    {
        if (player.GetRunningShoes())
        {
            StartCoroutine(player.ShowMessage("You already have the Running Shoes!"));
        }
        else if (player.GetCoinCount() < 500)
        {
            StartCoroutine(player.ShowMessage("You do not have enough coins."));
        }
        else if (player.GetCoinCount() >= 500)
        {
            player.SetCoinCount(player.GetCoinCount() - 500);

            StartCoroutine(player.ShowMessage("You purchased the Running Shoes!"));
            player.SetRunningShoes(true);
            itemPurchasedSound.Play();
        }
    }
}
