using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAnimControl : MonoBehaviour
{
    public TextMeshProUGUI Hp_Text;
    public Slider HpBar;
    public float DamageCoolTime = 0.5f;
    public float Hp = 100;
    public bool hit = false;
    public enum AnimationState
    {
        Normal, Jump, Air
    }

    public AnimationState State;

    PlayerMovement P;

    public Animator Player;
    public static PlayerAnimControl instance;


    public void HpRefresh()
    {
        HpBar.value = Hp;
        Hp_Text.text = Hp.ToString() + "/" + 100.ToString();
    }
    public void Hit(float damage)
    {
        if (hit == false)
        {
            hit = true;
            if (Hp > 0)
            {
                Hp -= damage;
            }
            if (Hp < 0)
            {
                Hp = 0;
            }
            Player.SetTrigger("Damaged");
            StartCoroutine(playerDamageRotator());
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
        HpRefresh();
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

    IEnumerator playerDamageRotator()
    {
        yield return new WaitForSeconds(DamageCoolTime);
        hit = false;
    }
}
