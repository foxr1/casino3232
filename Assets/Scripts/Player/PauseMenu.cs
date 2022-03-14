using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, returnToLobbyBtn;

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReturnToLobby()
    {
        SceneManager.LoadScene("LobbyRoom");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    // Disable "Return to lobby" button when in Space level so can't avoid completing the parkour to get back to the casino.
    public void CheckScene()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Space")))
        {
            returnToLobbyBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            returnToLobbyBtn.GetComponent<Button>().interactable = true;
        }
    }
}
