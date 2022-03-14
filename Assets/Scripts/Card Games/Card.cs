using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, IComparable<Card>
{
    public int cardValue;
    public string cardSuit;
    public Sprite frontCardSprite, backCardSprite;
    public bool isInHand;

    public Card(int value, string suit, Sprite frontSprite, Sprite backSprite, bool inHand)
    {
        cardValue = value;
        cardSuit = suit;
        frontCardSprite = frontSprite;
        backCardSprite = backSprite;
        isInHand = inHand;
    }

    public int CompareTo(Card other)
    {
        return cardValue.CompareTo(other.cardValue);
    }
}
