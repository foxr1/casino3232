using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackjackGameLogic : MonoBehaviour
{
    // Card Information
    public Sprite[] cardImgs;
    public Sprite cardBack;
    public GameObject discardPile, stockPile, cardObject, playerCardArea, dealerCardArea;
    private int currentCardNo;

    // UI Elements
    public GameObject messageBox, startScreen, restartScreen, coinAmount, playerScore;

    // Betting
    public GameObject betAmount;
    private int bet, coins;

    // Game Mechanics
    private bool gameInPlay = false;

    // Sounds
    public AudioSource winSound, drawSound, loseSound, cardPlaceSound;

    // Start is called before the first frame update
    void Start()
    {
        // Hide message box at start of game
        messageBox.GetComponent<Image>().CrossFadeAlpha(0f, 0f, false);
        messageBox.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0f, 0f, false);
        
        // Get amount of coins from player
        coins = PlayerController.coinCount;
        coinAmount.GetComponent<TextMeshProUGUI>().text = coins.ToString();

        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("BlackjackRoom");
        }
    }

    public void Restart(bool option)
    {
        if (option)
        {
            SceneManager.LoadScene("Blackjack2D");
        } 
        else
        {
            SceneManager.LoadScene("BlackjackRoom");
        }
    }

    public void PlaceBet()
    {
        bet = int.Parse(betAmount.GetComponent<TMP_InputField>().text);
        
        if (bet > coins)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You do not have enough coins."));
        }
        else
        {
            coins -= bet;
            PlayerController.coinCount = coins;
            coinAmount.GetComponent<TextMeshProUGUI>().text = (PlayerController.coinCount).ToString();
            startScreen.SetActive(false);

            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        gameInPlay = true;
        CardMechanics.Shuffle(cardImgs);

        for (currentCardNo = 0; currentCardNo < 4; currentCardNo++)
        {
            yield return new WaitForSeconds(0.75f);

            GameObject card;
            card = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);

            // Disable Rummy functionality from cards as not needed here
            card.GetComponent<DragDrop>().enabled = false;
            card.GetComponent<EventTrigger>().enabled = false;

            // Alternate between player and dealer
            if (currentCardNo % 2 == 0)
            {
                card.transform.SetParent(playerCardArea.transform, false);
            }
            else
            {
                card.transform.SetParent(dealerCardArea.transform, false);
            }

            card = CardMechanics.CreateCard(cardImgs, cardBack, card, currentCardNo, false);

            if (currentCardNo == 1) // First card dealt to dealer is face down
            {
                card.GetComponent<Image>().sprite = cardBack;
            }
            else
            {
                card.GetComponent<Image>().sprite = cardImgs[currentCardNo];
            }

            cardPlaceSound.Play();
            playerScore.GetComponent<TextMeshProUGUI>().text = GetScore(playerCardArea).ToString();
        }

        // Check if player has already got Blackjack
        if (GetScore(playerCardArea) == 21)
        {
            StartCoroutine(CheckWin());
        }

        gameInPlay = false;

        yield return null;
    }

    public void Hit()
    {
        if (!gameInPlay)
        {
            cardPlaceSound.Play();

            GameObject newCard;
            newCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(playerCardArea.transform, false);
            newCard = CardMechanics.CreateCard(cardImgs, cardBack, newCard, currentCardNo, false);
            newCard.GetComponent<Image>().sprite = cardImgs[currentCardNo];

            currentCardNo++;

            playerScore.GetComponent<TextMeshProUGUI>().text = GetScore(playerCardArea).ToString();

            if (GetScore(playerCardArea) > 21)
            {
                StartCoroutine(Bust());
            }
            else if (GetScore(playerCardArea) == 21) // Automatically stand if player gets Blackjack as no reason they would want to "hit" again
            {
                StartCoroutine(Stand());
            }
        }
        else
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "Cannot play while game is in play."));
        }
    }

    private int GetScore(GameObject deck)
    {
        Card[] cards = deck.GetComponentsInChildren<Card>();
        int totalScore = 0;
        foreach (Card card in cards)
        {
            int score = 0;
            if (card.cardValue > 10)
            {
                score = 10;
            }
            else if (card.cardValue == 1) // Ace can be either 1 or 11 so let game decide which value it should be
            {
                if ((totalScore + 11) > 21)
                {
                    score = 1;
                } else {
                    score = 11;
                }
            }
            else
            {
                score = card.cardValue;
            }

            totalScore += score;
        }

        return totalScore;
    }

    // Separate function for stand button as can't call IEnumerator function from button click
    public void StandButton()
    {
        if (!gameInPlay)
        {
            StartCoroutine(Stand());
        }
        else
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "Cannot play while game is in play."));
        }
    }

    private IEnumerator Stand()
    {
        while (GetScore(dealerCardArea) < 17) // Dealer will always "hit" as long as their hand is worth less than 17
        {
            GameObject newCard;
            newCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(dealerCardArea.transform, false);
            newCard = CardMechanics.CreateCard(cardImgs, cardBack, newCard, currentCardNo, false);
            newCard.GetComponent<Image>().sprite = cardImgs[currentCardNo];

            currentCardNo++;

            yield return new WaitForSeconds(0.75f);
        }

        StartCoroutine(CheckWin());
    }

    private IEnumerator CheckWin()
    {
        // Flip over dealers first card and spread them out
        dealerCardArea.GetComponentsInChildren<Card>()[0].GetComponent<Image>().sprite = dealerCardArea.GetComponentsInChildren<Card>()[0].frontCardSprite;
        dealerCardArea.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 0);

        if (GetScore(playerCardArea) == 21)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You got Blackjack!"));
            coinAmount.GetComponent<TextMeshProUGUI>().text = (coins += Mathf.CeilToInt(bet * 1.5f)).ToString();
            winSound.Play();
        }
        else if ((GetScore(playerCardArea) > GetScore(dealerCardArea)) || (GetScore(playerCardArea) < GetScore(dealerCardArea)) && GetScore(dealerCardArea) > 21)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You beat the dealer!"));
            coinAmount.GetComponent<TextMeshProUGUI>().text = (coins += bet * 2).ToString();
            winSound.Play();
        }
        else if (GetScore(playerCardArea) == GetScore(dealerCardArea))
        {
            coinAmount.GetComponent<TextMeshProUGUI>().text = (coins - bet).ToString();
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You tied."));
            coinAmount.GetComponent<TextMeshProUGUI>().text = (coins += bet).ToString();
            drawSound.Play();
        } 
        else if ((GetScore(playerCardArea) < GetScore(dealerCardArea)) && GetScore(dealerCardArea) <= 21)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You lost."));
            loseSound.Play();
        }

        PlayerController.coinCount = coins;

        yield return new WaitForSeconds(3f);

        restartScreen.SetActive(true);
    }

    public IEnumerator Bust()
    {
        dealerCardArea.GetComponentsInChildren<Card>()[0].GetComponent<Image>().sprite = dealerCardArea.GetComponentsInChildren<Card>()[0].frontCardSprite;
        dealerCardArea.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 0);

        StartCoroutine(CardMechanics.ShowMessage(messageBox, "You have gone bust."));
        loseSound.Play();

        yield return new WaitForSeconds(3f);

        restartScreen.SetActive(true);
    }
}
