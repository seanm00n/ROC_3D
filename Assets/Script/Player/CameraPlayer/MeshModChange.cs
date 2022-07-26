using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModChange : MonoBehaviour
{
    Material materialOrigin;
    public Material materialDamaged;

    public Material transparentMat;
    public void DamagedMat()
    {
        if(materialDamaged)
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
            GetComponent<Renderer>().material = transparentMat;
        }
        else
        {
            if (Player.instance.hit == true)
            {
                GetComponent<Renderer>().material = materialDamaged;
            }
            else

                GetComponent<Renderer>().material = materialOrigin;
        }
    }
}
