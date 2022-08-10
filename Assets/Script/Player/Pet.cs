using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Pet : MonoBehaviour
{
    
    [Header("Pet Setting")]
    public int damageSpeed = 30;
    public float attackCycleTime = 2f;
    public float radius;
    public Transform scope;
    
    [Space]
    [Header("Target")]
    public GameObject target;
    public LayerMask targetName;
    
    [Space]
    [Header("Attack Setting")]

    private GameObject attack;
    public GameObject attackPrefab;
    public ParticleSystem fireEffect;
    public Transform firePosition;

    // Auto Setting
    private float currTime;
    private Animator animator;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(0, 0, 0, 0.1f);
        Handles.DrawSolidDisc(scope.position, scope.up, radius);
    }
#endif
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var transformPos = transform.position;
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
                transform.LookAt(target.transform.position + new Vector3(0, 1, 0));
            Debug.DrawLine(transformPos, targetTransformPos, Color.blue);
            Physics.Linecast(transformPos, targetTransformPos);

            if (target)
                transform.LookAt(target.transform.position + new Vector3(0, 1, 0));


            if (Time.time > currTime + attackCycleTime && Player.instance.hp != 0)
            {
                currTime = Time.time;
                animator.SetTrigger("Attack");
            }

                return; // Block Repetitive Statement below.
        }
            //// Find Target ///
            var scopeEntity = Physics.OverlapSphere(scope.position, radius, targetName);

        for (int i = 0; i < scopeEntity.Length; i++)
        {
            target = scopeEntity[i].gameObject;
        }
    }

    public void Attack()
    {   
        attack = Instantiate(attackPrefab, firePosition.position, transform.rotation);
        fireEffect.Play();
    }
    
}
