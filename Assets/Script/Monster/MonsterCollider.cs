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
            //GetComponentInParent<Animator>().SetBool("Attack", true);
            Debug.Log("Attack");
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
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
            GetComponentInParent<MonsterAI>().m_isInRange = false;
            StopCoroutine(cAttack);
        }
    }
}
