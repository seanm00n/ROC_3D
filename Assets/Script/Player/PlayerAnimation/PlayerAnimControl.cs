using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    public enum AnimationState
    {
        Normal, Jump
    }

    public AnimationState State;

    PlayerMovement P;

    public Animator Player;
    public static PlayerAnimControl instance;

    void Awake()
    {
        Player = GetComponent<Animator>();
        if(!instance) instance = gameObject.GetComponent<PlayerAnimControl>();
        P = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Player.SetBool("Onground", P.isJumping);
    }
    // Update is called once per frame
    public void waitngJump()
    {
        Player.SetTrigger("JumpWaiting");
        P.DoJump = false;
    }
    public void air()
    {
        Player.SetTrigger("OnAir");
    }

    public void AnimationWork(Vector2 move)
    {
        Player.SetFloat("Horizontal",move.x);
        Player.SetFloat("Vertical", move.y);
    }
    public void Jump()
    {
        if (State == AnimationState.Jump)
            return;
        Player.ResetTrigger("OnAir");
        Player.SetTrigger("Jump");
    }

    public void AnimationAngleWork(float angle)
    {
        Player.SetFloat("Angle", angle);
    }

}
