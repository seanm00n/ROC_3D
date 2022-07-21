using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSmoothInterpolation : MonoBehaviour
{
    Vector3 top = new Vector3(0.12f,-0.53f,-1.6f);
    Vector3 bottom = new Vector3(0.12f,0.96f,-1.54f);
    Vector3 original;
    Transform cam;

    private void Awake()
    {
        original = transform.localPosition;
        cam = Camera.main.transform;
    }
    void Update()
    {
        if (Camera_manager.instance.targetNum == 1)
        {
            if (Input.GetAxisRaw("Mouse Y") > 0)
            {
                if (cam.localEulerAngles.x > 300 && cam.localEulerAngles.x < 360)
                {
                    float posX = (top.x - transform.localPosition.x) * 10 * Time.deltaTime;
                    float posY = (top.y - transform.localPosition.y) * 10 * Time.deltaTime;
                    float posZ = (top.z - transform.localPosition.z) * 10 * Time.deltaTime;

                    transform.localPosition = new Vector3(transform.localPosition.x + posX, transform.localPosition.y + posY, transform.localPosition.z + posZ);
                }
                else
                {
                    float posX = (original.x - transform.localPosition.x) * 2 * Time.deltaTime;
                    float posY = (original.y - transform.localPosition.y) * 2 * Time.deltaTime;
                    float posZ = (original.z - transform.localPosition.z) * 2 * Time.deltaTime;

                    transform.localPosition = new Vector3(transform.localPosition.x + posX, transform.localPosition.y + posY, transform.localPosition.z + posZ);
                }
            }
            else if (Input.GetAxisRaw("Mouse Y") < 0)
            {
                if (cam.localEulerAngles.x > 15)
                {
                    float posX = (bottom.x - transform.localPosition.x) * 5 * Time.deltaTime;
                    float posY = (bottom.y - transform.localPosition.y) * 5 * Time.deltaTime;
                    float posZ = (bottom.z - transform.localPosition.z) * 5 * Time.deltaTime;

                    transform.localPosition = new Vector3(transform.localPosition.x + posX, transform.localPosition.y + posY, transform.localPosition.z + posZ);
                }
                else
                {
                    float posX = (original.x - transform.localPosition.x) * 10 * Time.deltaTime;
                    float posY = (original.y - transform.localPosition.y) * 10 * Time.deltaTime;
                    float posZ = (original.z - transform.localPosition.z) * 10 * Time.deltaTime;

                    transform.localPosition = new Vector3(transform.localPosition.x + posX, transform.localPosition.y + posY, transform.localPosition.z + posZ);
                }
            }
        }
    }
}
