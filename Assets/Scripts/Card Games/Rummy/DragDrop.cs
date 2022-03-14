using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    // Adapted code from tutorial found at https://www.youtube.com/watch?v=0-dUB52eEMk

    // Card dragging
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private bool isOverDiscardPile = false;
    private GameObject dropZone;
    private Vector2 startPosition;
    
    // UI Elements
    public GameObject moveDisplay, leftButton, rightButton;
    private GameObject messageBox;

    private GameObject playerCards;
    private GameLogic gameLogic;

    // Sounds
    public AudioSource winSound, cardPickedUpSound, cardPlacedSound;

    void Start()
    {
        playerCards = GameObject.Find("PlayerArea");
        gameLogic = GameObject.Find("Board").GetComponent<GameLogic>();

        messageBox = GameObject.Find("MessageBox");

        ToggleMoveDisplay(false, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;

        if (collision.gameObject == GameObject.Find("DiscardCardPile"))
        {
            isOverDiscardPile = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;

        if (collision.gameObject == GameObject.Find("DiscardCardPile"))
        {
            isOverDiscardPile = false;
        }
    }

    public void StartDrag()
    {
        if (playerCards.transform.childCount == 7 && gameObject.GetComponent<Card>().isInHand == true)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You must pick up a card first"));
        } 
        else if (transform.parent.name != "StockPile" && transform.parent.name != "DiscardCardPile" &&
            transform.parent.name != "PlayerArea")
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You cannot look at opponent's cards"));
        }
        else if (playerCards.transform.childCount == 8 && !gameObject.GetComponent<Card>().isInHand)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You cannot pick up anymore cards"));
        }
        else if (gameLogic.opponentsPlaying == true)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "Opponents are still playing"));
        }
        else
        {
            cardPickedUpSound.Play();

            StartCoroutine(MainMenu.FadeCGAlpha(1f, 0f, 0.1f, GetComponentInChildren<CanvasGroup>()));

            startPosition = transform.position;
            isDragging = true;
        }
    }

    public void EndDrag()
    {
        if (playerCards.transform.childCount == 7 && gameObject.GetComponent<Card>().isInHand == true)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You must pick up a card first"));
        }
        else if (transform.parent.name != "StockPile" && transform.parent.name != "DiscardCardPile" &&
            transform.parent.name != "PlayerArea")
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You cannot look at opponent's cards"));
        }
        else if (playerCards.transform.childCount == 8 && !gameObject.GetComponent<Card>().isInHand)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You cannot pick up anymore cards"));
        }
        else if (gameLogic.opponentsPlaying == true)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "Opponents are still playing"));
        }
        else if (!gameObject.GetComponent<Card>().isInHand && isOverDiscardPile)
        {
            isDragging = false;
            transform.position = startPosition;
        }
        else
        {
            cardPlacedSound.Play();

            isDragging = false;
            if (isOverDropZone)
            {
                if (gameObject.GetComponent<Image>().sprite.name == "BackColor_Black")
                {
                    gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<Card>().frontCardSprite;
                }
                else
                {
                    // Only add card to known opponent cards if the card was already faced up
                    gameLogic.OpponentPickedUpCard(new System.Tuple<int, Card>(0, GetComponent<Card>()));
                }

                transform.SetParent(dropZone.transform, false);

                if (isOverDiscardPile)
                {
                    transform.localPosition = Vector3.zero;
                    gameObject.GetComponent<Card>().isInHand = false;
                    if (!gameLogic.CheckWin(playerCards))
                    {
                        gameLogic.OpponentCardPlaced(new System.Tuple<int, Card>(0, GetComponent<Card>()));
                        gameLogic.NextTurn();
                    }
                    else
                    {
                        winSound.Play();
                        StartCoroutine(CardMechanics.ShowMessage(messageBox, "You won the game!"));
                        StartCoroutine(gameLogic.EndGame(true));
                    }
                }
                else
                {
                    gameObject.GetComponent<Card>().isInHand = true;
                }
            }
            else
            {
                transform.position = startPosition;
            }
        }
    }

    // Show options to move cards left and right as long as the user's cursor is over the card and is in their hand
    public void PointerEnter()
    {
        ToggleMoveDisplay(true, 0.1f);
    }
    public void PointerExit()
    {
        ToggleMoveDisplay(false, 0.1f);
    }

    // Show options to move cards left and right as long as the user's cursor is over the card and is in their hand
    private void ToggleMoveDisplay(bool state, float duration)
    {
        if (gameObject.GetComponent<Card>().isInHand)
        {
            if (state)
            {
                StartCoroutine(MainMenu.FadeCGAlpha(0f, 1f, duration, GetComponentInChildren<CanvasGroup>()));
                leftButton.GetComponent<Button>().interactable = true;
                rightButton.GetComponent<Button>().interactable = true;
            }
            else if (!state)
            {
                StartCoroutine(MainMenu.FadeCGAlpha(1f, 0f, duration, GetComponentInChildren<CanvasGroup>()));
                leftButton.GetComponent<Button>().interactable = false;
                rightButton.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            StartCoroutine(MainMenu.FadeCGAlpha(1f, 0f, 0f, GetComponentInChildren<CanvasGroup>()));
            leftButton.GetComponent<Button>().interactable = false;
            rightButton.GetComponent<Button>().interactable = false;
        }
    }

    public void MoveLeft()
    {
        if (transform.GetSiblingIndex() > 0)
        {
            transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        }
        else  // Loop to end of hand if at beginning of hand
        {
            transform.SetSiblingIndex(playerCards.transform.childCount - 1);
        }
    }

    public void MoveRight()
    {
        if (transform.GetSiblingIndex() == playerCards.transform.childCount - 1)
        {
            transform.SetSiblingIndex(0);
        }
        else if (transform.GetSiblingIndex() < playerCards.transform.childCount) // Loop back to start of hand if at end of hand
        {
            transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        }
    }
}
