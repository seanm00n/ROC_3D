using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component changes the material of the player
public class MeshModChange : MonoBehaviour
{
    [Header("PlayerMaterial")]
    private Material materialOrigin; // Original material.
    public Material materialDamaged; // Damaged material.
    public Material transparentMat; // invisible material(Fps mode).

    public void DamagedMat()
    {
        if (materialDamaged) // Damaged Player's material is changed.
            GetComponent<Renderer>().material = materialDamaged;
    }

    void Start()
    {
        materialOrigin = GetComponent<Renderer>().material;
        
    }
    private void Update()
    {
        if (Player.instance.transform != transform.parent) return;
        
        if (CameraManager.fpsMode)
        {
            GetComponent<Renderer>().material = transparentMat; // Player is invisible in fps mode.
        }
        else
        {
            GetComponent<Renderer>().material = Player.instance.hit ? materialDamaged : materialOrigin;
        }
    }
}
