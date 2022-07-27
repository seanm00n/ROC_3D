using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class PlayerAnimationController : MonoBehaviour
{
    public enum AnimationState
    {
        Normal, Jump, Air
    }

    [Header("Player Animation State")]
    public AnimationState state;
    
    [Space]
    [Header("Player Animator")]
    public Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("Onground", Player.instance.movement.isJumping);

        if (CameraManager.fpsMode == true) // Fps mode don't need animation. 
        {
            animator.Rebind();
            animator.enabled = false;    
        }
        else
        {
            animator.enabled = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            AttackEnd();
        }
    }

    public void WaitngJump()
    {
        animator.SetTrigger("JumpWaiting");
        Player.instance.movement.doJump = false; // Block jump when "JumpWaiting" animation is work.
    }

    public void Air()
    {
        if (state == AnimationState.Jump || state == AnimationState.Air)
        {
            animator.ResetTrigger("OnAir");
            return;
        }
        animator.SetTrigger("OnAir"); // End Jump, fall.
    }

    public void AnimationWork(Vector2 move)
    {
        if (CameraManager.fpsMode == false)
        {
            animator.SetFloat("Horizontal", move.x);
            animator.SetFloat("Vertical", move.y);
        }
    }
    public void Jump()
    {
        animator.ResetTrigger("JumpWaiting");

        if (state == AnimationState.Jump) // Block repetitive animation.
            return;

        animator.ResetTrigger("OnAir");
        animator.SetTrigger("Jump");
    }

    public void Attack()
    {
        if (CameraManager.fpsMode == false)
        {
            animator.SetBool("OnAttack",true); //OnAttack help that player remain attack state.
            animator.SetTrigger("Attack");
        }
    }
    public void AttackEnd()
    {
        animator.SetBool("OnAttack", false);
    }

    public void SetAngle(float angle) // Player Head Rotate.
    {
        animator.SetFloat("Angle", angle);
    }

}
