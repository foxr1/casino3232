using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Adapted code from tutorial found at https://www.youtube.com/watch?v=_QajrabyTJc

    // Controller and Movement
    private CharacterController controller;
    private Animator anim;
    public Vector3 movement;
    private float characterHeight;
    private Vector3 characterCenter;
    private Transform theTransform;
    public bool isCrouching = false;
    public static bool canPickpocket = false; // Used for Warden AI

    // Camera
    private Camera mainCamera;
    private Vector3 cameraPos;

    // Speed values
    public float walkSpeed = 12f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 250f;
    private Vector3 velocity;

    // Ground check (for jumping)
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    // Pause menu
    public GameObject pauseMenu;
    private static bool paused = false;

    // UI elements
    public GameObject coinText;
    public GameObject messageBox;
    public static int coinCount = 50;
    public GameObject keyImg, shoesImg, pickpocketImg;
    public static bool hasKey = false;
    public static bool[] openedDoors = { false, false }; // { rummy, puzzle }
    public static bool undetectPickpocket = false;
    public static bool runningShoes = false;
    public static float volumeSliderValue = 1f;

    void Start() {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        characterHeight = controller.height;
        theTransform = transform;
        characterCenter = controller.center;
        mainCamera = GetComponentInChildren<Camera>();
        cameraPos = mainCamera.transform.localPosition;
        messageBox.GetComponent<Image>().CrossFadeAlpha(0f, 0f, false);
        messageBox.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0f, 0f, false);
    }

    // Input for detecting jump and escape press in Update so no latency between pressing button and execution in game
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;

            // Increased animation speed if player is sprinting
            int animValue;
            if (runningShoes && Input.GetKey(KeyCode.LeftShift))
            {
                animValue = 5;
            }
            else
            {
                animValue = 1;
            }

            if (Input.GetKey(KeyCode.W)) {
                anim.SetInteger("Speed", animValue);
            } else {
                anim.SetInteger("Speed", 0);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                anim.SetInteger("Speed", animValue);
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S)) {
                anim.SetInteger("Speed", 0);
            }

            if (Input.GetKey(KeyCode.S)) {
                anim.SetInteger("Speed", -animValue);
            }
        }

        // Jumping
        if (Input.GetButton("Jump") && !isGrounded) {
            anim.SetBool("IsInAir", true);
            StartCoroutine(StopJumping());
        }

        // Using Physics behind gravitational potential energy where a rearrangement of mgh = 1/2(mv^2) gives v = root(2gh), where v: velocity, g: gravity and h: height
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            PauseGame();
        }
    }

    // Jumping, movement and crouching is all in FixedUpdate to avoid different outcomes on different machines
    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float height = characterHeight;
        Vector3 center = characterCenter;
        Vector3 cameraPosition = cameraPos;
        float speed = walkSpeed;
        isCrouching = false;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetInteger("Speed", -2);
            controller.height = 9f;
            controller.center = new Vector3(center.x, 3.4f, center.z);
            mainCamera.transform.localPosition = new Vector3(cameraPosition.x, 8f, cameraPosition.z);
            speed = crouchSpeed;
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetInteger("Speed", 0);
            controller.center = characterCenter;
        }

        float lastHeight = controller.height;
        controller.height = Mathf.Lerp(controller.height, height, 5 * Time.deltaTime);
        controller.center = Vector3.Lerp(controller.center, center, 5 * Time.deltaTime);
        mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, cameraPosition, 5 * Time.deltaTime);
        Vector3 pos = theTransform.position;
        if (pos.y > controller.height)
        {
            pos.y = (controller.height - lastHeight) / 2;
            theTransform.position = pos;
        }

        movement = transform.right * x + transform.forward * z;

        if (runningShoes && Input.GetKey(KeyCode.LeftShift))
        {
            speed *= 1.5f;
        }

        // Using SUVAT equations s = ut + 1/2(at^2) where u = 0 and a is gravity gives the equation h = 1/2(gt^2)
        controller.Move(movement * speed * Time.deltaTime);
        velocity.y += gravity + Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public IEnumerator ShowMessage(string text)
    {
        messageBox.GetComponentInChildren<TextMeshProUGUI>().text = text;
        messageBox.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(1f, 0.5f, false);
        messageBox.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.5f, false);

        yield return new WaitForSeconds(2f);

        messageBox.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
        messageBox.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0f, 0.5f, false);
    }

    IEnumerator StopJumping() {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("IsInAir", false);
    }

    private void PauseGame()
    {
        pauseMenu.GetComponent<PauseMenu>().CheckScene();

        if (paused)
        {
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Getters and setters for various player attributes
    public int GetCoinCount()
    {
        return int.Parse(coinText.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void SetCoinCount(int newCoinCount)
    {
        coinText.GetComponentInChildren<TextMeshProUGUI>().text = newCoinCount.ToString();
        coinCount = newCoinCount;
    }

    public bool GetKey()
    {
        return hasKey;
    }

    public void SetKey(bool option)
    {
        hasKey = option;
        keyImg.SetActive(option);
    }

    public bool GetRunningShoes()
    {
        return runningShoes;
    }

    public void SetRunningShoes(bool option)
    {
        runningShoes = option;
        shoesImg.SetActive(option);
    }

    public bool GetUndetectPickpocket()
    {
        return undetectPickpocket;
    }

    public void SetUndetectPickpocket(bool option)
    {
        undetectPickpocket = option;
        pickpocketImg.SetActive(option);
    }

    public bool GetPaused()
    {
        return paused;
    }

    public void SetVolumeSliderValue(float value)
    {
        volumeSliderValue = value;
    }

    public float GetVolumeSliderValue()
    {
        return volumeSliderValue;
    }
}
