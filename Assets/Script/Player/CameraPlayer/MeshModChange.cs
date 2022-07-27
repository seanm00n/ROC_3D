using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModChange : MonoBehaviour
{
    [Header("PlayerMaterial")]
    Material materialOrigin; // Original material.
    public Material materialDamaged; // Damaged material.
    public Material transparentMat; // invisible material(Fps mode).

    public void DamagedMat()
    {
        if(materialDamaged) // Damaged Player's material is changed.
        GetComponent<Renderer>().material = materialDamaged;
    }

    void Start()
    {
        materialOrigin = GetComponent<Renderer>().material;
        
    }
    private void Update()
    {
        if (CameraManager.fpsMode == true)
        {
            GetComponent<Renderer>().material = transparentMat; // Player is invisible in fps mode.
        }
        else
        {
            if (Player.instance.hit == true)
            {
                GetComponent<Renderer>().material = materialDamaged;
            }
            else

                GetComponent<Renderer>().material = materialOrigin; // return to original player material.
        }
    }
}
