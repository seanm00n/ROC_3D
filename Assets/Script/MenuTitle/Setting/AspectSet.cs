using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AspectSet : MonoBehaviour
{
    [Space]
    [Header("CurrentGameScene")]
    public bool menu = false;

    [Space]
    [Header("UI")]
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenBtn;

    [Space]
    [Header("Player And Camera")] // Player + Camera Information.
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public CameraManager cameraManager;

    // Find
    private GameObject skillWindow;
    private FullScreenMode screenMode;

    readonly List<Resolution> resolutions = new();

    void Start()
    {
        InitUI();

        if (GameObject.Find("InGame_UI_sample") != null && GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade") != null)
        {
            skillWindow = GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade").gameObject;
        }

        playerMovement = Player.instance.movement;
        playerAttack = Player.instance.playerAttack;
        cameraManager = FindObjectOfType<CameraManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Instantiate();//return to menu prefab
            gameObject.SetActive(false);
        }
    }

    void InitUI()
    {
        foreach (var resolution in Screen.resolutions)
        {
            //if (resolution.refreshRate == 60) // 항상 60hz만 서포트?
            resolutions.Add(resolution);
        }

        resolutionsDropdown.options.Clear();

        // Add all available resolutions
        resolutionsDropdown.AddOptions(resolutions.Select(res => res.width + "x" + res.height).ToList());

        // Update the dropdown appearance
        resolutionsDropdown.RefreshShownValue();
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
    }

    // Used in inspector
    public void ApplyButton()
    {
        Screen.SetResolution(resolutions[resolutionsDropdown.value].width,
        resolutions[resolutionsDropdown.value].height,
        screenMode);
    }

    // Used in inspector
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    //Used in inspector
    public void ExitAspectSet()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable() // Stop Game Play
    {

        Time.timeScale = 0f; // Stop Time.
        if (!menu)
        {
            Player.instance.movement.enabled = false;
            Player.instance.playerAttack.enabled = false;
            FindObjectOfType<CameraManager>().enabled = false;
        }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
    }


    private void OnDisable() // Start Game Play again.
    {
        if (skillWindow)
        {
            if (skillWindow.activeSelf == false)
            {
                if(playerMovement)
                    playerMovement.enabled = true;

                if (playerAttack && StructureSpawn_Test.structureMode == false)
                    playerAttack.enabled = true;
                    
                if (cameraManager)
                    cameraManager.enabled = true;

                    Time.timeScale = 1f; // Time goes by.
            }
        }
        if (menu)
        {
            Time.timeScale = 1f; // Time goes by.
        }
    }
}