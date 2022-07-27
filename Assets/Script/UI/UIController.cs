using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Space]
    [Header("PlayerAim")]
    public Image aim;

    [Header("HUD")]
    public TextMeshProUGUI hpText;
    public Slider hpBar;
    public Slider mpBar;
    public GameObject turretHp;

    [Space]
    [Header("Setting")]
    public GameObject setting;

    private void Update()
    {
        // Show Option
        if (Input.GetKeyDown(KeyCode.Escape) && setting && !setting.activeSelf)
        {
            setting.SetActive(true);
        }
    }
}
