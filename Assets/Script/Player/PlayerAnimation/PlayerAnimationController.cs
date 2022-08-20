using ROC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Serialization;
using TMPro;

public class PlayerAnimationController : MonoBehaviour
{
    public enum AnimationState
    {
        Normal, Jump, Air, Attack, Die, Spin
    }

    [Header("Player Animation State")]
    public AnimationState state;
    
    [Space]
    [Header("Player Animator")]
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(AnimatorHashID.OngroundID, Player.instance.movement.isJumping);

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

        if(state == AnimationState.Spin && !Player.instance.movement.isSpin)
        {
            Player.instance.movement.isSpin = true;
        }

        if (state != AnimationState.Spin && Player.instance.movement.isSpin)
        {
            Player.instance.movement.isSpin = false;
        }
    }

    public void waitngJump()
    {
        animator.SetTrigger(AnimatorHashID.JumpWaitingID);
        Player.instance.movement.doJump = false; // Block jump when "JumpWaiting" animation is work.
    }

    public void Air()
    {
        if (state == AnimationState.Jump || state == AnimationState.Air)
        {
            animator.ResetTrigger(AnimatorHashID.OnAirID);
            return;
        }
        animator.SetTrigger(AnimatorHashID.OnAirID); // End Jump, fall.
    }

    public void AnimationWork(Vector2 move)
    {
        if (CameraManager.fpsMode == false)
        {
            animator.SetFloat(AnimatorHashID.HorizontalID, move.x);
            animator.SetFloat(AnimatorHashID.VerticalID, move.y);
        }
    }
    public void Jump()
    {
        animator.ResetTrigger(AnimatorHashID.JumpWaitingID);

        if (state == AnimationState.Jump) // Block repetitive animation.
            return;

        animator.ResetTrigger(AnimatorHashID.OnAirID);
        animator.SetTrigger(AnimatorHashID.JumpID);
    }

    public void Attack()
    {
        if (!CameraManager.fpsMode)
        {
            animator.SetBool(AnimatorHashID.OnAttackID, true); //OnAttack help that player remain attack state.
            animator.SetTrigger(AnimatorHashID.AttackID);
        }
    }
    public void AttackEnd()
    {
        animator.SetBool(AnimatorHashID.OnAttackID, false);
    }

    public void FrontSkill()
    {
        animator.SetTrigger(AnimatorHashID.FrontAttackID);
    }
    public void HandUpSkill()
    {
        animator.SetTrigger(AnimatorHashID.HandUpID);
    }

    public void SetAngle(float angle) // Player Head Rotate.
    {
        animator.SetFloat(AnimatorHashID.AngleID, angle);
    }

    public void OnDamaged()
    {
        animator.SetTrigger(AnimatorHashID.DamagedID);
    }

    public void OnDeath()
    {
        animator.SetTrigger(AnimatorHashID.DeathID);
    }
    public void OnRebirth()
    {
        animator.SetTrigger(AnimatorHashID.RebirthID);
    }

    public void OnSpin()
    {
        if (state == AnimationState.Normal || state == AnimationState.Jump || state == AnimationState.Air)
        {
            animator.SetTrigger(AnimatorHashID.SpinID);
        }
    }
}
