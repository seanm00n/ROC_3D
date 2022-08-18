using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private int sceneNum = 0;
    private Animator animator;

    public bool isMainMenuOrGameOver = false;
    [Header("BlockScreen")]
    public Image blockImage;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartButton(int scene)
    {
        if (isMainMenuOrGameOver == true)
        {
            PlayerSaveData.turretAmount = 0;
            PlayerSaveData.gold = 200;
            if (PlayerSaveData.itemList.Count == 4)
            {
                PlayerSaveData.itemList[0] = "1";
                PlayerSaveData.itemList[1] = "1";
                PlayerSaveData.itemList[2] = "1";
                PlayerSaveData.itemList[3] = "1";
            }
            PlayerSaveData.goldLock = true;

            PlayerAttackSkill.qSkillData = new SkillData();
            PlayerAttackSkill.qSkillData.thisSkill = PlayerAttackSkill.skill.None;
            
            PlayerAttackSkill.eSkillData = new SkillData();
            PlayerAttackSkill.eSkillData.thisSkill = PlayerAttackSkill.skill.None;
           
            PlayerAttackSkill.rSkillData = new SkillData();
            PlayerAttackSkill.rSkillData.thisSkill = PlayerAttackSkill.skill.None;
           
            PlayerAttackSkill.passiveSkillData = new SkillData();
            PlayerAttackSkill.passiveSkillData.thisSkill = PlayerAttackSkill.skill.None;
          
            PlayerAttackSkill.normalAttackMp = 3;
            PlayerAttackSkill.normalDamage = 20;
            PlayerAttackSkill.fireRate = 0.14f;

            Player.theNumberOfDeaths = 0;

            for (int i = 0; i < 4; i++)
            {
                SkillUpgrade.skillGrade[i] = 0;
                SkillUpgrade.skillComplete[i] = false;
            }

        }
        if (blockImage)
        StartCoroutine(FadeOut(scene));
        else
        {
            if (sceneNum == 0) SceneManager.LoadScene(scene, LoadSceneMode.Single);
            else
                SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
        }
    }
    public void LoadButton()
    {
        animator.SetTrigger("save");
        
    }

    public void LoadBoxExitButton()
    {
        animator.SetTrigger("exit");
    }



    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator FadeOut(int scene) // Screen FadeOut
    {
        Time.timeScale = 1;
        blockImage.gameObject.SetActive(true);
        Color c = new Color();
        
        for (; c.a < 1;) 
        {
            c.a += 0.2f;
            blockImage.color = c;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(0.1f);
        if(sceneNum == 0) SceneManager.LoadScene(scene, LoadSceneMode.Single);
        else
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
    }
}
