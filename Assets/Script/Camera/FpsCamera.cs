using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    PlayerMovement playerMovement;

    [Header("Camera Position in Fps Mode")]
    public Transform head; // Nomarl Camera Pos in FpsMode.
    public Transform head_2; // Camera Pos in FpsMode when player sit.

    void Start()
    {
        playerMovement = Player.instance.movement;
    }

    // Update is called once per frame
    void Update()
    {
        if (head_2 && playerMovement.speed == playerMovement.Slowspeed) // playerSpeed is slow when player sit.
        {
            transform.position = new Vector3(head_2.position.x, head_2.position.y, head_2.position.z);
        }
        else
        transform.position = new Vector3(head.position.x, head.position.y, head.position.z);
    }
}
