using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour, IItem
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use();
        }
    }

    public void Use()
    {
        Player.instance.mp = Player.maxMp;
        Destroy(gameObject);
    }
}
