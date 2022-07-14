using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Install_Compatibility : MonoBehaviour
{
    public bool Compatibility = true;
    public int ExceptLayer = 8;
    public Renderer[] remderers;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != ExceptLayer)
            Compatibility = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != ExceptLayer)
            Compatibility = true;
    }
}