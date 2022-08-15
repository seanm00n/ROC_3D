using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private Animator animator;
    private bool work = true;
    private static bool workOther = false;

    public Vector3 teleportPos;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && work) 
        {
            if (!workOther)
            {
                work = false;
                animator.SetBool("Work", true);
            }
            else
            {
                workOther = false;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !work)
        {
            work = true;
            animator.SetBool("Work", false);
        }
    }

    public void PortalTeleport()
    {
        workOther = true;
        Player.instance.transform.position = teleportPos;
    }
}
