using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ROC;
public class SkillUpgrade : MonoBehaviour
{
    // Skill 1th 2th 3th.
    private PlayerAttackSkill.skill[] skill = new PlayerAttackSkill.skill[3];

    // Number about seleted skill.
    private int skillNumber;

    // Seleted skill by player.
    private int currentSkillGrade;
    private int currentSkillView;
    private int currentSkill
    {
        get
        {
            return currentSkillView;
        }
        set 
        {
            currentSkillView = value;

            seletedBar[currentSkillView - 1].SetActive(true);
            for(int i = 0; i< seletedBar.Length; i++)
            {
                if(i != currentSkillView - 1)
                {
                    seletedBar[i].SetActive(false);
                }
            }
        }
    } 


    // Skill information
    private static int[] skillGrade = new int[4]; // Player skill grade [ 0 : Q skill, 1: E skill, 2 : R skill] + 3 is passive skill
    private static bool[] skillComplete = new bool[4]; // Player skill grade [ 0 : Q skill, 1: E skill, 2 : R skill] + 3 is passive skill

    private string[] skillDescriptions = new string[3];
    private string[] skillValues = new string[3];
    private string[] skillValuesEarly = new string[3];

    // BoxType
    public static int skillType = 0;

    // Sprite
    private Sprite selectedSkillView;

    [Header("Skill View And Description")]
    [Space]
    public Sprite[] skillView;
    
    // UI
    public GameObject[] seletedBar;
    public Image[] subjectOfApplicationSkillView;
    
    public Text skillDescription;
    public Text skillValue;
    public Text skillValueEarly;
    public TextMeshProUGUI title;

    // Skill information
    public SkillData[] savedSkillData = new SkillData[3];


    /// Function /////////////////////////////////////////////////

    private void OnEnable() // Random Skill Set
    {
        for (int i = 0; i< skillValuesEarly.Length; i++)
            skillValuesEarly[i] = "";

        savedSkillData[0] = new SkillData();
        savedSkillData[1] = new SkillData();
        savedSkillData[2] = new SkillData();

        if (Player.instance.ui.Hide.Length != 0)
        {
            for (int j = 0; j < Player.instance.ui.Hide.Length; j++)
            {
                if (Player.instance.ui.Hide[j])
                    Player.instance.ui.Hide[j].SetActive(true);
            }
        }
        #region
        if (skillType == 0)
        {
            for (int i = 0; i < skillComplete.Length - 1; i++)
            {
                if (!skillComplete[i]) break;
                if (i == skillComplete.Length - 2) skillType = 2;
            }
        }
        else if(skillType == 1)
        {
            InformationOfSkill.Information(10);
            if (skillComplete[3])
            {
                skillType = 2;
            }
        }
        #endregion
        if (skillType == 0) 
        {
            if (Player.instance.playerAttackSkill.rSkill == PlayerAttackSkill.skill.None)
                ResetSkill();
            else
                ResetFinishedSkill(); 
        }
        else if(skillType == 1)
        {
            if (Player.instance.playerAttackSkill.passiveSkill == PlayerAttackSkill.skill.None)
                ResetSkill();
            else
                ResetFinishedSkill();
        }
        else if(skillType == 2)
        {
            // Skeleton
            title.text = "Choose item you need!";
            ResetItem();
            return;
        }
        if (PlayerAttackSkill.qSkillData == null && PlayerAttackSkill.passiveSkillData == null)
        {
            for (int i = 0; i < skillGrade.Length; i++) 
                    skillGrade[i] = 0;
            for (int i = 0; i < skillComplete.Length; i++)
                    skillComplete[i] = false; 
        }

        if (PlayerAttackSkill.rSkillData == null)
        {
            title.text = "Choose Skill you need certainly!";
        }
        else
        {
            title.text = "Choose Skill you need to upgrade!";
        }
    }

    private void Update()
    {
        if (currentSkill != 0)
        {
            skillDescription.text = skillDescriptions[currentSkillView - 1];
            if (skillType != 2)
            {
                skillValue.text = skillValues[currentSkillView - 1];
                skillValueEarly.text = skillValuesEarly[currentSkillView - 1];
            }
            else
            {
                skillValue.text = "";
                skillValueEarly.text = "";
            }
        }
    }

    // Check skill level
    private int skillGradeCheck(int grade, int value02, int value03)
    {
        switch (grade)
        {
            case 1 :
                return value02;
            case 2 :
                return value03;
            default :
                return 0;
        }
    }

