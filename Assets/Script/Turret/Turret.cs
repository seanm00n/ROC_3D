using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Turret : MonoBehaviour
{
    [Header("Turret Setting")]
    public int damageSpeed = 30;
    public float attackCycleTime = 2f;
    public float radius;
    public Transform scope;

    [Space]

    public GameObject[] emergency;
    public GameObject damageEffect;
    public GameObject parentObject;
    public Transform firePosition;

    [Space]
    [Header("UI")]
    public GameObject hui;
    public Slider hpbar;
    public GameObject target;
    public LayerMask targetName;

    [Space]
    [Header("Attack Setting")]

    public GameObject attack;
    public GameObject attackPrefab;

    //Auto Setting
    float currentTime;

    [Space]
    [Header("Turret Status")]
    public float hp = 100;
    float fullHp;
    public bool isLive = false;
    bool die = false;


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(0,0,0,0.1f);
        Handles.DrawSolidDisc(scope.position, scope.up, radius);
    }
#endif


    private void Awake()
    {
        fullHp = hp;
    }
    void Update()
    {
        if (target) // Check distance with Target.
        {
            Vector3 Pos = new Vector3 (transform.position.x,0,transform.position.z);
            Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            if (Vector3.Distance(Pos, targetPos) > radius)
            {
                target = null;
            }
        }
        if (isLive) // Change Hpbar value and show emergency effect when Hp is low. 
        {
            if (hui && hui.GetComponentInChildren<Slider>().maxValue == hpbar.maxValue && (int)hpbar.value != hp)
            {
                hpbar.value -= damageSpeed * Time.deltaTime;
            }

            if (hp <= fullHp / 2 && emergency[0] && emergency[0].activeSelf == false)
            {
                emergency[0].SetActive(true);
            }

            if (hp <= fullHp / 5 && emergency[1] && emergency[1].activeSelf == false)
            {
                emergency[1].SetActive(true);
            }
        }

        if (hp == 0 && die == false) // Turret is die.
        {
            die = true;
            parentObject.GetComponent<Collider>().enabled = false;
            Destroy(parentObject,0.1f);
            Destroy(hui);
            if (emergency.Length >=3 && emergency[2]  )
            {
                GameObject Explosion = Instantiate(emergency[2],transform.position,Quaternion.identity);
                Destroy(Explosion, Explosion.GetComponent<ParticleSystem>().main.duration + 5f);
            }
        }

        if (target != null) // Check Target is live.
        {
            if (target.GetComponent<MonsterAI>() != null)
            {
                if (target.GetComponent<Animator>().GetBool("Death"))
                {
                    target = null;
                    return;
                }
            }
            transform.LookAt(target.transform);
            Debug.DrawLine(transform.position, target.transform.position, Color.blue);
            Physics.Linecast(transform.position,target.transform.position);
            if (attack != null)
            {
                attack.transform.LookAt(target.transform.position);
            }
            if (Time.time > currentTime + attackCycleTime)
            {
                currentTime = Time.time;
                attack = Instantiate(attackPrefab, firePosition.position, transform.rotation);
            }
            return; // Block Repetitive Statement below.
        }

        //// Find Target ///
        var scopeEntity = Physics.OverlapSphere(scope.position, radius,targetName);

        for (int i = 0; i < scopeEntity.Length; i++)
        {
            target = scopeEntity[i].gameObject;
        }
        
    }
    public void OnDamaged(int damage) // If Turret is damaged.
    {
        GameObject hit = Instantiate(damageEffect, transform.position, Quaternion.identity);
        Destroy(hit, hit.GetComponent<ParticleSystem>().main.duration);
        hp -= damage;

    }

}
