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

    PauseGame pause;

    UIController ui;
    private void Start()
    {
        pause = PauseGame.instance;
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
            pause.blackScreen.SetActive(true);
            pause.StopGame();
            pause.skillWindow.SetActive(true);
        }

        end = true;
    }

    public void UseEnd() // End use Test Box.
    {
        
        if (pause)
        {
            pause.skillWindow.SetActive(true);
            pause.PlayGame();
            pause.blackScreen.SetActive(false);
        }
        Destroy(gameObject);
    }
}
