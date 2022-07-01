using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_manager : MonoBehaviour
{
    public Camera c1;
    public Camera c2;
    public Camera c3;

    public float upperAngle = 0;
    //////////////////////////////////////////
    Transform target;
    public int targetNum = 1;
    public Transform targetPosition;

    public Transform[] targetPositions;

    public float cameraRotSpeed = 300;
    float cameraMoveSpeed = 8f;
    Vector3 cameraMoveVelocity;

    public float smoothSpeed = 50;
    float currentAngle;

    ////////////////////////
    float earlyTopAngle;
    float earlyDownAngle;
    ////////////////////////
    

    public float topAngle = 30;
    public float DownAngle = 0;

    public float maxAngle = 80;
    public float minAngle = -40f;

    void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
        earlyTopAngle = topAngle;
        earlyDownAngle = DownAngle;
    }

    void Update()
    {
        if (!target) return;
        CameraMove();
        CameraRotate();
    }

    void CameraMove()
    {
        Vector3 destination = new Vector3(targetPosition.position.x, targetPosition.position.y, targetPosition.position.z);
        Vector3 pVector = Vector3.Lerp(transform.position, destination,cameraMoveSpeed * Time.deltaTime);
        transform.position = pVector;

        float pointY = transform.eulerAngles.y + Input.GetAxisRaw("Mouse X") * cameraRotSpeed * Time.deltaTime;
        target.eulerAngles = new Vector3(target.rotation.x, pointY, target.rotation.z);
    }

    void CameraRotate()
    {
        float pointX = transform.eulerAngles.x - Input.GetAxisRaw("Mouse Y") * cameraRotSpeed * Time.deltaTime;
        Vector3 RoteteVelocity = new Vector3();

        if (minAngle >= 0)
        {
            upperAngle = ((pointX - (minAngle) + 1) / (maxAngle - minAngle + 1));
        }
        else
        {
            float Max = (-minAngle) + maxAngle + 1;
            if (transform.localEulerAngles.x > 300 || transform.localEulerAngles.x == 0)
            {
                float ThisNum = ((maxAngle) + 1) + (360-transform.localEulerAngles.x);
                upperAngle = (ThisNum / Max);
            }
            else if(transform.localEulerAngles.x > 0)
            {
                float ThisNum = (maxAngle - transform.localEulerAngles.x) + 1;
                upperAngle = (ThisNum / Max);
            }
        }


        if (targetNum == 1)
        {
            float Value = CameraReset(c1);

            if (Value == 0) targetPosition = targetPositions[0]; else { targetPosition = targetPositions[3]; }

            if (transform.localEulerAngles.x > topAngle) targetNum = 3; else if (pointX < DownAngle) targetNum = 2;
            RoteteVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
            transform.eulerAngles = RoteteVelocity;
        }
        else if (targetNum == 2)
        {
            float Value = CameraReset(c2);

            if (Value == 0) targetPosition = targetPositions[1]; else { targetPosition = targetPositions[4]; }
            currentAngle = transform.eulerAngles.x;
            if (minAngle + 360 > (pointX)) pointX = - 40;
            RoteteVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
            transform.eulerAngles = RoteteVelocity;
            if (40 > transform.eulerAngles.x) targetNum = 1;

        }
        else if (targetNum == 3)
        {
            float Value = CameraReset(c3);

            if (Value == 0) targetPosition = targetPositions[2]; else { targetPosition = targetPositions[5]; }
            currentAngle = transform.eulerAngles.x;
            if (maxAngle < transform.eulerAngles.x) pointX =  maxAngle;
            RoteteVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
            transform.eulerAngles = RoteteVelocity;
            if (30 > transform.eulerAngles.x) targetNum = 1; 
        }
        if (upperAngle == 1)
        {
            upperAngle = 0.9999f;
        }
        PlayerAnimControl.instance.AnimationAngleWork(upperAngle);
    }
    float CameraReset(Camera c)
    {
        RaycastHit hit;
        float Value = 0;
        var ray = c.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Linecast(ray.origin, target.position, out hit, ~target.GetComponent<PlayerMovement>().playerLayer))
        {
            Value = 1;
        }
        else Value = 0;

        return Value;
    }
}