    // Skill is reset randomly.
    public void ResetSkill()
    {
        int comparativeValueFirst = 0;

        for (int i = 0; i < 3; i++)
        {
            int comparativeValue = 0; // When skill view already have skill.
            bool retry = false; // When player already have skill.

            do
            {
                retry = false;
                comparativeValue = (int)Random.Range(1, 10);

                // SkillType => 0 : normal 1: passive 
                if (skillType == 0)
                {
                    if (skillView[comparativeValue - 1] == Player.instance.ui.qSkill.sprite)
                        retry = true;
                    else if (skillView[comparativeValue - 1] == Player.instance.ui.eSkill.sprite)
                        retry = true;
                    else if (skillView[comparativeValue - 1] == Player.instance.ui.rSkill.sprite)
                        retry = true;

                    if ((comparativeValue == 1) || (comparativeValue == 6) || (comparativeValue == 8)) retry = true;
                }

                else if(skillType == 1)
                {
                    retry = true;
                    if ((comparativeValue == 1) || (comparativeValue == 6) || (comparativeValue == 8)) retry = false;
                }
            }
            while (((skillNumber == comparativeValue) || (comparativeValue == comparativeValueFirst)) || retry == true);

            skillNumber = comparativeValue;

            if (comparativeValueFirst == 0)
                comparativeValueFirst = comparativeValue;

            selectedSkill(skillNumber, i);
            subjectOfApplicationSkillView[i].sprite = selectedSkillView;
        }
        currentSkill = 1;
    }

    public void ResetFinishedSkill()
    {
        int comparativeValueFirst = 0;

        for (int i = 0; i < 3; i++)
        {
            int comparativeValue; // When skill view already have skill.
            bool retry = false; // When player already have skill.

            do
            {
                retry = true;
                comparativeValue = (int)Random.Range(1, 11);
                if (skillType == 0)
                {
                    if (skillView[comparativeValue - 1] == Player.instance.ui.qSkill.sprite ||
                        skillView[comparativeValue - 1] == Player.instance.ui.eSkill.sprite ||
                        skillView[comparativeValue - 1] == Player.instance.ui.rSkill.sprite ||
                        (comparativeValue) == 10)
                        retry = false;

                    if (skillView[comparativeValue - 1] == skillView[0])
                        retry = true;
                }
                else if(skillType == 1)
                {
                    if (Player.instance.ui.Hide.Length != 0)
                    {
                        for (int j = 0; j < Player.instance.ui.Hide.Length; j++)
                        {
                            if (Player.instance.ui.Hide[j])
                                Player.instance.ui.Hide[j].SetActive(false);
                        }
                    }
                    if (PlayerAttackSkill.passiveSkillData.thisSkill == InformationOfSkill.SkillName(comparativeValue))
                        retry = false;
                }
            }
            while ((skillType == 0 && ((skillNumber == comparativeValue) || (comparativeValue == comparativeValueFirst))) || retry == true);
            skillNumber = comparativeValue;

            if (skillType == 0 && comparativeValueFirst == 0)
                comparativeValueFirst = comparativeValue;

            selectedSkill(skillNumber, i);
            subjectOfApplicationSkillView[i].sprite = selectedSkillView;

            currentSkill = 1;
        }
    }

    public void ResetItem()
    {
        int comparativeValueFirst = 0;

        for (int i = 0; i < 3; i++)
        {
            int comparativeValue = 0; // When skill view already have skill.

            do
            {
                comparativeValue = (int)Random.Range(11, 14);
            }
            while (((skillNumber == comparativeValue) || (comparativeValue == comparativeValueFirst)));

            skillNumber = comparativeValue;

            if (comparativeValueFirst == 0)
                comparativeValueFirst = comparativeValue;

            selectedSkill(skillNumber, i);
            subjectOfApplicationSkillView[i].sprite = skillView[skillNumber - 1];
        }
        currentSkill = 1;
    }

    // Used in inspector
    public void ChangeSkillContent(int number)
    {
        currentSkill = number;
    }

