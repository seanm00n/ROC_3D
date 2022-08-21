using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Turret : MonoBehaviour, IBattle
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
    public Transform firePosition2;
    private Collider parentObjectCollider;

    [Space]
    [Header("UI")]
    public GameObject hui; // Need it to delete hp ui when turret is gone.
    public Slider hpBar;

    [Space]
    [Header("Target")]
    public GameObject target;
    public LayerMask targetName;

    [Space]
    [Header("Attack Setting")]

    private GameObject attack;
    public GameObject attackPrefab;
    public ParticleSystem fireEffect;
    public ParticleSystem fireEffect2;

    // Auto Setting
    private float currTime;

    [Space]
    [Header("Turret Status")]
    public GameObject shooter;
    public GameObject shooter2;
    public int level = 0;
    public int hp = 100;
    public bool isAlive;    
    private float fullHp;
    public bool die = false;


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
        PlayerSaveData.turretAmount += 1;
    }

    private void Start()
    {
        parentObjectCollider = GetComponent<Collider>();
    }
    
    private void Update()
    {
        if (hp <= 0.01f && die == false) // Turret is die.
        {
            PlayerSaveData.turretAmount -= 1;
            die = true;
            parentObjectCollider.enabled = false;

            Destroy(parentObject, 0.1f);
            Destroy(hui);

            if (emergency.Length >= 3 && emergency[2])
            {
                GameObject explosion = Instantiate(emergency[2], shooter.transform.position, Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration + 5f);
            }
        }
        var transformPos = shooter.transform.position;
      
        if (isAlive) // Change HpBar value and show emergency effect when Hp is low. 
        {
            if (hpBar && (int)hpBar.value != hp)
            {
                hpBar.value -= damageSpeed * Time.deltaTime;
            }

            if (hpBar.value <= (hpBar.maxValue / 2) && emergency[0] && !emergency[0].activeSelf)
            {
                emergency[0].SetActive(true);
            }

            if (hpBar.value <= (hpBar.maxValue / 5) && emergency[1] && !emergency[1].activeSelf)
            {
                emergency[1].SetActive(true);
            }
        }

      

        if (target != null) 
        {
            // Check distance with Target.
            var targetTransformPos = target.transform.position;
            Vector3 pos = new(transformPos.x, 0, transformPos.z);
            Vector3 targetPos = new(targetTransformPos.x, 0, targetTransformPos.z);

            if (Vector3.Distance(pos, targetPos) > radius)
            {
                target = null;
            }

            // Check Target is live.
            if (target && target.GetComponent<MonsterAI>())
            {
                if (target.GetComponent<Animator>().GetBool("Death"))
                {
                    target = null;
                    return;
                }
            }
            if (target)
            {
                shooter.transform.LookAt(target.transform.position + new Vector3(0, 0.5f, 0));
                if (shooter2)
                {
                    float Xrot = shooter2.transform.localEulerAngles.x;
                    float Zrot = shooter2.transform.localEulerAngles.z;
                    shooter2.transform.LookAt(target.transform.position + new Vector3(0, 0.5f, 0));
                    shooter2.transform.localEulerAngles = new Vector3(Xrot, shooter2.transform.localEulerAngles.y, Zrot);    
                }
            }
            Debug.DrawLine(transformPos, targetTransformPos, Color.blue);
            Physics.Linecast(transformPos, targetTransformPos);
            
            if (attack != null)
            {
                if(target)
                attack.transform.LookAt(target.transform.position + new Vector3(0,0.5f,0));
            }
            
            if (Time.time > currTime + attackCycleTime && target)
            {
                currTime = Time.time;
                attack = Instantiate(attackPrefab, firePosition.position, shooter.transform.rotation);
                attack.GetComponent<ProjectileMover>().turretLevel = level;
                fireEffect.Play();
                if (firePosition2)
                {
                    attack = Instantiate(attackPrefab, firePosition2.position, shooter.transform.rotation);
                    attack.GetComponent<ProjectileMover>().turretLevel = level;
                    fireEffect2.Play();
                }
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
    public void Hit(int damage) // If Turret is damaged.
    {
        GameObject hit = Instantiate(damageEffect, shooter.transform.position, Quaternion.identity);
        Destroy(hit, hit.GetComponent<ParticleSystem>().main.duration);
        hp -= damage;
    }
}
