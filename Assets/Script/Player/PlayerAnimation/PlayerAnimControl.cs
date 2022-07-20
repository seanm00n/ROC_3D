using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    public float Hp = 100;
    public enum AnimationState
    {
        Normal, Jump, Air
    }

    public AnimationState State;

    PlayerMovement P;

    public Animator Player;
    public static PlayerAnimControl instance;

    public GameObject hitEffect;

    public void Hit(float damage)
    {
        if(Hp > 0)
        {
            Hp -= damage;
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }
        if(Hp < 0)
        {
            Hp = 0;
        }
    }
    void Awake()
    {
        Player = GetComponent<Animator>();
        if(!instance) instance = gameObject.GetComponent<PlayerAnimControl>();
        P = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Player.SetBool("Onground", P.isJumping);
        if (Camera_manager.fpsMode == true)
        {
            Player.Rebind();
            Player.enabled = false;    
        }
        else
        {
            Player.enabled = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            AttackEnd();
        }
    }
    // Update is called once per frame
    public void waitngJump()
    {
        Player.SetTrigger("JumpWaiting");
        P.DoJump = false;
    }
    public void air()
    {
        if (State == AnimationState.Jump || State == AnimationState.Air)
        {
            Player.ResetTrigger("OnAir");
            return;
        }
        Player.SetTrigger("OnAir");
    }

    public void AnimationWork(Vector2 move)
    {
        if (Camera_manager.fpsMode == false)
        {
            Player.SetFloat("Horizontal", move.x);
            Player.SetFloat("Vertical", move.y);
        }
    }
    public void Jump()
    {
        Player.ResetTrigger("JumpWaiting");
        if (State == AnimationState.Jump)
            return;
        Player.ResetTrigger("OnAir");
        Player.SetTrigger("Jump");
    }

    public void Attack()
    {
        if (Camera_manager.fpsMode == false)
        {
            Player.SetBool("OnAttack",true);
            Player.SetTrigger("Attack");
        }
    }
    public void AttackEnd()
    {
        Player.SetBool("OnAttack", false);
    }

    public void AnimationAngleWork(float angle)
    {
        Player.SetFloat("Angle", angle);
    }

}
