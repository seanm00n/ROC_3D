using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour, IBattle
{
    int m_health = 100000;
    private void Update () {
        GameOver();
    }
    public void GameOver () {
        if (m_health < 0) {
            //게임오버
            Debug.Log("HQ Down");
            Destroy(gameObject);
        }
    }
    public void Hit (int damage) {
        m_health -= damage;
    }
    
}
