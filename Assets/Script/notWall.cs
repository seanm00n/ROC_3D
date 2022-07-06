using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notWall : MonoBehaviour
{
    public bool FPS = false;
    Camera_manager cam;
    private void Start()
    {
        cam = FindObjectOfType<Camera_manager>();
    }

    private void Update()
    {
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer != cam.target.gameObject.layer)
        Camera_manager.instance.Value = 2;
    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer != cam.target.gameObject.layer)
        Camera_manager.instance.Value = 0;
    }
}
