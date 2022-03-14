using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Adapted from tutorial found at https://www.youtube.com/watch?v=ya1HyptE5uc
public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 0.15f;

    [SerializeField]
    private Transform wallPrefab;
    [SerializeField]
    private Transform floorPrefab;
    [SerializeField]
    private Transform goalPrefab;
    [SerializeField]
    private Transform orangeMousePrefab, blueMousePrefab;

    private Transform orangeMouse, blueMouse;

    // UI Elements
    [SerializeField]
    private GameObject messageBox;
    [SerializeField]
    private GameObject mazeSizeText;
    [SerializeField]
    private GameObject betMultiplierText;
    [SerializeField]
    private GameObject sizeSlider;

    public bool buildingMaze = false;
    public float betMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        CreateMaze();
    }

    public void CreateMaze()
    {
        buildingMaze = true;

        WallState[,] maze = MazeGenerator.Generate(width, height);
        StartCoroutine(Draw(maze));
    }

    private IEnumerator Draw(WallState[,] maze)
    {
        Transform floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                WallState cell = maze[i, j];
                Vector3 position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if (cell.HasFlag(WallState.UP))
                {
                    Transform topWall = Instantiate(wallPrefab, transform);
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    Transform leftWall = Instantiate(wallPrefab, transform);
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        Transform rightWall = Instantiate(wallPrefab, transform);
                        rightWall.position = position + new Vector3(size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        Transform bottomWall = Instantiate(wallPrefab, transform);
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }

            yield return null;
        }

        

        orangeMouse = Instantiate(orangeMousePrefab, transform);
        blueMouse = Instantiate(blueMousePrefab, transform);

        if (width % 2 == 0) // If even width
        {
            orangeMouse.transform.position = new Vector3(-(width / 2), 0.3f, (width / 2) - 1); // Top left
            blueMouse.transform.position = new Vector3((width / 2) - 1, 0.3f, -(width / 2)); // Bottom left
        }
        else
        {
            orangeMouse.transform.position = new Vector3(-(width / 2), 0.3f, (width / 2)); // Top left
            blueMouse.transform.position = new Vector3((width / 2), 0.3f, -(width / 2)); // Bottom left
        }

        BuildNavMesh();
        Transform goal = Instantiate(goalPrefab, transform);

        orangeMouse.GetComponent<NavMeshAgent>().enabled = true;
        blueMouse.GetComponent<NavMeshAgent>().enabled = true;

        buildingMaze = false;
    }

    private void BuildNavMesh()
    {
        NavMeshSurface[] navMeshSurfaces = GetComponentsInChildren<NavMeshSurface>();

        foreach (NavMeshSurface navMeshSurface in navMeshSurfaces)
        {
            navMeshSurface.BuildNavMesh();
        }
    }

    public void ChangeMazeSize(float size)
    {
        if (!buildingMaze)
        {
            width = (int)size;
            height = (int)size;
            Camera.main.transform.position = new Vector3(0, size, 0);
            mazeSizeText.GetComponent<TextMeshProUGUI>().text = "MAZE SIZE: <color=grey>" + size.ToString() + "</color>";

            betMultiplier = 1.5f * (size / 15);
            betMultiplierText.GetComponent<TextMeshProUGUI>().text = "BET MULTIPLIER: <color=yellow>x" + Math.Round(betMultiplier, 2) + "</color>";
        }
        else
        {
            StartCoroutine(CardMechanics.ShowMessage(messageBox, "Please wait for maze to finish building."));
            sizeSlider.GetComponent<Slider>().value = width;
        }
    }

    public void StartGame()
    {
        orangeMouse.GetComponent<MouseMove>().SetDestination();
        blueMouse.GetComponent<MouseMove>().SetDestination();
    }
}
