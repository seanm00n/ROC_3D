using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Target")]

    Transform targetLocation;

    [Header("FpsModeSetting")]

    public GameObject bookVisibleFps;

    [Space]
    
    [Header("ExtraCamera")]
    public Camera c1; // Middle sight of view.
    public Camera c2; // Down sight of view.
    public Camera c3; // Top sight of view.


    [Header("PlayerHeadRotation")]
    public float upperAngle = 0;

    [Space]

    public Transform target;
    public int targetNum = 1;
    public Transform targetPosition;

    [Space]

    public Transform[] targetPositions;

    public float cameraRotSpeed = 200;
    public float cameraMoveSpeed = 8f;
    Vector3 cameraMoveVelocity;

    [Space]

    [Header("AngleSetting")]

    float currentAngle;
    float earlyTopAngle;
    float earlyDownAngle;
    public float topAngle = 50;
    public float DownAngle = 0;
    public float maxAngle = 80;
    public float minAngle = -40f;

    [Space]

    public float smoothCameraTime;
    public float smoothCameraSpeed = 24f;

    [Space]
    public static CameraManager instance;
    public static bool fpsMode = false;

    public int[] exceptLayerNum;
    public float Value; //카메라 벽 넘기 방지

    private void Awake()
    {
        targetLocation = targetPositions[0];
        instance = this;
    }
    void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
        earlyTopAngle = topAngle;
        earlyDownAngle = DownAngle;
    }

    void Update()
    {
        if (CameraManager.fpsMode == true && bookVisibleFps)
        {
            bookVisibleFps.SetActive(true);
        }
        else
        {
            bookVisibleFps.SetActive(false);
        }
        if (smoothCameraTime > 0)
        {
            if (smoothCameraSpeed > cameraMoveSpeed && targetNum == 1)
            {
                float value = cameraMoveSpeed;
                cameraMoveSpeed = smoothCameraSpeed;
                smoothCameraSpeed = value;
            }
            smoothCameraTime -= Time.deltaTime;
        }
        else if(smoothCameraTime < 0)
        {
            smoothCameraTime = 0;
            float value = cameraMoveSpeed;
            cameraMoveSpeed = smoothCameraSpeed;
            smoothCameraSpeed = value;
        }

        if (!target) return;
        CameraMove();
        CameraRotate();

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (fpsMode) {fpsMode = false; Camera.main.nearClipPlane = 0.01f;}
            else { fpsMode = true;}
        }
    }


    void CameraMove()
    {
        if (!fpsMode)
        {
            Vector3 destination = new Vector3(targetPosition.position.x, targetPosition.position.y, targetPosition.position.z);
            //Vector3 pVector = Vector3.Lerp(transform.position, destination, cameraMoveSpeed * Time.deltaTime);
            transform.position = destination;

            float pointY = transform.eulerAngles.y + Input.GetAxisRaw("Mouse X") * cameraRotSpeed * Time.deltaTime;
            
            target.eulerAngles = new Vector3(target.rotation.x, pointY, target.rotation.z);
        }
        else
        {
            Vector3 destination = new Vector3(targetPosition.position.x, targetPosition.position.y, targetPosition.position.z);
            transform.position = destination;

            float pointY = target.eulerAngles.y + Input.GetAxisRaw("Mouse X") * cameraRotSpeed * Time.smoothDeltaTime;
            
            target.eulerAngles = new Vector3(target.rotation.x, pointY, target.rotation.z);  
        }
    }

    void CameraRotate()
    {
        float pointX = transform.eulerAngles.x - Input.GetAxisRaw("Mouse Y") * cameraRotSpeed * Time.deltaTime;
        if (pointX < 300 && pointX > maxAngle) pointX = maxAngle;
        Vector3 RoteteVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);

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

        if (!fpsMode)
        {
            float pointX_ = 0;
            if (pointX > 300 && DownAngle < 0)
            {
                pointX_ = pointX - 360;

            }
            if (targetNum == 1)
            {
                if (Value != 2)
                Value = CameraReset(c1);

                if (Value == 0) targetPosition = targetLocation; else { targetPosition = targetPositions[3]; }

                if (pointX_ != 0)
                {
                    if (pointX_ < DownAngle) targetNum = 2;
                }
                else 
                {
                    if (transform.localEulerAngles.x > topAngle) targetNum = 3; else if (pointX < DownAngle) targetNum = 2;
                }

                transform.eulerAngles = RoteteVelocity;
            }
            else if (targetNum == 2)
            {
                if (Value != 2)
                    Value = CameraReset(c2);

                if (Value == 0) targetPosition = targetPositions[1]; else { targetPosition = targetPositions[4]; }
                currentAngle = transform.eulerAngles.x;

                
                if (minAngle + 360 > (pointX)) pointX = -40;
                RoteteVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
                

                transform.eulerAngles = RoteteVelocity;
                if (DownAngle < pointX_ && pointX_ != 0) targetNum = 1;

            }
            else if (targetNum == 3)
            {
                if (Value != 2)
                    Value = CameraReset(c3);

                if (Value == 0) targetPosition = targetPositions[2]; else { targetPosition = targetPositions[5]; }
                currentAngle = transform.eulerAngles.x;
                RoteteVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
                transform.eulerAngles = RoteteVelocity;
                if (30 > transform.eulerAngles.x) targetNum = 1;
            }
        }
        else
        {
            targetPosition = targetPositions[6];
            if (pointX > 200 && pointX < 303) return;
            else if (pointX < 200 && pointX > 73) pointX = 73;
            RoteteVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
            transform.eulerAngles = RoteteVelocity;
        }
        if (upperAngle >= 1)
        {
            upperAngle = 0.9999f;
        }
        PlayerAnimControl.instance.AnimationAngleWork(upperAngle);
    }
    float CameraReset(Camera c)
    {
        RaycastHit hit;
        float Value = 0;
        var ray = c.ViewportPointToRay(new Vector3(0.5f, 0.5f, -3));
        if (Physics.Linecast(ray.origin, target.position, out hit, ~target.GetComponent<PlayerMovement>().playerLayer))
        {
            for (int i = 0; i < exceptLayerNum.Length; i++) 
            {
                if (hit.collider.gameObject.layer == exceptLayerNum[i])
                { 
                    return Value;
                }
            }

            if (hit.collider.gameObject.CompareTag("PlayerAttack"))
            {
                return Value;

            }
            Value = 1;
        }
        else Value = 0;

        return Value;
    }

    public void CameraSensitivity(float Rot)
    {
        cameraRotSpeed = Rot;
    }
}
