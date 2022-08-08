using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSensitivitySetting : MonoBehaviour, ISetting
{
    // Used in inspector
    
    public void ApplyButton(float num)
    {
        CameraManager.cameraRotSpeed = (float)num;
    }
}
