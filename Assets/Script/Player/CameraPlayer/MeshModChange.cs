using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModChange : MonoBehaviour
{
    Material materialOrigin;

    public Material transparentMat;
    void Start()
    {
        materialOrigin = GetComponent<Renderer>().material;
        
    }
    private void Update()
    {
        if (Camera_manager.fpsMode == true)
        {
            GetComponent<Renderer>().material = transparentMat;
        }
        else
            GetComponent<Renderer>().material = materialOrigin;
    }
}
