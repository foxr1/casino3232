using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMechanics : MonoBehaviour
{
    public static GameObject CreateCard(Sprite[] cardImgs, Sprite cardBack, GameObject card, int cardNo, bool isInHand)
    {
        int value = int.Parse(System.Text.RegularExpressions.Regex.Match(cardImgs[cardNo].name, @"\d+").Value); // Regex to get numerial part of card name
        string suit = System.Text.RegularExpressions.Regex.Replace(cardImgs[cardNo].name, @"[\d-]", string.Empty); // Regex to get string part of card name

        card.GetComponent<Card>().cardValue = value;
        card.GetComponent<Card>().cardSuit = suit;
        card.GetComponent<Card>().frontCardSprite = cardImgs[cardNo];
        card.GetComponent<Card>().backCardSprite = cardBack;
        card.GetComponent<Card>().isInHand = isInHand;

        return card;
    }

    // Adapted from code found at https://answers.unity.com/questions/1189736/im-trying-to-shuffle-an-arrays-order.html
    public static Sprite[] Shuffle(Sprite[] deck)
    {
        System.Random rnd = new System.Random();

        int n = deck.Length;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            Sprite value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }

        return deck;
    }

    public static IEnumerator ShowMessage(GameObject messageBox, string text)
    {
        messageBox.GetComponentInChildren<TextMeshProUGUI>().text = text;
        messageBox.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(1f, 0.5f, false);
        messageBox.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.5f, false);

        yield return new WaitForSeconds(2f);

        messageBox.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
        messageBox.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0f, 0.5f, false);
    }
}
