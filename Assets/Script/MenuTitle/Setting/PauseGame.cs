using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    [Space]
    [Header("Player And Camera")] // Need to stop game.
    public PlayerMovement playerMovement;
    public PlayerAttackSkill playerAttackSkill;
    public CameraManager cameraManager;

    [Space]
    [Header("UI")]
    public GameObject skillWindow; // Choose skill view in UI. 
    public GameObject blackScreen; // Backgorund when pause Game.

    //Single Tone
    public static PauseGame instance;

    private void Awake()
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
        if (Player.instance)
            Player.instance.unbeatable = false;

        if (Player.instance.hp != 0)
        {
            // Can control again.
            if (playerMovement)
                playerMovement.enabled = true;

            if (playerAttackSkill && StructureSpawn_Test.structureMode == false)
                playerAttackSkill.enabled = true;

            if (cameraManager)
                cameraManager.enabled = true;
        }
        Time.timeScale = 1f; // Unfreeze time
    }
    public void StopGame() // Stop Game Play
    {
        if (Player.instance)
            Player.instance.unbeatable = true;
        Time.timeScale = 0f; // Freeze time

        if (Player.instance.hp != 0)
        {
            if (Player.instance)
            {
                if (!cameraManager) // Stop Control
                {
                    playerMovement = Player.instance.movement;
                    playerAttackSkill = Player.instance.playerAttackSkill;
                    cameraManager = Player.instance.playerCamera.GetComponent<CameraManager>();
                }
                playerMovement.enabled = false;
                playerAttackSkill.enabled = false;
                cameraManager.enabled = false;
            }
            // Stop cursor lock
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}