using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    private void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player") {
            GetComponentInParent<MonsterAI>().m_isInRange = true;
            GetComponentInParent<MonsterAI>().Attack();
        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player") {
            GetComponentInParent<MonsterAI>().m_isInRange = false;
        }
    }
}
