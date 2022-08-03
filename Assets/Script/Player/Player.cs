using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IBattle
{
    [Space]
    [Header("Player Status")]
    public int hp = 100;
    public int mp = 20;
    
    [Space]
    [Header("Case : Player is damaged")]
    public bool hit; // Hit show that player is damaged.
    public float damageCoolTime = 0.5f; // Waiting time about next hit.

    [Space]
    [Header("Case : Player is die")]
    public UnityEvent onDeath;

    [Space]
    [Header("Prefabs")]
    public GameObject uiPrefab;
    public GameObject cameraPrefab;
    public GameObject gameoverPrefab;

    [Space]
    [Header("Player View")]
    public ViewCameraPosition cameraPos;

    [Space]
    [Header("Player Feature")]
    public PlayerAnimationController animationController;
    public PlayerAttackSkill playerAttackSkill;
    public PlayerMovement movement;
    public UIController ui;

    //Single Tone Stuff//
    public static Player instance;
    public GameObject playerCamera;

    // Absolute Value
    public static int maxHp = 100;
    public static int maxMp = 20;

    private void Awake()
    {
        if (!instance)
            instance = this;

        animationController = GetComponent<PlayerAnimationController>();
        playerAttackSkill = GetComponent<PlayerAttackSkill>();
        movement = GetComponent<PlayerMovement>();

        if (!ui)
        {
            GameObject instance = Instantiate(uiPrefab);
            ui = instance.GetComponent<UIController>();

            PauseGame.instance = ui.setting.GetComponent<PauseGame>();
        }
        if (instance && !instance.playerCamera)
            playerCamera = Instantiate(cameraPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        HpRefresh(); // Always hp value is changing.
        MpRefresh(); // Always mp value is changing.

        if (Input.GetKeyDown(KeyCode.K))
        {
            onDeath.Invoke();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    public void HpRefresh()
    {
        if ((int)(ui.hpBar.value - hp) != 0) // Approximately hpBar.value = hp
        {
            if (ui.hpBar.value < hp)
                ui.hpBar.value += 40 * Time.deltaTime; // 40 = Rise Speed
            else if (ui.hpBar.value > hp)
                ui.hpBar.value -= 40 * Time.deltaTime; // 40 = Contractible Speed
        }
        ui.hpText.text = hp.ToString() + "/" + 100.ToString(); // Hp value is marked by hpText.text.
    }

    public void MpRefresh() // Same
    {
        if ((int)(ui.mpBar.value - mp) != 0 || (int)(ui.mpBar.value) != mp)
        {
            if (ui.mpBar.value < mp)
                ui.mpBar.value += 10 * Time.deltaTime;
            else if (ui.mpBar.value > mp)
                ui.mpBar.value -= 10 * Time.deltaTime;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void Hit(int damage)
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
            if (hp == 0)
            {
                hit = false;
                onDeath.Invoke();
                animationController.animator.SetTrigger("Death");

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            animationController.animator.SetTrigger("Damaged");
            StartCoroutine(PlayerDamageRotator());
        }
    }

    public void Die()
    {
        Camera.main.gameObject.SetActive(false);
        ui.gameObject.SetActive(false);
        GameObject gameover = Instantiate(gameoverPrefab);
    }

    IEnumerator PlayerDamageRotator()
    {
        yield return new WaitForSeconds(damageCoolTime); // Wait as much as "damageCoolTime".
        hit = false;
    }
}
