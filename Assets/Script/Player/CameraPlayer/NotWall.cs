using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotWall : MonoBehaviour
{
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
        if (transform.parent == Player.instance.gameObject)
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
                    cam.clampedPos = 2; // Behind player is wall.
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (transform.parent == Player.instance.gameObject)
        {
            if (other.gameObject.CompareTag("PlayerAttack") == false)
            {
                if (other.gameObject.layer != cam.target.gameObject.layer)
                    CameraManager.instance.clampedPos = 0; // Behind player isn't wall.
            }
        }
    }
}
