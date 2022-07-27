using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Header("FPS Mode Options")]
    // handheld object that is always static to camera
    public GameObject bookVisibleFps;

    [Space]
    [Header("Player Head Rotation")]
    public float upperAngle;

    [Space]

    public Transform target;
    public int targetNum = 1;
    public Transform targetPosition;

    public float cameraRotSpeed = 200;
    Vector3 cameraMoveVelocity;

    [Space]
    [Header("AngleSetting")]
    private float currentAngle;
    private float earlyTopAngle;
    private float earlyDownAngle;
    public float topAngle = 50;

    public float downAngle;
    public float maxAngle = 80;
    public float minAngle = -40f;

    [Space]
    
    public int[] exceptLayerNum;
    public float clampedPos; // Block camera collision with wall.

    // Singleton stuff
    public static CameraManager instance { get; private set; }
    public static bool fpsMode { get; private set; }
    
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        targetPosition = Player.instance.cameraPos.middleViewCamera.transform;
        target = Player.instance.transform;
        earlyTopAngle = topAngle;
        earlyDownAngle = downAngle;
        
        // semi-disable clipping
        Camera.main.nearClipPlane = 0.01f;
    }

    private void Update()
    {
        // Prevent exception from method calls below
        if (!target) return;
        
        MoveCamera();
        RotateCamera();

        // Toggle fps mode (first-person-perspective?)
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (bookVisibleFps)
            {
                fpsMode = !fpsMode;
                // Set visibility of held book
                bookVisibleFps.SetActive(fpsMode);
            }
        }
    }
    
    // This method not only moves the camera, but also rotates player's angles
    private void MoveCamera()
    {
        var position = targetPosition.position;
        var destination = new Vector3(position.x, position.y, position.z);
            
        transform.position = destination;

        var pointY = target.eulerAngles.y + Input.GetAxisRaw("Mouse X") * cameraRotSpeed * Time.deltaTime;
            
        // Rotate player
        target.eulerAngles = new Vector3(target.rotation.x, pointY, target.rotation.z);
    }

    // This method sets the player's Y position when looking at the sky or ground
    void RotateCamera()
    {
        float pointX = transform.eulerAngles.x - Input.GetAxisRaw("Mouse Y") * cameraRotSpeed * Time.deltaTime;
        if (pointX < 300 && pointX > maxAngle) pointX = maxAngle;
        Vector3 rotateVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);

        if (minAngle >= 0)
        {
            upperAngle = ((pointX - (minAngle) + 1) / (maxAngle - minAngle + 1));
        }
        else
        {
            float max = maxAngle - minAngle + 1;
            if (transform.localEulerAngles.x > 300 || transform.localEulerAngles.x == 0)
            {
                float unsanitizedUpperAngle = ((maxAngle) + 1) + (360-transform.localEulerAngles.x);
                upperAngle = (unsanitizedUpperAngle / max);
            }
            else if (transform.localEulerAngles.x > 0)
            {
                float unsanitizedUpperAngle = (maxAngle - transform.localEulerAngles.x) + 1;
                upperAngle = (unsanitizedUpperAngle / max);
            }
        }

        if (!fpsMode)
        {
            float pointX_ = 0;
            if (pointX > 300 && downAngle < 0)
            {
                pointX_ = pointX - 360;

            }
            if (targetNum == 1)
            {
                if (clampedPos != 2)
                    clampedPos = ResetCamera(Player.instance.cameraPos.middleViewCamera);

                targetPosition = clampedPos == 0 ? Player.instance.cameraPos.targetPositions[0] : Player.instance.cameraPos.targetPositions[3];

                if (pointX_ != 0)
                {
                    if (pointX_ < downAngle) targetNum = 2;
                }
                else 
                {
                    if (transform.localEulerAngles.x > topAngle) targetNum = 3; else if (pointX < downAngle) targetNum = 2;
                }

                transform.eulerAngles = rotateVelocity;
            }
            else if (targetNum == 2)
            {
                if (clampedPos != 2)
                    clampedPos = ResetCamera(Player.instance.cameraPos.downViewCamera);

                if (clampedPos == 0) targetPosition = Player.instance.cameraPos.targetPositions[1]; else { targetPosition = Player.instance.cameraPos.targetPositions[4]; }
                currentAngle = transform.eulerAngles.x;

                
                if (minAngle + 360 > (pointX)) pointX = -40;
                rotateVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
                

                transform.eulerAngles = rotateVelocity;
                if (downAngle < pointX_ && pointX_ != 0) targetNum = 1;

            }
            else if (targetNum == 3)
            {
                if (clampedPos != 2)
                    clampedPos = ResetCamera(Player.instance.cameraPos.topViewCamera);

                targetPosition = clampedPos == 0 ? Player.instance.cameraPos.targetPositions[2] : Player.instance.cameraPos.targetPositions[5];
                
                currentAngle = transform.eulerAngles.x;
                rotateVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
                transform.eulerAngles = rotateVelocity;
                
                if (30 > transform.eulerAngles.x) targetNum = 1;
            }
        }
        else
        {
            targetPosition = Player.instance.cameraPos.targetPositions[6];
            
            switch (pointX)
            {
                // Limit looking up
                case > 200 and < 303:
                    return;
                
                // Limit looking down
                case < 200 and > 73:
                    // pointX = 73;
                    return;
            }

            rotateVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
            transform.eulerAngles = rotateVelocity;
        }
        
        // Cap below 1, or the animations will break
        if (upperAngle >= 1)
        {
            upperAngle = 0.9999f;
        }
        
        Player.instance.animationController.SetAngle(upperAngle);
    }
    float ResetCamera(Camera cam)
    {
        var newClampedPos = 0;
        var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, -3));
        
        if (Physics.Linecast(ray.origin, target.position, out var hit, ~target.GetComponent<PlayerMovement>().playerLayer))
        {
            for (int i = 0; i < exceptLayerNum.Length; i++) 
            {
                if (hit.collider.gameObject.layer == exceptLayerNum[i])
                { 
                    return newClampedPos;
                }
            }

            if (hit.collider.gameObject.CompareTag("PlayerAttack"))
            {
                return newClampedPos;

            }
            newClampedPos = 1;
        }
        else newClampedPos = 0;

        return newClampedPos;
    }

    public void SetCameraSensitivity(float rot)
    {
        cameraRotSpeed = rot;
    }
}
