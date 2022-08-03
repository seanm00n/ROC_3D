using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    public PlayerAttackSkill.skill skillName;

    [Header("Damage")]
    public int skillDamageValue;

    [Space]
    [Header("Master Setting")]
    public bool turretAttack;

    [Space]
    [Header("MonsterLayer")]
    public int monsterLayerNumber; 

    [Space]
    [Header("Effect")]
    public GameObject explosion;

    private Rigidbody skillRigidbody;

    private void Start()
    {
        if (turretAttack)
        {
            skillRigidbody = GetComponent<Rigidbody>();
            skillRigidbody.AddForce(transform.forward * 500f);
            Destroy(gameObject, 2f);
        }
    }

    public void Update()
    {
        if (skillDamageValue != 0) return;
        if (Player.instance.playerAttackSkill.qSkill == skillName) 
        {
            skillDamageValue = PlayerAttackSkill.qSkillData.damage;
        }
        else if (Player.instance.playerAttackSkill.eSkill == skillName)
        {
            skillDamageValue = PlayerAttackSkill.eSkillData.damage;
        }
        else if (Player.instance.playerAttackSkill.rSkill == skillName)
        {
            skillDamageValue = PlayerAttackSkill.rSkillData.damage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (turretAttack) return;
        
        if (other.gameObject.layer == monsterLayerNumber) // Attack
        {
            GameObject explosionEffect = Instantiate(explosion, other.gameObject.transform.transform.position, Quaternion.identity);
                
            Destroy(explosionEffect, 2f);
            Destroy(gameObject);
        }
    }
}
