using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Install_Compatibility : MonoBehaviour
{
    [Header("Check surrounding object")]
    public bool compatibility = true;

    [Header("Cut out of Check")]
    public int exceptLayer = 8;
    
    // TODO: Rename this field to "exceptLayer2", and explain the difference between exceptLayer and exceptLayer2
    public int exceptLayer_2 = 10;

    [Header("Object Renderer (Surely Add)")]
    public Renderer[] renderers;

    private void OnTriggerEnter(Collider other) => UpdateCompatibility(other);
    private void OnTriggerStay(Collider other) => UpdateCompatibility(other);
    private void OnTriggerExit(Collider other) => UpdateCompatibility(other, true);

    private void UpdateCompatibility(Component other, bool value = false)
    {
        if (other.gameObject.layer != exceptLayer && other.gameObject.layer != exceptLayer_2)
            compatibility = value;
    }
}