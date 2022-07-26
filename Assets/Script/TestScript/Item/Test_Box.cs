using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test_Box : MonoBehaviour, IItem
{
    public UnityEvent OpenBox;
    bool end = false;

    GameObject skillWindow;
    GameObject blackScreen;
    private void Update()
    {
        if(end == true && skillWindow && skillWindow.activeSelf == false)
        {
            FindObjectOfType<PlayerMovement>().enabled = true;
            FindObjectOfType<CameraManager>().enabled = true;
            UseEnd();
        }
    }
    public void Use()
    {
        Time.timeScale = 0f;
        OpenBox.Invoke();
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        skillWindow = GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade").gameObject;
        skillWindow.SetActive(true);
        blackScreen = GameObject.Find("InGame_UI_sample").transform.Find("BlackOut").gameObject;
        blackScreen.SetActive(true);

        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<PlayerAttack>().enabled = false;
        FindObjectOfType<CameraManager>().enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        end = true;
    }

    public void UseEnd()
    {
        Time.timeScale = 1f;
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        if (StructureSpawn_Test.StructureMode == false)
        FindObjectOfType<PlayerAttack>().enabled = true;

        Destroy(gameObject);
    }
}
