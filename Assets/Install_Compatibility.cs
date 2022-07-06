using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Install_Compatibility : MonoBehaviour
{
    public bool Compatibility = true;

    public Renderer[] remderers;
    private void OnTriggerEnter()
    {
        Compatibility = false;
    }
    private void OnTriggerExit()
    {
        Compatibility = true;
    }
}
