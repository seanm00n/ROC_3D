using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestBox : MonoBehaviour, IItem
{
    [Header("Event")]
    public UnityEvent OpenBox;
    
    //End use.
    bool end = false;

    // Used UI
    GameObject skillWindow;
    GameObject blackScreen;

    private void Update()
    {
        if(end == true && skillWindow && skillWindow.activeSelf == false)
        {
            Player.instance.movement.enabled = true;
            FindObjectOfType<CameraManager>().enabled = true;
            UseEnd();
        }
    }
    public void Use() // Use Test Box
    {
        Time.timeScale = 0f; // Stop time.
        OpenBox.Invoke(); // Start Event.

        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        
        skillWindow = GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade").gameObject;
        skillWindow.SetActive(true);
        blackScreen = GameObject.Find("InGame_UI_sample").transform.Find("BlackOut").gameObject;
        blackScreen.SetActive(true);

        Player.instance.movement.enabled = false;
        Player.instance.playerAttack.enabled = false;
        FindObjectOfType<CameraManager>().enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        end = true;
    }

    public void UseEnd() // End use Test Box.
    {
        Time.timeScale = 1f; // Time goes by. 

        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        if (StructureSpawn_Test.structureMode == false)
        Player.instance.playerAttack.enabled = true;

        Destroy(gameObject);
    }
}
