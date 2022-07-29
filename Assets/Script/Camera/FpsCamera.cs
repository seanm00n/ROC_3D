using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [Header("Camera Position in Fps Mode")]
    public Transform head; // Normal Camera Pos in FpsMode.
    public Transform head_2; // Camera Pos in FpsMode when player sits.

    void Start()
    {
        playerMovement = Player.instance.movement;
    }

    void Update()
    {
        var currentHeadPos = head.position;
        
        if (head_2 && playerMovement.speed == playerMovement.slowspeed) // playerSpeed is slow when player sit.
        {
            currentHeadPos = head_2.position;
        }

        transform.position = currentHeadPos;
    }
}
