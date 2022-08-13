using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour, IBattle
{
    int m_health = 100;
    private void Update () {
        if(m_health < 0) {
            Debug.Log("HQ Down");
            Destroy(gameObject);
        }
    }
    public void Hit (int damage) {
        m_health -= damage;
    }
    
}
