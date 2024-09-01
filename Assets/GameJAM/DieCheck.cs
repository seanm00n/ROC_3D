using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCheck : MonoBehaviour
{
    public float deathHeight;
    private void Update()
    {
        if (transform.position.y <= deathHeight)
        {
            Player.instance?.Hit(2147483647);
        }
    }
}
