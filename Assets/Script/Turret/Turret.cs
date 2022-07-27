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
    private Collider parentObjectCollider;

    [Space]
    [Header("UI")]
    public GameObject hui; // TODO: Get Slider instead of GameObject and rename this field to huiSlider
    public Slider hpbar; // TODO: Rename this field to "hpBar"
    public GameObject target;
    public LayerMask targetName;

    [Space]
    [Header("Attack Setting")]

    public GameObject attack;
    public GameObject attackPrefab;

    // Auto Setting
    private float currTime;

    [Space]
    [Header("Turret Status")]
    public float hp = 100;
    public bool isLive; // TODO: Rename this field to "isAlive"
    
    private float fullHp;
    private bool die;


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

    private void Start()
    {
        parentObjectCollider = parentObject.GetComponent<Collider>();
    }
    
    private void Update()
    {
        var transformPos = transform.position;
        var targetTransformPos = target.transform.position;

        if (target != null) // Check distance with Target.
        {
            Vector3 pos = new(transformPos.x, 0, transformPos.z);
            Vector3 targetPos = new(targetTransformPos.x, 0, targetTransformPos.z);
            
            if (Vector3.Distance(pos, targetPos) > radius)
            {
                target = null;
            }
        }
        
        if (isLive) // Change HpBar value and show emergency effect when Hp is low. 
        {
            if (hui && hui.GetComponentInChildren<Slider>().maxValue == hpbar.maxValue && (int)hpbar.value != (int)hp)
            {
                hpbar.value -= damageSpeed * Time.deltaTime;
            }

            if (hp <= fullHp / 2 && emergency[0] && !emergency[0].activeSelf)
            {
                emergency[0].SetActive(true);
            }

            if (hp <= fullHp / 5 && emergency[1] && !emergency[1].activeSelf)
            {
                emergency[1].SetActive(true);
            }
        }

        if (hp == 0 && die == false) // Turret is die.
        {
            die = true;
            parentObjectCollider.enabled = false;
            
            Destroy(parentObject,0.1f);
            Destroy(hui);
            
            if (emergency.Length >=3 && emergency[2])
            {
                GameObject explosion = Instantiate(emergency[2],transform.position,Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration + 5f);
            }
        }

        if (target != null) // Check Target is live.
        {
            if (target.GetComponent<MonsterAI>())
            {
                if (target.GetComponent<Animator>().GetBool("Death"))
                {
                    target = null;
                    return;
                }
            }
            
            transform.LookAt(target.transform);
            Debug.DrawLine(transformPos, targetTransformPos, Color.blue);
            Physics.Linecast(transformPos, targetTransformPos);
            
            if (attack != null)
            {
                attack.transform.LookAt(target.transform.position);
            }
            
            if (Time.time > currTime + attackCycleTime)
            {
                currTime = Time.time;
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
