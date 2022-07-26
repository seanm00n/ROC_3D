using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class PlayerAnimationController : MonoBehaviour
{
    [FormerlySerializedAs("Hp_Text")] public TextMeshProUGUI hpText;
    [FormerlySerializedAs("HpBar")] public Slider hpBar;
    [FormerlySerializedAs("DamageCoolTime")] public float damageCoolTime = 0.5f;
    [FormerlySerializedAs("Hp")] public float hp = 100;
    public bool hit;
    public enum AnimationState
    {
        Normal, Jump, Air
    }

    [FormerlySerializedAs("State")] public AnimationState state;

    PlayerMovement playerMovement;

    [FormerlySerializedAs("playerAnimator")] [FormerlySerializedAs("Player")] public Animator animator;
    public static PlayerAnimationController instance;

    public UnityEvent OnDeath;
    public void HpRefresh()
    {
        if ((int)(hpBar.value - hp) != 0)
        {
            if (hpBar.value < hp)
                hpBar.value += 40 * Time.deltaTime;
            else if (hpBar.value > hp)
                hpBar.value -= 40 *Time.deltaTime;
        }

        hpText.text = hp.ToString() + "/" + 100.ToString();
    }
    
    public void Hit(float damage)
    {
        if (hit == false && hp != 0)
        {
            hit = true;
            if (hp > 0)
            {
                hp -= damage;
            }
            if (hp < 0)
            {
                hp = 0;
            }
            if(hp == 0)
            {
                hit = false;
                OnDeath.Invoke();
                animator.SetTrigger("Death");

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            animator.SetTrigger("Damaged");
            StartCoroutine(PlayerDamageRotator());
        }
    }
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        if(!instance) instance = gameObject.GetComponent<PlayerAnimationController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        HpRefresh();
        animator.SetBool("Onground", playerMovement.isJumping);
        if (CameraManager.fpsMode == true)
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
    // Update is called once per frame
    public void waitngJump()
    {
        animator.SetTrigger("JumpWaiting");
        playerMovement.DoJump = false;
    }
    public void air()
    {
        if (state == AnimationState.Jump || state == AnimationState.Air)
        {
            animator.ResetTrigger("OnAir");
            return;
        }
        animator.SetTrigger("OnAir");
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
        if (state == AnimationState.Jump)
            return;
        animator.ResetTrigger("OnAir");
        animator.SetTrigger("Jump");
    }

    public void Attack()
    {
        if (CameraManager.fpsMode == false)
        {
            animator.SetBool("OnAttack",true);
            animator.SetTrigger("Attack");
        }
    }
    public void AttackEnd()
    {
        animator.SetBool("OnAttack", false);
    }

    public void SetAngle(float angle)
    {
        animator.SetFloat("Angle", angle);
    }

    IEnumerator PlayerDamageRotator()
    {
        yield return new WaitForSeconds(damageCoolTime);
        hit = false;
    }
}
