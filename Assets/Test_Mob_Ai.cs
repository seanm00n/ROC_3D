using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Mob_Ai : MonoBehaviour
{
    public GameObject target;
    public float speed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
        {
            Terret terret = other.transform.parent.gameObject.GetComponentInChildren<Terret>();
            if (terret)
            {
                terret.OnDamaged(1);
            }
        }
    }

    private void Update()
    {
        if(target)
        transform.parent.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        transform.parent.position += transform.parent.forward * speed * Time.deltaTime;
    }
}
