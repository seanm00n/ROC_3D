using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Space]
    [Header("Camera Stop")]
    public bool stop = false;
    public Transform dieCameraTransform; // If player is die.

    [Space]
    [Header("Camera Shake")]
    public float amplitude;
    public float frequency;
    public float duration;
    public float timeRemaining;
    private Vector3 noiseOffset;
    private Vector3 noise;
    private AnimationCurve smoothCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f), new Keyframe(0.2f, 1.0f), new Keyframe(1.0f, 0.0f));

    [Header("FPS Mode Options")]
    // handheld object that is always static to camera
    public GameObject bookVisibleFps;

    [Space]
    [Header("Player Head Rotation")]
    public float upperAngle;

    [Space]

    public Transform target;
    public int targetNum = 1;
    // targetNum
    // 1: middle view but closer to wall
    // 2: top view
    // 3: down view

    public Transform targetPosition;

    public static float cameraRotSpeed = 200;
    Vector3 cameraMoveVelocity;

    [Space]
    [Header("AngleSetting")]
    private float currentAngle;
    private float earlyTopAngle;
    private float earlyDownAngle;
    public float topAngle = 60;

    public float downAngle = -20;
    public float maxAngle = 80;
    public float minAngle = -40f;

    [Space]
    
    public int[] exceptLayerNum; // Exception about clampedPos. 
    public float clampedPos;
    // clampedPos : Block camera collision with wall. 
    // 0 : Normal
    // 1 : When object cover player.
    // 2 : When other stay behind player.

    // Singleton stuff
    public static CameraManager instance { get; private set; }
    public static bool fpsMode;
    
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

        //Shake noise setting
        float rand = 32.0f;
        noiseOffset.x = Random.Range(0.0f, rand);
        noiseOffset.y = Random.Range(0.0f, rand);
        noiseOffset.z = Random.Range(0.0f, rand);
        // semi-disable clipping
        Camera.main.nearClipPlane = 0.01f;
    }

    private void Update()
    {
        // Prevent exception from method calls below
        if (!target) return;

        if (!stop)
        {
            MoveCamera();
            RotateCamera();
        }
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

        if (timeRemaining <= 0)
            return;

        float deltaTime = Time.deltaTime;
        timeRemaining -= deltaTime;
        float noiseOffsetDelta = deltaTime * frequency;

        noiseOffset.x += noiseOffsetDelta;
        noiseOffset.y += noiseOffsetDelta;
        noiseOffset.z += noiseOffsetDelta;

        noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
        noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);
        noise.z = Mathf.PerlinNoise(noiseOffset.z, 2.0f);

        noise -= Vector3.one * 0.5f;
        noise *= amplitude;

        float agePercent = 1.0f - (timeRemaining / duration);
        noise *= smoothCurve.Evaluate(agePercent);
    }
    
    // This method not only moves the camera, but also rotates player's angles
    private void MoveCamera()
    {
        var position = targetPosition.position;
        var destination = new Vector3(position.x, position.y, position.z);
            
        transform.position = destination;
        transform.position += noise;
        var pointY = target.eulerAngles.y + Input.GetAxisRaw("Mouse X") * cameraRotSpeed * Time.deltaTime;
            
        // Rotate player
        target.eulerAngles = new Vector3(target.rotation.x, pointY, target.rotation.z);
    }

    // This method sets the player's Y position when looking at the sky or ground
    #region
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

        if (!fpsMode) // TPS MODE
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

            }
            else if (targetNum == 2)
            {
                if (clampedPos != 2)
                    clampedPos = ResetCamera(Player.instance.cameraPos.downViewCamera);

                if (clampedPos == 0) targetPosition = Player.instance.cameraPos.targetPositions[1]; else { targetPosition = Player.instance.cameraPos.targetPositions[4]; }
                currentAngle = transform.eulerAngles.x;

                
                if (minAngle + 360 > (pointX)) pointX = -40;
                rotateVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
                

                if (downAngle < pointX_ && pointX_ != 0) targetNum = 1;

            }
            else if (targetNum == 3)
            {
                if (clampedPos != 2)
                    clampedPos = ResetCamera(Player.instance.cameraPos.topViewCamera);

                targetPosition = clampedPos == 0 ? Player.instance.cameraPos.targetPositions[2] : Player.instance.cameraPos.targetPositions[5];
                
                currentAngle = transform.eulerAngles.x;
                rotateVelocity = new Vector3(pointX, targetPosition.eulerAngles.y, 0);
                
                if (30 > transform.eulerAngles.x) targetNum = 1;
            }
            transform.eulerAngles = rotateVelocity;
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


        transform.eulerAngles += noise;
        Player.instance.animationController.SetAngle(upperAngle);
    }
    #endregion

    float ResetCamera(Camera cam) // Return to original camera position.
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

    public IEnumerator Shake(float amp, float freq, float dur, float wait)
    {
        yield return new WaitForSeconds(wait);
        float rand = 32.0f;
        noiseOffset.x = Random.Range(0.0f, rand);
        noiseOffset.y = Random.Range(0.0f, rand);
        noiseOffset.z = Random.Range(0.0f, rand);
        amplitude = amp;
        frequency = freq;
        duration = dur;
        timeRemaining += dur;
        if (timeRemaining > dur)
        {
            timeRemaining = dur;
        }
    }
}
