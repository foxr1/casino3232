using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject stockPile, discardPile;

    public GameObject[] opponents;
    public bool opponentsPlaying = false;
    public int opponentPlayer = 0;
    public GameObject messageBox;

    private bool gameWon = false;

    private void Start()
    {
        // Hide message box at start of game
        messageBox.GetComponent<Image>().CrossFadeAlpha(0f, 0f, false);
        messageBox.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0f, 0f, false);
    }

    public void NextTurn()
    {
        StartCoroutine(PlayNextTurn());
    }

    public IEnumerator PlayNextTurn()
    {
        opponentsPlaying = true;
        int i = 1;
        foreach (GameObject opponent in opponents)
        {
            if (!gameWon)
            {
                opponentPlayer = i;
                opponent.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
                StartCoroutine(opponent.GetComponent<OpponentAI>().PlayTurn());
                yield return new WaitForSeconds(1f); // Pause between each turn
                opponent.GetComponent<Image>().CrossFadeAlpha(1f, 0.5f, false);
                i++;
            }
            else
            {
                break;
            }
        }
        opponentsPlaying = false;
        i = 0; // 0 = player, no opponents are playing
    }

    public void OpponentPickedUpCard(Tuple<int, Card> cardPickedUp)
    {
        foreach (GameObject opponent in opponents)
        {
            if (opponent.name[opponent.name.Length - 1] != cardPickedUp.Item1) // Only add to other opponents lists
            {
                
            }

            OpponentAI opponentAI = opponent.GetComponent<OpponentAI>();
            opponentAI.knownOpponentCards.Add(cardPickedUp);

            if (opponentAI.knownDiscardedCards.Contains(cardPickedUp.Item2))
            {
                opponentAI.knownDiscardedCards.Remove(cardPickedUp.Item2);
            }
        }
    }

    public void OpponentCardPlaced(Tuple<int, Card> cardPlaced)
    {
        foreach (GameObject opponent in opponents)
        {
            if (opponent.name[opponent.name.Length - 1] != cardPlaced.Item1) // Only remove from other opponents lists
            {
                OpponentAI opponentAI = opponent.GetComponent<OpponentAI>();
                if (opponentAI.knownOpponentCards != null)
                {
                    if (opponentAI.knownOpponentCards.Contains(cardPlaced))
                    {
                        opponentAI.knownOpponentCards.Remove(cardPlaced);
                    }
                }
                opponentAI.knownDiscardedCards.Add(cardPlaced.Item2);
            }
        }
    }

    public bool CheckWin(GameObject playerCards)
    {
        Card[] cards = playerCards.GetComponentsInChildren<Card>();

        List<int> hearts = new List<int>();
        List<GameObject> heartCards = new List<GameObject>();
        List<int> spades = new List<int>();
        List<GameObject> spadeCards = new List<GameObject>();
        List<int> clubs = new List<int>();
        List<GameObject> clubCards = new List<GameObject>();
        List<int> diamonds = new List<int>();
        List<GameObject> diamondCards = new List<GameObject>();

        List<GameObject> winningCards = new List<GameObject>();

        List<List<int>> suits = new List<List<int>>
        {
            hearts,
            spades,
            clubs,
            diamonds
        };

        List<List<GameObject>> suitCards = new List<List<GameObject>>
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
                hearts.Add(card.cardValue);
                heartCards.Add(card.gameObject);
            }
            else if (card.cardSuit.Equals("Spade"))
            {
                spades.Add(card.cardValue);
                spadeCards.Add(card.gameObject);
            }
            else if (card.cardSuit.Equals("Club"))
            {
                clubs.Add(card.cardValue);
                clubCards.Add(card.gameObject);
            }
            else if (card.cardSuit.Equals("Diamond"))
            {
                diamonds.Add(card.cardValue);
                diamondCards.Add(card.gameObject);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (suits[i].Count > 1) // Check for at least 2 cards in hand
            {
                List<int> tempSuits = suits[i];

                suits[i].Sort();

                for (int j = 0; j < suits[i].Count; j++)
                {
                    if (j + 3 < suits[i].Count)
                    {
                        if ((suits[i][j + 1] - suits[i][j]) == 1 && (suits[i][j + 2] - suits[i][j + 1]) == 1 && (suits[i][j + 3] - suits[i][j + 2]) == 1) // Check each pair of cards has difference of 1 (consecutive numbers), for run of 4 cards
                        {
                            winningCards.Add(suitCards[i][tempSuits.IndexOf(suits[i][j])]);
                            winningCards.Add(suitCards[i][tempSuits.IndexOf(suits[i][j + 1])]);
                            winningCards.Add(suitCards[i][tempSuits.IndexOf(suits[i][j + 2])]);
                            winningCards.Add(suitCards[i][tempSuits.IndexOf(suits[i][j + 3])]);

                            if (suits[i].Count < 6) // If there is less than 6 cards then can't be more than 1 run in hand so stop checking
                            {
                                break;
                            }
                        }
                    }
                   
                    if (j + 2 < suits[i].Count)
                    {
                        if ((suits[i][j + 1] - suits[i][j]) == 1 && (suits[i][j + 2] - suits[i][j + 1]) == 1) // Check each pair of cards has difference of 1 (consecutive numbers), for run of 3 cards
                        {
                            winningCards.Add(suitCards[i][tempSuits.IndexOf(suits[i][j])]);
                            winningCards.Add(suitCards[i][tempSuits.IndexOf(suits[i][j + 1])]);
                            winningCards.Add(suitCards[i][tempSuits.IndexOf(suits[i][j + 2])]);

                            if (suits[i].Count < 6) // If there is less than 6 cards then can't be more than 1 run in hand so stop checking
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (winningCards.Count == 7) // If player's deck consists of 2 runs
        {
            return true;
        }
        else // Check for sets
        {
            List<Card> remainingCards = new List<Card>();
            List<int> remainingCardValues = new List<int>();

            foreach (Card card in cards)
            {
                if (!winningCards.Contains(card.gameObject))
                {
                    remainingCards.Add(card);
                }
            }

            foreach (Card card in remainingCards)
            {
                remainingCardValues.Add(card.cardValue);
            }

            remainingCardValues.Sort();

            if (remainingCardValues.Count == 3)
            {
                if (remainingCardValues[0] == remainingCardValues[1] && remainingCardValues[1] == remainingCardValues[2]) // Check all card values are equal to prove a set exists
                {
                    return true;
                }
            }
            else if (remainingCardValues.Count == 4)
            {
                if (remainingCardValues[0] == remainingCardValues[1] && remainingCardValues[1] == remainingCardValues[2] &&
                    remainingCardValues[2] == remainingCardValues[3])
                {
                    return true;
                }
            }
            else if (remainingCardValues.Count == 7) // No runs so check for 2 sets
            {
                if (remainingCardValues[0] == remainingCardValues[1] && remainingCardValues[1] == remainingCardValues[2] &&
                    remainingCardValues[3] == remainingCardValues[4] && remainingCardValues[4] == remainingCardValues[5] &&
                    remainingCardValues[5] == remainingCardValues[6] ||
                    remainingCardValues[0] == remainingCardValues[1] && remainingCardValues[1] == remainingCardValues[2] &&
                    remainingCardValues[2] == remainingCardValues[3] && remainingCardValues[4] == remainingCardValues[5] &&
                    remainingCardValues[5] == remainingCardValues[6])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public IEnumerator EndGame(bool won)
    {
        gameWon = true;

        yield return new WaitForSeconds(5f);

        if (won)
        {
            PlayerController.hasKey = true;
        }
        
        SceneManager.LoadScene("RummyRoom");
    }

    public void CheckStockPile()
    {
        if (stockPile.transform.childCount == 0)
        {
            for (int i = discardPile.transform.childCount - 1; i > 0; i--)
            {
                GameObject card = discardPile.transform.GetChild(i).gameObject;
                card.transform.SetParent(stockPile.transform);
                card.transform.localPosition = Vector3.zero;
                card.GetComponent<Image>().sprite = card.GetComponent<Card>().backCardSprite;
            }

            // Clear known discarded cards from opponents as deck as been reset
            foreach (GameObject opponent in opponents)
            {
                OpponentAI opponentAI = opponent.GetComponent<OpponentAI>();
                opponentAI.knownDiscardedCards.Clear();
                opponentAI.knownDiscardedCards.Add(discardPile.transform.GetChild(0).GetComponent<Card>());
            }
        }
    }
}
