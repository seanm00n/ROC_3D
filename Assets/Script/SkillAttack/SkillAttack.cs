using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    public PlayerAttackSkill.skill skillName = PlayerAttackSkill.skill.None;

    [Header("Damage")]
    public static int petDamage;
    public int skillDamageValue;
    public float sustainmentTime = 0;
    public float declinedDamageValue;
    [Space]
    [Header("Master Setting")]
    public bool turretAttack;
    public bool petAttack;

    [Space]
    [Header("MonsterLayer")]
    private int[] monsterLayerNumber = new int[2]; 

    [Space]
    [Header("Effect")]
    public GameObject explosion;

    private Rigidbody skillRigidbody;
    private Collider hitbox;

    private void Start()
    {
        if (petAttack) skillDamageValue = petDamage;
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource)
            audioSource.PlayOneShot(audioSource.clip);
        //monsterLayer set
        monsterLayerNumber[0] = 6;
        monsterLayerNumber[1] = 11;

        if (turretAttack)
        {
            skillRigidbody = GetComponent<Rigidbody>();
            skillRigidbody.AddForce(transform.forward * 500f);
            Destroy(gameObject, 2f);
        }
        if (petAttack)
        {
            skillRigidbody = GetComponent<Rigidbody>();
            skillRigidbody.AddForce(transform.forward * 500f);
            Destroy(gameObject, 4f);
        }
        hitbox = GetComponent<Collider>();

        if (sustainmentTime != 0)
        {
            StartCoroutine(SustainmentTimeOfSkill());
        }
    }

    public void Update()
    {
        if (skillDamageValue != 0) return;
        int addDamage = 0;
        if (Player.instance.playerAttackSkill.qSkill == skillName)
        {
            if (PlayerAttackSkill.passiveSkillData != null && PlayerAttackSkill.passiveSkillData.addDamage != 0)
            {
                addDamage = (int)(PlayerAttackSkill.qSkillData.damage * PlayerAttackSkill.passiveSkillData.addDamage);
                addDamage = (int)(addDamage * Random.Range(0.8f, 1.0f));
                skillDamageValue = PlayerAttackSkill.qSkillData.damage + addDamage;
            }
            else
            {
                skillDamageValue = (int)(PlayerAttackSkill.qSkillData.damage * Random.Range(0.8f, 1.0f));
            }
        }
        else if (Player.instance.playerAttackSkill.eSkill == skillName)
        {
            if (PlayerAttackSkill.passiveSkillData != null && PlayerAttackSkill.passiveSkillData.addDamage != 0)
            {
                addDamage = (int)(PlayerAttackSkill.eSkillData.damage * PlayerAttackSkill.passiveSkillData.addDamage);
                addDamage = (int)(addDamage * Random.Range(0.8f, 1.0f));
                skillDamageValue = PlayerAttackSkill.eSkillData.damage + addDamage;
            }
            else
            {
                skillDamageValue = (int)(PlayerAttackSkill.eSkillData.damage * Random.Range(0.8f, 1.0f));
            }
        }
        else if (Player.instance.playerAttackSkill.rSkill == skillName)
        {
            if (PlayerAttackSkill.passiveSkillData != null && PlayerAttackSkill.passiveSkillData.addDamage != 0)
            {
                addDamage = (int)(PlayerAttackSkill.rSkillData.damage * PlayerAttackSkill.passiveSkillData.addDamage);
                addDamage = (int)(addDamage * Random.Range(0.8f, 1.0f));
                skillDamageValue = PlayerAttackSkill.rSkillData.damage + addDamage;
            }
            else
            {
                skillDamageValue = (int)(PlayerAttackSkill.rSkillData.damage * Random.Range(0.8f, 1.0f));
            }
        }
        if (skillName == PlayerAttackSkill.skill.LightRay_1)
        {
            StartCoroutine(delaySkillDamage(5));   
        }
        if (declinedDamageValue != 0f) skillDamageValue = (int)(skillDamageValue * declinedDamageValue);
        hitbox.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < monsterLayerNumber.Length; i++)
            if (other.gameObject.layer == monsterLayerNumber[i])
            {
                if (sustainmentTime == 0)
                {
                    hitbox.enabled = false;
                    this.enabled = false;
                }
                if (explosion) // Attack
                {
                    GameObject explosionEffect = Instantiate(explosion, other.gameObject.transform.transform.position, Quaternion.identity);
                    Destroy(explosionEffect, 2f);

                    if (skillName == PlayerAttackSkill.skill.None)
                        Destroy(gameObject);
                    break;
                }
            }

    }

    private IEnumerator SustainmentTimeOfSkill()
    {
        yield return new WaitForSeconds(sustainmentTime);
        hitbox.enabled = false;
        this.enabled = false;
    }
    private IEnumerator delaySkillDamage(int repeat)
    {
        for (int i = 0; i< repeat;)
        {
            yield return new WaitForSeconds(0.35f);
            hitbox.enabled = !hitbox.enabled;
            if (Time.timeScale != 0) i++;
        }
        hitbox.enabled = false;
        skillDamageValue = 0;
        this.enabled = false;
    }
}
