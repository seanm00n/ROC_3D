using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCameraPosition : MonoBehaviour
{
    [Header("Extra Camera")]
    public Camera middleViewCamera; // Middle sight of view.
    public Camera downViewCamera; // Down sight of view.
    public Camera topViewCamera; // Top sight of view.

    [Space]
    // 0: middle view
    // 1: top view
    // 2: down view
    // 3: middle view but closer to wall
    // 4: top view but closer to wall
    // 5: down view but closer to wall
    // 6: fpsmode cam
    public Transform[] targetPositions;

}
