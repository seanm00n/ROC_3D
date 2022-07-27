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

    // TODO: Move these somewhere?
    private static readonly int OnAttackID = Animator.StringToHash("OnAttack");
    private static readonly int AngleID = Animator.StringToHash("Angle");
    private static readonly int AttackID = Animator.StringToHash("Attack");
    private static readonly int JumpID = Animator.StringToHash("Jump");
    private static readonly int OnAirID = Animator.StringToHash("OnAir");
    private static readonly int JumpWaitingID = Animator.StringToHash("JumpWaiting");
    private static readonly int VerticalID = Animator.StringToHash("Vertical");
    private static readonly int HorizontalID = Animator.StringToHash("Horizontal");

    private void Awake()
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

    public void WaitngJump() // TODO: Rename it to "WaitingJump"
    {
        animator.SetTrigger(JumpWaitingID);
        Player.instance.movement.doJump = false; // Block jump when "JumpWaiting" animation is work.
    }

    public void Air()
    {
        if (state == AnimationState.Jump || state == AnimationState.Air)
        {
            animator.ResetTrigger(OnAirID);
            return;
        }
        animator.SetTrigger(OnAirID); // End Jump, fall.
    }

    public void AnimationWork(Vector2 move)
    {
        if (CameraManager.fpsMode == false)
        {
            animator.SetFloat(HorizontalID, move.x);
            animator.SetFloat(VerticalID, move.y);
        }
    }
    public void Jump()
    {
        animator.ResetTrigger(JumpWaitingID);

        if (state == AnimationState.Jump) // Block repetitive animation.
            return;

        animator.ResetTrigger(OnAirID);
        animator.SetTrigger(JumpID);
    }

    public void Attack()
    {
        if (!CameraManager.fpsMode)
        {
            animator.SetBool(OnAttackID,true); //OnAttack help that player remain attack state.
            animator.SetTrigger(AttackID);
        }
    }
    public void AttackEnd()
    {
        animator.SetBool(OnAttackID, false);
    }

    public void SetAngle(float angle) // Player Head Rotate.
    {
        animator.SetFloat(AngleID, angle);
    }

}
