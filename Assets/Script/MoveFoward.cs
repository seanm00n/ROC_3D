using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    
    void Start()
    {
        
    }

   
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Force);
    }
}
