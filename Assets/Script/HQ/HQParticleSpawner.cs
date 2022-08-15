using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQParticleSpawner : MonoBehaviour
{
    public int spawnAmount = 5;

    public GameObject particleMini;
    public void spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Instantiate(particleMini, transform.localPosition, transform.rotation);
        }
        Instantiate(particleMini, transform.localPosition, transform.rotation);
        Destroy(this.gameObject);
    }
}