    // Used in inspector
    public void AddSkill(int skillNumber)
    {
        if (skillType == 2)
        {
            PlayerSaveData data;
            try
            {
                data = SaveManager.Load<PlayerSaveData>("PlayerData");
            }
            catch
            {
                data = new PlayerSaveData();
            }
            if (subjectOfApplicationSkillView[skillNumber].sprite == this.skillView[10])
            {
                PlayerSaveData.gold += 30;
            }
            if (subjectOfApplicationSkillView[skillNumber].sprite == this.skillView[11])
            {
                data.bone += 5;
            }
            if (subjectOfApplicationSkillView[skillNumber].sprite == this.skillView[12])
            {
                Player.instance.hp += 30;
            }

            SaveManager.Save("PlayerData", data);
            return;
        }

        int skillGradeNumber = SkillButton(skill[skillNumber]);
        Sprite skillView = subjectOfApplicationSkillView[skillNumber].sprite;      

      if (skill[skillNumber] == PlayerAttackSkill.skill.None)
      {
            PlayerAttackSkill.normalDamage = savedSkillData[skillNumber].damage;
            PlayerAttackSkill.normalAttackMp = savedSkillData[skillNumber].usedMp;
            PlayerAttackSkill.fireRate = savedSkillData[skillNumber].AttackCycle;
      }
      else
      {
        if (skillType == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Player.instance.ui.skillView[i] == null)
                {
                    Player.instance.ui.skillView[i] = skillView;
                    switch (i)
                    {
                        case 0:
                            Player.instance.playerAttackSkill.qSkill = skill[skillNumber];
                            PlayerAttackSkill.qSkillData = savedSkillData[skillNumber];
                            break;
                        case 1:
                            Player.instance.playerAttackSkill.eSkill = skill[skillNumber];
                            PlayerAttackSkill.eSkillData = savedSkillData[skillNumber];
                            break;
                        case 2:
                            Player.instance.playerAttackSkill.rSkill = skill[skillNumber];
                            PlayerAttackSkill.rSkillData = savedSkillData[skillNumber];
                            break;
                    }
                    currentSkillGrade = i;
                    break;
                }
                }
                InsertData(currentSkillGrade, skillNumber);
                if (skillGradeNumber < 2)
                {
                    skillGrade[currentSkillGrade]++;
                }
                else
                {
                    skillComplete[currentSkillGrade] = true;
                }
        }
        else if (skillType == 1)
        {
            Player.instance.ui.passiveSkill.sprite = skillView;
            Player.instance.ui.passiveSkill.gameObject.SetActive(true);

            if (savedSkillData[skillNumber].thisSkill != PlayerAttackSkill.skill.Angel_2)
                PassiveSkillEffect(savedSkillData[skillNumber].thisSkill);
            else
            {
                skillComplete[3] = true;
            }
            InsertData(3, skillNumber);
            if (Player.instance.playerAttackSkill.passiveSkill == PlayerAttackSkill.skill.None)
            {
                Player.instance.playerAttackSkill.passiveSkill = skill[skillNumber];
                PlayerAttackSkill.passiveSkillData = savedSkillData[skillNumber];
            }

            if (skillGrade[3] < 2)
            {
                skillGrade[3]++;
            }
            else
            {
                skillComplete[3] = true;
            }
        }
      }
    }

    private int SkillButton(PlayerAttackSkill.skill skillQWE) // Show which is button. 
    {
        if (PlayerAttackSkill.qSkillData != null && PlayerAttackSkill.qSkillData.thisSkill == skillQWE)
        { 
            currentSkillGrade = 0;
        }
        else if (PlayerAttackSkill.eSkillData != null && PlayerAttackSkill.eSkillData.thisSkill == skillQWE)
        {
            currentSkillGrade = 1;
        }
        else if (PlayerAttackSkill.rSkillData != null && PlayerAttackSkill.rSkillData.thisSkill == skillQWE)
        {
            currentSkillGrade = 2;
        }
        else if (PlayerAttackSkill.passiveSkillData != null && PlayerAttackSkill.passiveSkillData.thisSkill == skillQWE)
        {
            currentSkillGrade = 3;
        }
        else
        {
            return 0;
        }

        int grade = skillGrade[currentSkillGrade];

        return grade;
    }


    // All Skill
    #region
    private void selectedSkill(int choosedSkill, int skillNth)
    {
        int attack = 0;
        int percentValue = 0;
        int timeValue =0;
        int mp = 0;
        int grade = 0; // 

        //Early
        int attackEarly = 0;
        int percentValueEarly = 0;
        int timeValueEarly = 0;
        int mpEarly = 0;

        switch (choosedSkill)
        {
            case 1:
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.Angel_2, choosedSkill);
                skillValues[skillNth] = "체력을 전부 소모할 시 자동으로 사용된다."; // No value. So set description instead of value.
                SaveSkillData(skillNth, attack, timeValue, mp, Player.maxHp / 2, Player.maxMp / 2, percentValue);
                break;

            case 2: // Attack Skill
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.EnergyStrike_1, choosedSkill);
                break;

            case 3: // Attack Skill
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.Energy_1, choosedSkill);
                break;

            case 4: // Attack Skill
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.FrontExplosion_2, choosedSkill);
                break;

            case 5: // Attack Skill
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.LightRay_1, choosedSkill);
                break;

            case 6:
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.Magic_1, choosedSkill);
                AllSetPercentFucntion(ref skillNth, ref grade, ref percentValue, ref percentValueEarly, "스킬 쿨타임 감소율 : ", "[현재] 스킬 쿨타임 감소율 : ", skillNth, ref timeValue, ref timeValueEarly);
                break;

            case 7:

                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.Nature_1, choosedSkill);
                grade = SkillButton(skill[skillNth]);
                SetSkillPercentValue(ref choosedSkill, grade, ref percentValue, ref percentValueEarly, ref timeValue, ref timeValueEarly, skillNth);
                skillValues[skillNth] = "체력 회복률 : " + percentValue + "%" + "      쿨타임 : " + timeValue;
                if (grade > 0)
                    skillValuesEarly[skillNth] = "[현재] 체력 회복률 : " + percentValueEarly + "%" + "      쿨타임 : " + timeValueEarly;
                SaveSkillData(skillNth, 0, timeValue, 0, 0, 0, percentValue);
                break;

            case 8:
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.Shine_1, choosedSkill);
                AllSetPercentFucntion(ref choosedSkill, ref grade, ref percentValue, ref percentValueEarly, "공격력 증가율 : ", "[현재] 공격력 증가율 : ", skillNth, ref timeValue, ref timeValueEarly);
                break;

            case 9: // Attack Skill
                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.Shine_2, choosedSkill);
                break;

            case 10:

                SetSkillInformation(skillNth, (choosedSkill - 1), PlayerAttackSkill.skill.None, choosedSkill);
                attack = InformationOfSkill.Information(choosedSkill).attack[1];
                mp = InformationOfSkill.Information(choosedSkill).mp[1];
                int cycle = InformationOfSkill.Information(choosedSkill).timeValue[1];

                attackEarly = InformationOfSkill.Information(choosedSkill).attack[0];
                mpEarly = InformationOfSkill.Information(choosedSkill).mp[0];
                int cycleEarly = InformationOfSkill.Information(choosedSkill).timeValue[0];
               
                if (PlayerAttackSkill.normalDamage == InformationOfSkill.Information(choosedSkill).attack[1])
                {
                    SetNormalAttack(choosedSkill,ref attack, ref mp, ref cycle, ref attackEarly,ref mpEarly,ref cycleEarly, 2,1,skillNth);
                }
                else if(PlayerAttackSkill.normalDamage == InformationOfSkill.Information(choosedSkill).attack[2])
                {
                    SetNormalAttack(choosedSkill, ref attack, ref mp, ref cycle, ref attackEarly, ref mpEarly, ref cycleEarly, 2, 2, skillNth);
                }
                if (cycleEarly != cycle)
                    skillValues[skillNth] = "공격력 : " + (attack - attackEarly) + "증가" + "       공격 속도 증가 " + "       소비 마나 : " + (mpEarly - mp) + "감소";
                else
                    skillValues[skillNth] = "강화 완료";
                skillValuesEarly[skillNth] = "[현재] 공격력 : " + attackEarly + "       소비 마나 : " + mpEarly; ;

                SaveSkillData(skillNth, attack, cycle, mp, 0, 0, 0);
                break;

            case 11:
                skillDescriptions[skillNth] = "30 골드 획득";
                return;
            case 12:

                skillDescriptions[skillNth] = "뼈 5개 획득";
                return;
            case 13:

                skillDescriptions[skillNth] = "30만큼 체력 회복";
                return;
        }
        #endregion 
        if(choosedSkill == 2 || choosedSkill == 3 || choosedSkill == 4 || choosedSkill == 5 || choosedSkill == 9) 
            AllSetAttackFucntion(ref choosedSkill, ref grade, ref attack, ref timeValue, ref mp, ref attackEarly, ref timeValueEarly, ref mpEarly, skillNth);
        if (choosedSkill != 10) savedSkillData[skillNth].thisSkill = skill[skillNth];
    }

    private void SaveSkillData(int skillNth, int attack, int timeValue, int mp, int rebirthHp, int rebirthMp, float percentValue)
    {
        savedSkillData[skillNth].damage = attack;
        savedSkillData[skillNth].limitTime = timeValue;
        savedSkillData[skillNth].usedMp = mp;
        savedSkillData[skillNth].rebirthHp = rebirthHp;
        savedSkillData[skillNth].rebirthMp = rebirthMp;
        savedSkillData[skillNth].declinedTime = percentValue / 100;
        savedSkillData[skillNth].heal = (int) ((percentValue /100f) * Player.maxHp);
        savedSkillData[skillNth].addDamage = percentValue / 100;
        savedSkillData[skillNth].AttackCycle = timeValue / 100f;
    }

    private void AllSetAttackFucntion(ref int choosedSkill, ref int grade, ref int attack, ref int timeValue, ref int mp, ref int attackEarly, ref int timeValueEarly, ref int mpEarly, int skillNth)
    {
        grade = SkillButton(skill[skillNth]);
                
        SetSkillAttackValue(ref choosedSkill, grade, ref attack, ref timeValue, ref mp, ref attackEarly, ref timeValueEarly, ref mpEarly, skillNth);
        SetAttackSkillValueDescription(skillNth, grade, attack, timeValue, mp, attackEarly, timeValueEarly, mpEarly);
        SaveSkillData(skillNth, attack, timeValue, mp, 0, 0, 0);
    }

    private void AllSetPercentFucntion(ref int choosedSkill, ref int grade, ref int percentValue, ref int percentValueEarly, string text, string textEarly, int skillNth, ref int timeValue, ref int timeValueEarly)
    {
        grade = SkillButton(skill[skillNth]);

        SetSkillPercentValue(ref choosedSkill, grade, ref percentValue, ref percentValueEarly, ref timeValue, ref timeValueEarly, skillNth);
        SetPercentSkillValueDescription(skillNth, grade, percentValue, percentValueEarly, text, textEarly);
        SaveSkillData(skillNth, 0, 0, 0, 0, 0, percentValue);
    }

    private void SetSkillInformation(int skillNth, int skillViewNum, PlayerAttackSkill.skill skillname, int choosedSkill)
    {
        skill[skillNth] = skillname;
        selectedSkillView = skillView[skillViewNum];
        skillDescriptions[skillNth] = InformationOfSkill.Information(choosedSkill).skillDescription;
    }

    private void SetNormalAttack(int choosedSkill, ref int attack, ref int mp,ref int cycle, ref int attackEarly, ref int mpEarly, ref int cycleEarly, int previous, int next, int skillNth)
    {
        attack = InformationOfSkill.Information(choosedSkill).attack[previous];
        mp = InformationOfSkill.Information(choosedSkill).mp[previous];
        cycle = InformationOfSkill.Information(choosedSkill).timeValue[previous];

        attackEarly = InformationOfSkill.Information(choosedSkill).attack[next];
        mpEarly = InformationOfSkill.Information(choosedSkill).mp[next];
        cycleEarly = InformationOfSkill.Information(choosedSkill).timeValue[next];
    }

    private void SetSkillAttackValue(ref int choosedSkill, int grade, ref int attack, ref int timeValue, ref int mp, ref int attackEarly, ref int timeValueEarly, ref int mpEarly, int skillNth)
    {
        attack = skillGradeCheck(grade, InformationOfSkill.Information(choosedSkill).attack[1], InformationOfSkill.Information(choosedSkill).attack[2]);
        timeValue = skillGradeCheck(grade, InformationOfSkill.Information(choosedSkill).timeValue[1], InformationOfSkill.Information(choosedSkill).timeValue[2]);
        mp = skillGradeCheck(grade, InformationOfSkill.Information(choosedSkill).mp[1], InformationOfSkill.Information(choosedSkill).mp[2]);
        
        if (skillComplete[currentSkillGrade] == true) 
        { 
            attackEarly = InformationOfSkill.Information(choosedSkill).attack[2];
            timeValueEarly = InformationOfSkill.Information(choosedSkill).timeValue[2];
            mpEarly = InformationOfSkill.Information(choosedSkill).mp[2];
        }
        else
        {
            attackEarly = skillGradeCheck(grade - 1, InformationOfSkill.Information(choosedSkill).attack[1], 0);
            timeValueEarly = skillGradeCheck(grade - 1, InformationOfSkill.Information(choosedSkill).timeValue[1], 0);
            mpEarly = skillGradeCheck(grade - 1, InformationOfSkill.Information(choosedSkill).mp[1], 0);
        }
        if (attack == 0) attack = InformationOfSkill.Information(choosedSkill).attack[0];
        if (timeValue == 0) timeValue = InformationOfSkill.Information(choosedSkill).timeValue[0];
        if (mp == 0) mp = InformationOfSkill.Information(choosedSkill).mp[0];

        if (attackEarly == 0) attackEarly = InformationOfSkill.Information(choosedSkill).attack[0];
        if (timeValueEarly == 0) timeValueEarly = InformationOfSkill.Information(choosedSkill).timeValue[0];
        if (mpEarly == 0) mpEarly = InformationOfSkill.Information(choosedSkill).mp[0];
    }
    private void SetSkillPercentValue(ref int choosedSkill, int grade, ref int percentValue, ref int percentValueEarly, ref int timeValue, ref int timeValueEarly, int skillNth)
    {
        percentValue = skillGradeCheck(grade, InformationOfSkill.Information(choosedSkill).percentValue[1], InformationOfSkill.Information(choosedSkill).percentValue[2]);
        timeValue = skillGradeCheck(grade, InformationOfSkill.Information(choosedSkill).timeValue[1], InformationOfSkill.Information(choosedSkill).timeValue[2]);

        if (skillComplete[currentSkillGrade] == true)
        {
            percentValueEarly = InformationOfSkill.Information(choosedSkill).percentValue[2];
            timeValueEarly = InformationOfSkill.Information(choosedSkill).timeValue[2];
        }
        else
        {
            percentValueEarly = skillGradeCheck(grade - 1, InformationOfSkill.Information(choosedSkill).percentValue[1], 0);
            timeValueEarly = skillGradeCheck(grade - 1, InformationOfSkill.Information(choosedSkill).timeValue[1], 0);
        }
        if (percentValue == 0) percentValue = InformationOfSkill.Information(choosedSkill).percentValue[0];
        if (timeValue == 0) timeValue = InformationOfSkill.Information(choosedSkill).timeValue[0];

        if (percentValueEarly == 0) percentValueEarly = InformationOfSkill.Information(choosedSkill).percentValue[0];
        if (timeValueEarly == 0) timeValueEarly = InformationOfSkill.Information(choosedSkill).timeValue[0];
    }

    private void SetAttackSkillValueDescription(int skillNth, int grade, int attack, int timeValue, int mp, int attackEarly, int timeValueEarly, int mpEarly)
    {
        skillValues[skillNth] = "공격력 : " + attack + "       쿨타임 : " + timeValue + "       소비 마나 : " + mp;
        if (grade > 0)
            skillValuesEarly[skillNth] = "[현재] 공격력 : " + attackEarly + "       쿨타임 : " + timeValueEarly + "       소비 마나 : " + mpEarly;
    }

    private void SetPercentSkillValueDescription(int skillNth, int grade, int percentValue, int percentValueEarly, string text, string textEarly)
    {
        skillValues[skillNth] = text + percentValue + "%";
        if (grade > 0)
            skillValuesEarly[skillNth] = textEarly + percentValueEarly + "%";
    }

    private void InsertData(int Data, int skillNumber)
    {
        switch (Data)
        {
            case 0:
                if (PlayerAttackSkill.qSkillData != null)
                {
                    PlayerAttackSkill.qSkillData = savedSkillData[skillNumber];
                }
                break;
            case 1:
                if (PlayerAttackSkill.eSkillData != null)
                {
                    PlayerAttackSkill.eSkillData = savedSkillData[skillNumber];
                }
                break;
            case 2:
                if (PlayerAttackSkill.rSkillData != null)
                {
                    PlayerAttackSkill.rSkillData = savedSkillData[skillNumber];
                }
                break;
            case 3:
                if (PlayerAttackSkill.passiveSkillData != null)
                {
                    PlayerAttackSkill.passiveSkillData = savedSkillData[skillNumber];
                }
                break;
        }
    }
    private void PassiveSkillEffect(PlayerAttackSkill.skill skill)
    {
        Player.instance.playerAttackSkill.PassiveEffect(skill);
    }
}
