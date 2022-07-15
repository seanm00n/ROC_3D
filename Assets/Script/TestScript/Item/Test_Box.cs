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
            FindObjectOfType<Camera_manager>().enabled = true;
            UseEnd();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Work");
            if (Input.GetKeyDown(KeyCode.F) && end == false)
            {
                Use();
            }
        }
         
    }
    public void Use()
    {
        Time.timeScale = 0f;
        OpenBox.Invoke();
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        skillWindow = GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade").gameObject;
        skillWindow.SetActive(true);
        blackScreen = GameObject.Find("InGame_UI_sample").transform.Find("BlackOut").gameObject; ;
        blackScreen.SetActive(true);

        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<Camera_manager>().enabled = false;

        end = true;
    }

    public void UseEnd()
    {
        Time.timeScale = 1f;
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        Destroy(this);
    }
}
