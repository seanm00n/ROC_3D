using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    float m_attackDelay = 1f;
    bool isDelay = false;
    GameObject Player;
    private void Start () {
        Player = GameObject.Find("Test_Player");
    }
    public IEnumerator Attack () {
        Debug.Log("Attack");
        //GetComponentInParent<Animator>().SetBool("Attack", true);
        //Player.GetComponent<PlayerAnimControl>().Hit(m_attack);
        yield return new WaitForSeconds(m_attackDelay);
        isDelay = false;        
    }
    private void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player") {
            GetComponentInParent<MonsterAI>().m_isInRange = true;
            if (!isDelay) {
                GetComponentInParent<MonsterAI>().StartCoroutine(Attack());
                isDelay = true;
            }
        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player") {
            GetComponentInParent<MonsterAI>().m_isInRange = false;
        }
    }
}
