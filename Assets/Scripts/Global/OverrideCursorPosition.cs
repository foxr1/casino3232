using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Adapted from code found at https://forum.unity.com/threads/fake-mouse-position-in-4-6-ui-answered.283748/?_ga=2.172224739.1977630401.1639093888-1896384234.1635271023
// This is exists so that the player can interact with UI elements in the world space, like the slider on the table in the lobby room and buttons in the shop
public class OverrideCursorPosition : StandaloneInputModule
{
    private bool paused = false;

    protected override MouseState GetMousePointerEventData(int id)
    {
        var lockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        var mouseState = base.GetMousePointerEventData(id);
        Cursor.lockState = lockState;
        CheckIfPaused();
        return mouseState;
    }

    protected override void ProcessMove(PointerEventData pointerEvent)
    {
        var lockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        base.ProcessMove(pointerEvent);
        Cursor.lockState = lockState;
        CheckIfPaused();
    }

    protected override void ProcessDrag(PointerEventData pointerEvent)
    {
        var lockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        base.ProcessDrag(pointerEvent);
        Cursor.lockState = lockState;
        CheckIfPaused();
    }

    private void CheckIfPaused()
    {
        if (GameObject.Find("Player") != null)
        {
            paused = GameObject.Find("Player").GetComponent<PlayerController>().GetPaused();
        }

        if (!paused)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }
}
