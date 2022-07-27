using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Install_Compatibility : MonoBehaviour
{
    [Header("Check surrounding object")]
    public bool compatibility = true;

    [Header("Cut out of Check")]
    public int exceptLayer = 8;
    public int exceptLayer_2 = 10;

    [Header("Object Renderer(Surely Add)")]
    public Renderer[] renderers;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != exceptLayer && other.gameObject.layer != exceptLayer_2)
            compatibility = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != exceptLayer && other.gameObject.layer != exceptLayer_2)
            compatibility = false;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != exceptLayer && other.gameObject.layer != exceptLayer_2)
            compatibility = true;
    }
}