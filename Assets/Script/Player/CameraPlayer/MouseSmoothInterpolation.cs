using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseSmoothInterpolation : MonoBehaviour
{
    Vector3 top = new Vector3(0.12f,-1.41f,-1.82f);
    Vector3 bottom = new Vector3(0.12f,1.46f,-0.69f);
    Vector3 original;
    Transform cam;
    private void Awake()
    {
        original = transform.localPosition;
    }
    private void Start()
    {
        cam = Camera.main.transform;
    }
    void Update()
    {
        if (CameraManager.instance.targetNum == 1)
        {
            if (Input.GetAxisRaw("Mouse Y") > 0.3f)
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
                    float posX = (original.x - transform.localPosition.x) * 10 * Time.deltaTime;
                    float posY = (original.y - transform.localPosition.y) * 10 * Time.deltaTime;
                    float posZ = (original.z - transform.localPosition.z) * 10 * Time.deltaTime;

                    transform.localPosition = new Vector3(transform.localPosition.x + posX, transform.localPosition.y + posY, transform.localPosition.z + posZ);
                }
            }
            else if (Input.GetAxisRaw("Mouse Y") < -0.3f)
            {
                if (cam.localEulerAngles.x > 300 && cam.localEulerAngles.x < 355)
                {
                    float posX = (original.x - (transform.localPosition.x -360)) * 10 * Time.deltaTime;
                    float posY = (original.y - (transform.localPosition.y - 360)) * 10 * Time.deltaTime;
                    float posZ = (original.z - (transform.localPosition.z - 360)) * 10 * Time.deltaTime;
                }
                else if (cam.localEulerAngles.x > 15)
                {
                    float posX = (bottom.x - transform.localPosition.x) * 10 * Time.deltaTime;
                    float posY = (bottom.y - transform.localPosition.y) * 10 * Time.deltaTime;
                    float posZ = (bottom.z - transform.localPosition.z) * 10 * Time.deltaTime;

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
