using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    public Transform head;
    public Transform head_2;

    public float ySight;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float posY = head.position.y + ySight;
        float posY_2 = head_2.position.y + ySight;

        PlayerMovement p = FindObjectOfType<PlayerMovement>();
        if (head_2 && p.speed == p.Slowspeed)
        {
            transform.position = new Vector3(head_2.position.x, head_2.position.y, head_2.position.z);
        }
        else
        transform.position = new Vector3(head.position.x, posY, head.position.z);
    }
}
