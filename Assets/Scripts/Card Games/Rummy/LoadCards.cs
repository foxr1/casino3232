using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadCards : MonoBehaviour
{
    public Sprite[] cardImgs;
    public Sprite cardBack;
    public GameObject discardPile;

    public GameObject[] playingAreas;
    public GameObject cardObject;
    public GameObject stockPileArea;

    private int noOfPlayerCards = 7;
    private GameObject[] playerDeck = new GameObject[7];
    private GameObject[] opponent1Deck = new GameObject[7];
    private GameObject[] opponent2Deck = new GameObject[7];
    private GameObject[] opponent3Deck = new GameObject[7];
    private List<GameObject> stockPile = new List<GameObject>();
    public List<GameObject[]> allPlayerDecks = new List<GameObject[]>();

    private GameLogic gameLogic;

    // Sounds
    public AudioSource cardPlacedSound;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        gameLogic = GetComponent<GameLogic>();

        allPlayerDecks.Add(playerDeck);
        allPlayerDecks.Add(opponent1Deck);
        allPlayerDecks.Add(opponent2Deck);
        allPlayerDecks.Add(opponent3Deck);

        InitPlayerCards();

        CardMechanics.Shuffle(cardImgs);
        StartCoroutine(SetRandomCards());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("RummyRoom");
        }
    }

    private void InitPlayerCards()
    {
        for (int i = 0; i < playingAreas.Length; i++)
        {
            for (int j = 0; j < noOfPlayerCards; j++)
            {
                GameObject playerCard;
                playerCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);

                playerCard.transform.SetParent(playingAreas[i].transform, false);
                allPlayerDecks[i][j] = playerCard;
            }
        }
    }

    private IEnumerator SetRandomCards()
    {
        int cardNo = 0;
        for (int i = 0; i < noOfPlayerCards; i++)
        {
            for (int j = 0; j < allPlayerDecks.Capacity; j++)
            {
                if (j == 0)
                {
                    allPlayerDecks[j][i] = CardMechanics.CreateCard(cardImgs, cardBack, allPlayerDecks[j][i], cardNo, true);
                } 
                else
                {
                    allPlayerDecks[j][i] = CardMechanics.CreateCard(cardImgs, cardBack, allPlayerDecks[j][i], cardNo, false);
                }


                // If player then show cards, else show back of card for opponents
                if (j == 0)
                {
                    allPlayerDecks[j][i].GetComponent<Image>().sprite = cardImgs[cardNo];
                }
                else
                {
                    allPlayerDecks[j][i].GetComponent<Image>().sprite = cardBack;
                }

                cardNo++;

                cardPlacedSound.Play();
                yield return new WaitForSeconds(0.05f);
            }
        }

        GameObject discardCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
        discardCard = CardMechanics.CreateCard(cardImgs, cardBack, discardCard, cardNo, false);
        discardCard.transform.SetParent(discardPile.transform, false);
        discardCard.transform.localPosition = Vector3.zero;

        discardCard.GetComponent<Image>().sprite = cardImgs[cardNo];

        // Add discard top card to all opponents "known discards"
        foreach (GameObject opponent in gameLogic.opponents)
        {
            opponent.GetComponent<OpponentAI>().knownDiscardedCards.Add(discardCard.GetComponent<Card>());
        }

        for (int k = cardNo + 1; k < 52; k++)
        {
            cardObject = CardMechanics.CreateCard(cardImgs, cardBack, cardObject, k, false);

            GameObject playerCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.GetComponent<Image>().sprite = cardBack;
            playerCard.transform.SetParent(stockPileArea.transform, false);
            playerCard.transform.localPosition = Vector3.zero;

            stockPile.Add(playerCard);
        }
    }
}
