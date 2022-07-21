using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    float m_attackDelay = 1f;
    IEnumerator cAttack;
    private void Start () {
        cAttack = Attack();
    }
    public IEnumerator Attack () {
        while (true) {
            GetComponent<Animator>().SetBool("Attack", true);
            yield return new WaitForSeconds(m_attackDelay);
        }
    }
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
            GetComponentInParent<MonsterAI>().m_isInRange = true;
            StartCoroutine(cAttack);
        }
        if(other.gameObject.tag == "PlayerAttack") {
            Debug.Log("Monster Hit");
            //this.HP -= other.AP;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            PlayerAnimControl.instance.Hit(10f);
        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
            StopCoroutine(cAttack);
            GetComponent<MonsterAI>().m_isInRange = false;
            GetComponent<Animator>().SetBool("Attack", false);
            
        }
    }
}
