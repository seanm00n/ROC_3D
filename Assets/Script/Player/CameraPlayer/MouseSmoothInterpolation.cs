using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseSmoothInterpolation : MonoBehaviour
{
    private readonly Vector3 top = new(0.12f,-1.41f,-1.82f); // Interpolate Camera Move (TopView).
    private readonly Vector3 bottom = new(0.12f,1.46f,-0.69f); // Interpolate Camera Move (DownView).
    private Vector3 original;

    private Transform cam; // camera transform.

    private void Awake()
    {
        original = transform.localPosition;
    }
    
    private void Start()
    {
        cam = Camera.main.transform;
    }
    
    private void Update()
    {
        // Camera movement is more smooth when cameraPos is Rotated.
        if (CameraManager.instance.targetNum == 1) // It is work when cameraview is middleview.
        {
            var localPosition = transform.localPosition;
            if (Input.GetAxisRaw("Mouse Y") > 0.3f)
            {
                transform.localPosition = InterpolateVector(
                    (cam.localEulerAngles.x is > 300 and < 360) ? top : original,
                    localPosition);
            }
            else if (Input.GetAxisRaw("Mouse Y") < -0.3f)
            {
                // switch scope below works with the case stated below:
                // condition => value,
                // condition2 => value2, etc...
                // the _ condition here means "default"
                transform.localPosition = cam.localEulerAngles.x switch
                {
                    > 300 and < 355 => InterpolateVector(original, localPosition, -360),
                    > 15 => InterpolateVector(bottom, localPosition),
                    _ => InterpolateVector(original, localPosition)
                };
            }
        }
    }

    private static Vector3 InterpolateVector(Vector3 targetPos, Vector3 localPos, float? localPosOffset = null)
    {
        // Sanitize nullable parameter
        if (localPosOffset is not { } offset) offset = 0;
        var offsetVector = new Vector3(offset, offset, offset);
        
        return (targetPos - (localPos + offsetVector)) * (10 * Time.deltaTime); // 10 is speed
    }
}
