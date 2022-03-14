using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRoom : MonoBehaviour
{
    public string scene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (scene == "MainMenu")
            {
                Cursor.lockState = CursorLockMode.None; // Unlock cursor from First Person Controller in order to click menu items
            }

            SceneManager.LoadScene(scene);
        }
    }
}
