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

    [Space]
    [Header("SkillLock")]
    public Image[] Lock = new Image[3];

    [Header("HUD")]
    public TextMeshProUGUI hpText;
    public Slider hpBar;
    public Slider mpBar;
    public GameObject turretHp;
    
    //Skill Image
    public Image qSkill;
    public Image eSkill;
    public Image rSkill;

    public Sprite[] skillView = new Sprite[3];

    [Space]
    [Header("Setting")]
    public GameObject setting;

    [HideInInspector] public Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    private void Update()
    {
        // Show Skill Image
        if (qSkill.sprite != skillView[0] && skillView[0]) qSkill.sprite = skillView[0];
        if (eSkill.sprite != skillView[1] && skillView[1]) eSkill.sprite = skillView[1];
        if (rSkill.sprite != skillView[2] && skillView[2]) rSkill.sprite = skillView[2];
        
        // Show Option
        if (Input.GetKeyDown(KeyCode.Escape) && setting && !setting.activeSelf)
        {
            setting.SetActive(true);
        }
    }
}
