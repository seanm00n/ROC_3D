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
    [Header("Hide In Shop")]
    public GameObject[] DeleteUI;

    [Space]
    [Header("Skill Lock")]
    public Image[] sLock = new Image[3];
    private float declinedTime;
    private float limitTime;

    [Header("HUD")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public Slider hpBar;
    public Slider mpBar;
    public GameObject turretHp;
    public TextMeshProUGUI turretAmount;
    
    [Space]
    [Header("Hide GameObject")]
    public GameObject[] Hide;

    [Space]
    [Header("Skill View")]
    //Skill Image
    public Image qSkill;
    public Image eSkill;
    public Image rSkill;
    public Image passiveSkill;

    public Sprite[] skillView = new Sprite[3];

    [Space]
    [Header("Setting")]
    public GameObject setting;

    [HideInInspector] public Canvas canvas;

    [Space]
    [Header("Shop")]
    public GameObject shopUI;
    public GameObject monsterUI;

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

        if(Player.theNumberOfDeaths != 0)
        {
            sLock[3].gameObject.SetActive(true);
        }

        if (turretAmount) turretAmount.text = "Turret :  " + PlayerSaveData.turretAmount + "/" + PlayerSaveData.turretAmountMax;
    }
    
    public IEnumerator LockskillView(Image skillView, SkillData data)
    {
        if (!(skillView.fillAmount != 0 && limitTime > 0)) yield return 0;
        skillView.fillAmount = 1;
        
        while (skillView.fillAmount != 0)
        {
            yield return new WaitForSeconds(0.1f);
            declinedTime = Player.instance.playerAttackSkill.DeclineTime(data);
            limitTime = data.limitTime - declinedTime;
            if (Time.timeScale != 0)
                skillView.fillAmount -= (1/ limitTime)/10f; 
        }
    }
}
