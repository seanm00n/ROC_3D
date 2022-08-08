using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestBox : MonoBehaviour, IItem
{
    // SkillType => 0 : normal 1: passive 
    [Header("SkillType")]
    public int skillType = 0;

    [Space]
    [Header("Event")]
    public UnityEvent OpenBox;

    //End use.
    private bool end = false;

    PauseGame pause;

    [Header("Effect")]
    public Canvas uiEffect;
    
    private void Start()
    {
        pause = PauseGame.instance;
        if (uiEffect) uiEffect.worldCamera = Camera.main;
    }
    private void Update()
    {
        if (!pause) return;
        if(end == true && pause.skillWindow && pause.skillWindow.activeSelf == false)
        {
            Player.instance.movement.enabled = true;
            FindObjectOfType<CameraManager>().enabled = true;
            UseEnd();
        }
    }
    public void Use() // Use Test Box
    {
        OpenBox.Invoke(); // Start Event.
        if (pause)
        {
            SkillUpgrade.skillType = skillType;
            pause.blackScreen.SetActive(true);
            pause.StopGame();
            pause.skillWindow.SetActive(true);
            Player.instance.ui.canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Player.instance.ui.canvas.worldCamera = Camera.main;
        }

        end = true;
    }

    public void UseEnd() // End use Test Box.
    {
        
        if (pause)
        {
            pause.skillWindow.SetActive(false);
            pause.PlayGame();
            pause.blackScreen.SetActive(false);
            Player.instance.ui.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        Destroy(gameObject);
    }
}
