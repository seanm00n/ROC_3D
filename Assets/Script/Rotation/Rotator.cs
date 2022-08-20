using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Always gameObject rotate.
    public int num = 0;
    public int speed = 0;
    private void Update()
    {
        switch (num) 
        {
            case 0
            : transform.Rotate(transform.up * speed * Time.deltaTime);
                break;
            case 1
            : transform.Rotate(-transform.up * speed * Time.deltaTime);
                break;
            case 2
            : transform.Rotate(-transform.right * speed * Time.deltaTime);
                break;
            case 3
            : transform.Rotate(transform.right * speed * Time.deltaTime);
                break;
            case 4
            :
                transform.Rotate(transform.forward * speed * Time.deltaTime);
                break;
            case 5
            :
                transform.Rotate(-transform.forward * speed * Time.deltaTime);
                break;
        }
    }
}
