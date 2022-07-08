using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        transform.Rotate(transform.right * 100f * Time.deltaTime);        
    }

}
