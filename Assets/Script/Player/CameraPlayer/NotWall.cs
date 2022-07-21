using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotWall : MonoBehaviour
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
        if (other.gameObject.CompareTag("PlayerAttack") == false)
        {
            for (int i = 0; i < cam.ExceptLayerNum.Length; i++)
            {
                if (other.gameObject.layer == cam.ExceptLayerNum[i])
                {
                    return;
                }
            }

            if (other.gameObject.layer != cam.target.gameObject.layer)
                Camera_manager.instance.Value = 2;
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerAttack") == false)
        {
            if (other.gameObject.layer != cam.target.gameObject.layer)
                Camera_manager.instance.Value = 0;
        }
    }
}
