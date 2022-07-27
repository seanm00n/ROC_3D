using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    [Header("Damage")]
    public float skill_Damage_Value = 0;

    [Space]
    [Header("Master Setting")]
    public bool turretAttack = false;

    [Space]
    [Header("MonserLayer")]
    public int monsterlayerNumber = 0;

    [Space]
    [Header("Effect")]
    public GameObject explosion;

    Rigidbody r;

    void Start()
    {
        if (turretAttack)
        {
            r = GetComponent<Rigidbody>();
            r.AddForce(transform.forward * 500f);
            Destroy(gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!turretAttack)
        {
            if (other.gameObject.layer == monsterlayerNumber)
            {
                GameObject Explo = Instantiate(explosion, other.gameObject.transform.transform.position, Quaternion.identity);
                
                Destroy(Explo, 2f);
                Destroy(gameObject);
            }
        }
    }
}
