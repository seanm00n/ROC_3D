using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Terret : MonoBehaviour
{
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var scopeEntity = Physics.OverlapSphere(scope.position, radius,targetName);

        if (target != null)
        {
            transform.LookAt(target.transform.position);
            Debug.DrawLine(transform.position, target.transform.position, Color.blue);
            Physics.Linecast(transform.position,target.transform.position);
            if (Attack != null)
            {
                Attack.transform.LookAt(target.transform.position);
            }
            if (Time.time > currentTime + attackCycleTime)
            {
                currentTime = Time.time;
                Attack = Instantiate(AttackPrefab, transform.position, transform.rotation);
            }
            return;
        }

        for (int i = 0; i < scopeEntity.Length; i++) 
        {
            target = scopeEntity[i].gameObject;
        }
        
    }
}
