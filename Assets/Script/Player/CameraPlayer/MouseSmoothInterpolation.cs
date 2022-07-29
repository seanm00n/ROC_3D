using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseSmoothInterpolation : MonoBehaviour
{
    private readonly Vector3 top = new(0.12f, -1.41f, -1.82f); // Interpolate Camera Move (TopView).
    private readonly Vector3 bottom = new(0.12f, 1.46f, -0.69f); // Interpolate Camera Move (DownView).
    private Vector3 original;

    private Transform cam; // camera transform.

    private void Awake()
    {
        original = transform.localPosition; // Original middle camera position.
    }
    private void Start()
    {
        cam = Camera.main.transform; // Main camera position.
    }
    private void Update()
    {
        // This code's purpose is that Camera move is more slowly.
        if (CameraManager.instance.targetNum == 1)
        {
            if (Input.GetAxisRaw("Mouse Y") > 0.1f)
            {
                LocalPosition((cam.localEulerAngles.x > 300 && cam.localEulerAngles.x < 350) ? top : original); // Camera move up slowly.
            }
            else if (Input.GetAxisRaw("Mouse Y") < -0.1f)
            {
                LocalPosition((!(cam.localEulerAngles.x > 300 && cam.localEulerAngles.x < 360) && cam.localEulerAngles.x > 10) ? bottom : original); // Camera move down slowly.
            }
        }
    }
    public void LocalPosition(Vector3 pos) // Camera position is moving in real time.
    {
        float speed = 10f * Time.deltaTime; // Camera move speed
        Vector3 localPos = transform.localPosition;
        transform.localPosition += (pos - localPos) * speed;
    }
}
