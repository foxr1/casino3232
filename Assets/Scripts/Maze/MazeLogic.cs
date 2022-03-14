using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MazeLogic : MonoBehaviour
{
    // Maze
    public MazeRenderer maze;

    // UI Elements
    public GameObject messageBox, startScreen, restartScreen, coinAmount, selectedText, betMultiplierText;

    // Betting
    private string pickedColour = "";
    public GameObject betAmount;
    private int bet, coins;

    // Sounds
    public AudioSource winSound, loseSound;

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
            SceneManager.LoadScene("MazeRoom");
        }
    }

    public void PlaceBet()
    {
        bet = int.Parse(betAmount.GetComponent<TMP_InputField>().text);

        if (bet > coins)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You do not have enough coins."));
        }
        else if (pickedColour == "")
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You must pick a colour to bet on."));
        }
        else if (maze.buildingMaze)
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "Please wait for maze to finish building."));
        }
        else
        {
            coins -= bet;
            PlayerController.coinCount = coins;
            coinAmount.GetComponent<TextMeshProUGUI>().text = (PlayerController.coinCount).ToString();
            startScreen.SetActive(false);

            maze.StartGame();
        }
    }

    public void SetMouse(string colour)
    {
        pickedColour = colour;
        selectedText.GetComponent<TextMeshProUGUI>().text = "SELECTED: <color=" + colour.ToLower() + ">" + colour.ToUpper() + "</color>";
    }

    public IEnumerator GameWon(string winningColour)
    {
        if (pickedColour.Equals(winningColour))
        {
            winSound.Play();
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You won the game!"));
            coinAmount.GetComponent<TextMeshProUGUI>().text = (coins += (int)(bet * maze.betMultiplier)).ToString();
        } 
        else
        {
            loseSound.Play();
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "You lost the game."));
        }

        // Stop agents moving
        NavMeshAgent[] agents = maze.GetComponentsInChildren<NavMeshAgent>();

        foreach (NavMeshAgent agent in agents)
        {
            agent.enabled = false;
        }

        PlayerController.coinCount = coins;

        yield return new WaitForSeconds(3f);

        restartScreen.SetActive(true);
    }

    public void Restart(bool option)
    {
        restartScreen.SetActive(false);

        if (option)
        {
            DestroyAndRecreateMaze();

            startScreen.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MazeRoom");
        }
    }

    public void DestroyAndRecreateMaze()
    {
        maze.buildingMaze = true;

        // Reset "mice" position
        Rigidbody[] gameElements = maze.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody elm in gameElements)
        {
            Destroy(elm.gameObject);
        }

        // Delete exisitng maze and reset
        BoxCollider[] mazePieces = maze.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider piece in mazePieces)
        {
            if (piece != null)
            {
                if (piece.name.Equals("Wall(Clone)") || piece.name.Equals("Floor(Clone)"))
                {
                    Destroy(piece.gameObject);
                }
            }
        }

        maze.CreateMaze();
    }
}
