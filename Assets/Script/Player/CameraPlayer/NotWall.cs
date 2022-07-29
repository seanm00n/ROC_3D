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

    void OnTriggerStay(Collider other)
    {
        if (transform.parent != Player.instance.transform) return; // Ignore other player
        
        if (other.gameObject.CompareTag("PlayerAttack")) return; // Ignore attack

        for (int i = 0; i < cam.exceptLayerNum.Length; i++)
        {
            if (other.gameObject.layer == cam.exceptLayerNum[i])
            {
                // skip if the layer should be ignored
                return;
            }
        }

        if (other.gameObject.layer != cam.target.gameObject.layer)
            cam.clampedPos = 2; // there IS a wall behind the player
    }

    public void OnTriggerExit(Collider other)
    {
        if (transform.parent != Player.instance.transform) return;
        
        if (other.gameObject.CompareTag("PlayerAttack")) return;
        
        if (other.gameObject.layer != cam.target.gameObject.layer)
            CameraManager.instance.clampedPos = 0; // there ISN'T a wall behind the player
    }
}
