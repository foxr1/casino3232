using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private CanvasGroup activeScreen;
    public CanvasGroup mainMenu, instructions, options;
    public GameObject backButton, playerObj;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock cursor if returning from lobby
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && activeScreen != mainMenu)
        {
            HideScreen(activeScreen);
        }
    }

    public void PlayGame()
    {
        // If user has exited game then re-entered lobby, reset coins back to 50, remove key and close doors
        PlayerController.coinCount = 50;
        PlayerController.openedDoors[0] = false;
        PlayerController.openedDoors[1] = false;
        PlayerController.hasKey = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowScreen(CanvasGroup screen)
    {
        activeScreen = screen;
        screen.blocksRaycasts = true;
        mainMenu.blocksRaycasts = false;
        StartCoroutine(FadeCGAlpha(1f, 0f, 0.5f, mainMenu));
        StartCoroutine(FadeCGAlpha(0f, 1f, 0.5f, screen));
    }

    public void HideScreen(CanvasGroup screen)
    {
        activeScreen = mainMenu;
        screen.blocksRaycasts = false;
        mainMenu.blocksRaycasts = true;
        StartCoroutine(FadeCGAlpha(0f, 1f, 0.5f, mainMenu));
        StartCoroutine(FadeCGAlpha(1f, 0f, 0.5f, screen));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Adapted from code found at https://forum.unity.com/threads/unity-4-6-ui-how-to-fade-a-canvas-group-in-and-then-out-after-a-seconds.299283/
    public static IEnumerator FadeCGAlpha(float from, float to, float duration, CanvasGroup canvasGroup)
    {
        float elaspedTime = 0f;
        while (elaspedTime <= duration)
        {
            elaspedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elaspedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
