using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour, IBattle
{
    int m_health = 100;
    public void Hit (int damage) {
        m_health -= damage;
    }
}
