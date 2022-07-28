using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    [Space]
    [Header("Player And Camera")] // Need to stop game.
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public CameraManager cameraManager;

    [Space]
    [Header("UI")]
    public GameObject skillWindow; // Choose skill view in UI. 
    public GameObject blackScreen; // Backgorund when pause Game.

    //Single Tone
    public static PauseGame instance;

    private void Start()
    {
        if (!instance)
            instance = this;       
    }

    private void Update()
    {
        // window exit.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable() 
    {
        StopGame();
    }

    private void OnDisable()
    {
        if (!skillWindow || !skillWindow.activeSelf) // Playe game again, but this case is that skill window is not open.
            PlayGame();
    }

    public void PlayGame() // Start Gameplay again.
    {
        // Can control again.
        if (playerMovement)
            playerMovement.enabled = true;

        if (playerAttack && StructureSpawn_Test.structureMode == false)
            playerAttack.enabled = true;

        if (cameraManager)
            cameraManager.enabled = true;

        Time.timeScale = 1f; // Unfreeze time
    }
    public void StopGame() // Stop Game Play
    {

        Time.timeScale = 0f; // Freeze time
        if (Player.instance)
        {
            if (!cameraManager) // Stop Control
            {
                playerMovement = Player.instance.movement;
                playerAttack = Player.instance.playerAttack;
                cameraManager = Player.instance.playerCamera.GetComponent<CameraManager>();
            }
            playerMovement.enabled = false;
            playerAttack.enabled = false;
            cameraManager.enabled = false;
        }
        // Stop cursor lock
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}