using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Turret : MonoBehaviour
{
    bool Die = false;

    public int DamageSpeed = 30;
    public GameObject[] Emergency;
    public GameObject DamageEffect;

    public GameObject ParentObject;
    public Transform FirePosition;
    public bool isLive = false;

    public GameObject Hui;
    public Slider Hpbar;
    public float HP = 100;
    float Full_HP;
    public GameObject AttackPrefab;
    public GameObject target;
    public GameObject Attack;
    public LayerMask targetName;
    public float radius;
    public Transform scope;

    public float attackCycleTime = 2f;

    float currentTime;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(0,0,0,0.1f);
        Handles.DrawSolidDisc(scope.position, scope.up, radius);
    }
#endif


    private void Awake()
    {
        Full_HP = HP;
    }
    // Start is called before the first frame update
    public void OnDamaged(int damage)
    {
        GameObject hit = Instantiate(DamageEffect, transform.position, Quaternion.identity);
        Destroy(hit, hit.GetComponent<ParticleSystem>().main.duration);
        HP -= damage;

    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 Pos = new Vector3 (transform.position.x,0,transform.position.z);
            Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            if (Vector3.Distance(Pos, targetPos) > radius)
            {
                target = null;
            }
        }
        if (isLive)
        {
            if (Hui && Hui.GetComponentInChildren<Slider>().maxValue == Hpbar.maxValue && (int)Hpbar.value != HP)
            {
                Hpbar.value -= DamageSpeed * Time.deltaTime;
            }

            if (HP <= Full_HP / 2 && Emergency[0] && Emergency[0].activeSelf == false)
            {
                Emergency[0].SetActive(true);
            }

            if (HP <= Full_HP / 5 && Emergency[1] && Emergency[1].activeSelf == false)
            {
                Emergency[1].SetActive(true);
            }
        }

        if (HP == 0 && Die == false)
        {
            Die = true;
            ParentObject.GetComponent<Collider>().enabled = false;
            Destroy(ParentObject,0.1f);
            Destroy(Hui);
            if (Emergency.Length >=3 && Emergency[2]  )
            {
                GameObject Explosion = Instantiate(Emergency[2],transform.position,Quaternion.identity);
                Destroy(Explosion, Explosion.GetComponent<ParticleSystem>().main.duration + 5f);
            }
        }

        var scopeEntity = Physics.OverlapSphere(scope.position, radius,targetName);

        if (target != null)
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
            if (Attack != null)
            {
                Attack.transform.LookAt(target.transform.position);
            }
            if (Time.time > currentTime + attackCycleTime)
            {
                currentTime = Time.time;
                Attack = Instantiate(AttackPrefab, FirePosition.position, transform.rotation);
            }
            return; // Block Repetitive Statement below.
        }

        for (int i = 0; i < scopeEntity.Length; i++)
        {
            target = scopeEntity[i].gameObject;
        }
        
    }
}
