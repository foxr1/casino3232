using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OpponentAI : MonoBehaviour
{
    public GameObject stockPile, discardPile;
    public GameLogic gameLogic;
    private GameObject messageBox;

    public List<Card> knownDiscardedCards = new List<Card>();
    public List<Tuple<int, Card>> knownOpponentCards = new List<Tuple<int, Card>>(); // opponentNo, Card
    private List<Tuple<int, string>> possibleCardChoices = new List<Tuple<int, string>>(); // value, suit
    private List<Card> cardsInHand = new List<Card>();
    private List<Card> cardsToKeep = new List<Card>();
    private List<Card> cardsCanDiscard = new List<Card>();

    // Sounds
    public AudioSource cardPickedUpSound, cardPlacedSound, loseSound;

    private void Start()
    {
        messageBox = GameObject.Find("MessageBox");
    }

    private List<GameObject> GetCards()
    {
        List<GameObject> cardsInHand = new List<GameObject>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            cardsInHand.Add(transform.GetChild(i).gameObject);
        }

        return cardsInHand;
    }

    public IEnumerator PlayTurn()
    {
        // Check if stock pile has reached end
        gameLogic.CheckStockPile();

        cardsInHand = GetComponentsInChildren<Card>().ToList();
        cardsCanDiscard.Clear();
        cardsToKeep.Clear();
        possibleCardChoices.Clear();

        // Check for sets
        Dictionary<int, int> duplicateValues = GetPossibleSetCards(cardsInHand); // [Value, Count]

        // Check runs
        possibleCardChoices.AddRange(GetPossibleRunCards(cardsInHand));

        // Add any cards that aren't duplicates to array to be discarded
        foreach (Card card in cardsInHand)
        {
            if (!duplicateValues.Keys.Contains(card.cardValue) && !cardsToKeep.Contains(card))
            {
                cardsCanDiscard.Add(card);
            }
        }

        Card newCard = PickUpCard(duplicateValues);
        gameLogic.OpponentPickedUpCard(new Tuple<int, Card>(name[name.Length - 1], newCard));
        cardPickedUpSound.Play();
        yield return new WaitForSeconds(0.5f);

        // If list of known discarded cards contains the picked up card then remove
        if (knownDiscardedCards.Contains(newCard))
        {
            knownDiscardedCards.Remove(newCard);
        }

        // Choose which card to discard
        DiscardCard(duplicateValues, cardsCanDiscard);
        cardPlacedSound.Play();
        yield return new WaitForSeconds(0.5f);

        if (gameLogic.CheckWin(gameObject))
        {
            Card[] cards = gameObject.GetComponentsInChildren<Card>();
            foreach (Card card in cards)
            {
                card.gameObject.GetComponent<Image>().sprite = card.frontCardSprite;
            }
            loseSound.Play();
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "Player " + gameObject.name[gameObject.name.Length - 1] + " won the game"));
            StartCoroutine(gameLogic.EndGame(false));
        }

        gameLogic.CheckStockPile();
    }

    private Card PickUpCard(Dictionary<int, int> duplicateValues)
    {
        Card topDiscardCard = discardPile.transform.GetChild(discardPile.transform.childCount - 1).GetComponent<Card>();
        Card newCard;

        // Check discard pile for any duplicate value that matches existing set
        if (duplicateValues.Keys.Contains(topDiscardCard.cardValue))
        {
            newCard = topDiscardCard;
        }
        // If discard pile would add to any of the runs
        else if (possibleCardChoices.Contains(new Tuple<int, string>(topDiscardCard.cardValue, topDiscardCard.cardSuit)))
        {
            newCard = topDiscardCard;
        }
        // Else pick up from stock pile
        else
        {
            newCard = stockPile.transform.GetChild(stockPile.transform.childCount - 1).GetComponent<Card>();
        }

        newCard.transform.SetParent(transform);
        newCard.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newCard.GetComponent<Image>().sprite = newCard.GetComponent<Card>().backCardSprite;

        return newCard;
    }

    private void DiscardCard(Dictionary<int, int> duplicateValues, List<Card> cardsCanDiscard)
    {
        // If the AI knows a card has been discard that they need to win, remove from card choices and add to discard list
        foreach (Card card in knownDiscardedCards)
        {
            Tuple<int, string> tupleCard = new Tuple<int, string>(card.cardValue, card.cardSuit);

            if (possibleCardChoices.Contains(tupleCard))
            {
                possibleCardChoices.Remove(tupleCard);
            }

            // If AI has run that is contained within known discarded cards 
            if (duplicateValues.Keys.Contains(card.cardValue))
            {
                int count;
                duplicateValues.TryGetValue(card.cardValue, out count);

                if (count < 3) // If AI only has 2 or less duplicate cards then discard as will be difficult to get run 
                {
                    duplicateValues.Remove(card.cardValue);
                }
            }
        }

        // Check if this opponent has any cards the next opponent might need

        // Get opponents deck
        List<Card> opponentsCards = new List<Card>();
        foreach (Tuple<int, Card> tOpponentCard in knownOpponentCards)
        {
            int thisOpponentNo = name[name.Length - 1];

            if (thisOpponentNo == tOpponentCard.Item1 + 1 || thisOpponentNo == 3 && tOpponentCard.Item1 == 0) // Only check next opponents cards
            {
                opponentsCards.Add(tOpponentCard.Item2);
            }
        }

        // Get opponents possible cards
        List<Tuple<int, string>> possibleOpponentCards = new List<Tuple<int, string>>();
        possibleOpponentCards.AddRange(GetPossibleRunCards(opponentsCards));

        // If possible opponent card choices are in cards to discard, remove to stop giving advantage to other players
        List<Card> cardsToRemove = new List<Card>(); // Create list as cannot remove from list during loop
        foreach (Tuple<int, string> card in possibleOpponentCards)
        {
            foreach (Card discardCard in cardsCanDiscard)
            {
                Tuple<int, string> tupleCard = new Tuple<int, string>(discardCard.cardValue, discardCard.cardSuit);

                if (tupleCard.Equals(card))
                {
                    cardsToRemove.Add(discardCard);
                }
            }
        }

        if (cardsToRemove.Count > 0)
        {
            foreach (Card card in cardsToRemove)
            {
                if (cardsCanDiscard.Contains(card))
                {
                    cardsCanDiscard.Remove(card);
                }
            }
        }

        Card cardToDiscard = null;
        if (duplicateValues.Count != 0 && cardsCanDiscard.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, cardsCanDiscard.Count);
            cardToDiscard = cardsCanDiscard[index];
            
        }
        else if (duplicateValues.Count == 0 || duplicateValues.Count != 0 && cardsCanDiscard.Count == 0)
        {
            int index = UnityEngine.Random.Range(0, cardsInHand.Count);
            cardToDiscard = cardsInHand[index];
        }
        else if (cardsCanDiscard.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, cardsCanDiscard.Count);
            cardToDiscard = cardsCanDiscard[index];
        }
        else
        {
            int index = UnityEngine.Random.Range(0, cardsInHand.Count);
            cardToDiscard = cardsInHand[index];
        }

        cardToDiscard.GetComponent<Image>().sprite = cardToDiscard.GetComponent<Card>().frontCardSprite;
        cardToDiscard.transform.SetParent(discardPile.transform);
        cardToDiscard.transform.localPosition = Vector3.zero;
        cardToDiscard.transform.localRotation = Quaternion.Euler(0, 0, 0);

        knownDiscardedCards.Add(cardToDiscard);
        gameLogic.OpponentCardPlaced(new Tuple<int, Card>(name[name.Length - 1], cardToDiscard));
    }

    private List<Tuple<int, string>> GetPossibleRunCards(List<Card> cards)
    {
        List<Tuple<int, string>> possibleCardChoices = new List<Tuple<int, string>>();

        List<Card> heartCards = new List<Card>();
        List<Card> spadeCards = new List<Card>();
        List<Card> clubCards = new List<Card>();
        List<Card> diamondCards = new List<Card>();

        List<string> suitNames = new List<string>
        {
            "Heart",
            "Spade",
            "Club",
            "Diamond"
        };

        List<List<Card>> suitCards = new List<List<Card>>
        {
            heartCards,
            spadeCards,
            clubCards,
            diamondCards
        };

        foreach (Card card in cards)
        {
            if (card.cardSuit.Equals("Heart"))
            {
                heartCards.Add(card);
            }
            else if (card.cardSuit.Equals("Spade"))
            {
                spadeCards.Add(card);
            }
            else if (card.cardSuit.Equals("Club"))
            {
                clubCards.Add(card);
            }
            else if (card.cardSuit.Equals("Diamond"))
            {
                diamondCards.Add(card);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (suitCards[i].Count > 1) // Check for at least 2 cards in hand
            {
                List<Card> tempSuits = suitCards[i];

                suitCards[i].Sort();

                for (int j = 0; j < suitCards[i].Count; j++)
                {
                    if ((j + 1) < suitCards[i].Count)
                    {
                        if ((suitCards[i][j + 1].cardValue - suitCards[i][j].cardValue) == 1) // Consecutive cards (i.e. 2C and 3C)
                        {
                            cardsToKeep.Add(suitCards[i][tempSuits.IndexOf(suitCards[i][j])]);
                            cardsToKeep.Add(suitCards[i][tempSuits.IndexOf(suitCards[i][j + 1])]);

                            if ((suitCards[i][j].cardValue - 1) != 0)
                            {
                                possibleCardChoices.Add(new Tuple<int, string>(suitCards[i][j].cardValue - 1, suitNames[i])); // Card in front of first card (i.e. AC)
                            }
                            if ((suitCards[i][j].cardValue + 1) != 14)
                            {
                                possibleCardChoices.Add(new Tuple<int, string>(suitCards[i][j + 1].cardValue + 1, suitNames[i])); // Card after second card (i.e. 5C)
                            }
                        }
                        else if ((suitCards[i][j + 1].cardValue - suitCards[i][j].cardValue) == 2) // Gap in middle of two cards (i.e. 2C and 4C)
                        {
                            cardsToKeep.Add(suitCards[i][tempSuits.IndexOf(suitCards[i][j])]);
                            cardsToKeep.Add(suitCards[i][tempSuits.IndexOf(suitCards[i][j + 1])]);

                            if ((suitCards[i][j].cardValue - 1) != 0)
                            {
                                possibleCardChoices.Add(new Tuple<int, string>(suitCards[i][j].cardValue - 1, suitNames[i])); // Card in front of first card (i.e. AC)
                            }
                            possibleCardChoices.Add(new Tuple<int, string>(suitCards[i][j].cardValue + 1, suitNames[i])); // Middle card (i.e. 3C)
                            if ((suitCards[i][j].cardValue + 1) != 14)
                            {
                                possibleCardChoices.Add(new Tuple<int, string>(suitCards[i][j + 1].cardValue + 1, suitNames[i])); // Card after second card (i.e. 5C)
                            }
                        }
                        else if (suitCards[i].Count == 2 && suitCards[i][j + 1].cardValue - suitCards[i][j].cardValue == 3)
                        { // If there is a gap of 2 between 2 cards (i.e. 2C and 5C)
                            cardsToKeep.Add(suitCards[i][tempSuits.IndexOf(suitCards[i][j])]);
                            cardsToKeep.Add(suitCards[i][tempSuits.IndexOf(suitCards[i][j + 1])]);

                            if ((suitCards[i][j].cardValue + 1) != 14)
                            {
                                possibleCardChoices.Add(new Tuple<int, string>(suitCards[i][j + 1].cardValue + 1, suitNames[i])); // First middle card (i.e. 3C)
                            }
                            if ((suitCards[i][j].cardValue + 2) != 14)
                            {
                                possibleCardChoices.Add(new Tuple<int, string>(suitCards[i][j + 1].cardValue + 1, suitNames[i])); // Second middle card (i.e. 4C)
                            }
                        }
                    }
                }
            }
        }

        return possibleCardChoices;
    }

    private Dictionary<int, int> GetPossibleSetCards(List<Card> cards)
    {
        List<int> cardValues = new List<int>();

        foreach (Card card in cards)
        {
            cardValues.Add(card.cardValue);
        }

        cardValues.Sort();

        Dictionary<int, int> duplicates = cardValues.GroupBy(x => x).Where(g => g.Count() > 1).ToDictionary(x => x.Key, x => x.Count()); // [Value, Count]

        return duplicates;
    }
}
