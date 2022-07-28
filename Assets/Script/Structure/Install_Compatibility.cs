using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Install_Compatibility : MonoBehaviour
{
    [Header("Check surrounding object")]
    public bool compatibility = true;

    [Header("Cut out of Check")]
    public int structureLayer = 8; // This ground is place that player can build.

    public int structureAreaLayer = 10; // Place that player can build. 

    [Header("Object Renderer (Surely Add)")]
    public Renderer[] renderers;

    // Check Colider.
    private void OnTriggerEnter(Collider other) => UpdateCompatibility(other);
    private void OnTriggerStay(Collider other) => UpdateCompatibility(other);
    private void OnTriggerExit(Collider other) => UpdateCompatibility(other, true);

    // Check build possibility.
    private void UpdateCompatibility(Component other, bool value = false)
    {
        if (other.gameObject.layer != structureLayer && other.gameObject.layer != structureAreaLayer)
            compatibility = value;
    }
}