using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    
    public int attack = 0;
    [SerializeField] bool isThrow;
    void Start()
    {
        if (isThrow) {
            GetComponent<Rigidbody>().AddForce(transform.forward * 700f, ForceMode.Force);
        }
        
    }
    private void OnCollisionEnter (Collision collision) {
        if(collision.gameObject.tag == "Player"||
           collision.gameObject.tag == "Turret"||
           collision.gameObject.tag == "HQ") {
           collision.gameObject.GetComponent<IBattle>().Hit(attack);
            if (!isThrow) {
                Destroy(gameObject, 1f);
            } else {
                Destroy(gameObject);
            }
        }   
    }
}
