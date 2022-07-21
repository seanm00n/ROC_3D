using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Install_Compatibility : MonoBehaviour
{
    public bool Compatibility = true;
    public int ExceptLayer = 8;
    public int ExceptLayer_2 = 10;
    public Renderer[] remderers;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != ExceptLayer && other.gameObject.layer != ExceptLayer_2)
            Compatibility = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != ExceptLayer && other.gameObject.layer != ExceptLayer_2)
            Compatibility = false;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != ExceptLayer && other.gameObject.layer != ExceptLayer_2)
            Compatibility = true;
    }
}