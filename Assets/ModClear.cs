using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModClear : MonoBehaviour
{
    public GameObject clearUI;

    private void OnTriggerStay(Collider other)
    {
        if (!clearUI.activeSelf && other.CompareTag("Player")) ClearGame();        
    }

    public void ClearGame()
    {
        clearUI.SetActive(true);
        Player.instance.gameObject.SetActive(false);
        Camera.main.gameObject.SetActive(false);
        Player.instance.ui.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
