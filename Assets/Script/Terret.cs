using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Terret : MonoBehaviour
{
    public GameObject ParentObject;
    public Transform FirePosition;
    public bool isLive = false;
    public Slider Hpbar;
    public float HP = 100;
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

    // Start is called before the first frame update
    public void OnDamaged(int damage)
    {
        HP -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLive)
        {
            if (Hpbar && Hpbar.value != HP)
            {
                Hpbar.value = HP;
            }
        }

        if(HP == 0)
        {
            Destroy(ParentObject);
        }

        var scopeEntity = Physics.OverlapSphere(scope.position, radius,targetName);

        if (target != null)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
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
            return;
        }

        for (int i = 0; i < scopeEntity.Length; i++) 
        {
            target = scopeEntity[i].gameObject;
        }
        
    }
}
