using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Always gameObject rotate.
    void Update()
    {
        transform.Rotate(transform.right * 100f * Time.deltaTime);        
    }

}
