using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    [Header("Damage")]
    public float skillDamageValue;

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

    void Start()
    {
        if (turretAttack)
        {
            skillRigidbody = GetComponent<Rigidbody>();
            skillRigidbody.AddForce(transform.forward * 500f);
            Destroy(gameObject, 2f);
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
