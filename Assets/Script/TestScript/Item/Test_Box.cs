using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test_Box : MonoBehaviour, IItem
{
    public UnityEvent OpenBox;
    bool end = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Work");
            if (Input.GetKeyDown(KeyCode.F) && end == false)
            {
                end = true;
                Use();
            }
        }
         
    }
    public void Use()
    {
        Time.timeScale = 0f;
        OpenBox.Invoke();
    }

    public void UseEnd()
    {
        Time.timeScale = 1f;
    }
}
