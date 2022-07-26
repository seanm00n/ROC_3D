using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotWall : MonoBehaviour
{
    public bool FPS = false;
    CameraManager cam;
    private void Start()
    {
        cam = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttack") == false)
        {
            for (int i = 0; i < cam.exceptLayerNum.Length; i++)
            {
                if (other.gameObject.layer == cam.exceptLayerNum[i])
                {
                    return;
                }
            }

            if (other.gameObject.layer != cam.target.gameObject.layer)
                CameraManager.instance.Value = 2;
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerAttack") == false)
        {
            if (other.gameObject.layer != cam.target.gameObject.layer)
                CameraManager.instance.Value = 0;
        }
    }
}
