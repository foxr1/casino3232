using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGoal : MonoBehaviour
{
    private MazeLogic maze;

    private void Start()
    {
        maze = transform.parent.GetComponent<MazeLogic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        string[] collidedName = other.name.Split(new char[] { ' ' });
        if (collidedName.Length > 1) {
            if (collidedName[1] == "Mouse(Clone)")
            {
                other.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(maze.GameWon(collidedName[0].ToString()));
            }
        }
    }
}
