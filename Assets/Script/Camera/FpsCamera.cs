using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    public Transform head;

    public float ySight;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float posY = head.position.y + ySight;

        transform.position = new Vector3(head.position.x, posY, head.position.z);
    }
}
