using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySetting : MonoBehaviour, ISetting
{
    public Slider mouseSetting;

    public void Awake()
    {
        if(mouseSetting)
            mouseSetting.value = CameraManager.cameraRotSpeed;
    }
    // Used in inspector
    public void ApplyButton(float num)
    {
        CameraManager.cameraRotSpeed = (float)num;
    }
}
