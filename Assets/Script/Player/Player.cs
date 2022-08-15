using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ROC;

public class PlayerSaveData
{
    public static int turretAmount = 0;
    public static int turretAmountMax = 8;
    public static int gold = 200;
    public static bool goldLock = false;
    public int bone = 500;
    public int maxHP = 200;
    public int maxMP = 30;

    public bool pet = false;
    public bool petSpecial = false;

    public int getMoreGold = 0;
    public int getMoreBone = 0;

    public int petDamage = 0;

    public static List<string> itemList = new List<string>();
}
public class Player : MonoBehaviour, IBattle
{
    [Space]
    [Header("Player Status")]
    public float hp = 100;
    public int mp = 20;
    public bool unbeatable = false;
    public static int theNumberOfDeaths = 0;

    [Space]
    [Header("Case : Player is damaged")]
    public bool hit; // Hit show that player is damaged.
    public float damageCoolTime = 0.5f; // Waiting time about next hit.

    [Space]
    [Header("Case : Player is die")]
    public UnityEvent onDeath;
    private StructureSpawn_Test spawn;
    private ItemInteractionManager itemInteraction;

    [Space]
    [Header("Prefabs")]
    public GameObject uiPrefab;
    public GameObject gameoverPrefab;
    public CameraManager cameraPrefab;

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
    public CameraManager playerCamera;
    public GameObject shopCamera;

    // Absolute Value
    public static int maxHp = 0;
    public static int maxMp = 0;

    private PlayerSaveData playerSaveData;
    
    private bool isRecoverMp; 
    private void Awake()
    {
        if (PlayerSaveData.goldLock)
        {
            PlayerSaveData.goldLock = false;
            PlayerSaveData.turretAmount = 0;
            PlayerSaveData.gold = 200;
            PlayerSaveData.itemList = new List<string>();
        }

            try
        {
            playerSaveData = SaveManager.Load<PlayerSaveData>("PlayerData");
        }
        catch
        {
            playerSaveData = new PlayerSaveData();
        }
        hp = playerSaveData.maxHP;
        mp = playerSaveData.maxMP;
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
        if (!isRecoverMp && !Player.instance.playerAttackSkill.isAttack)
        {
            isRecoverMp = true;
            StartCoroutine(RevertMp());
        } // Mp Recover

        HpRefresh(); // Always hp value is changing.
        MpRefresh(); // Always mp value is changing.

        if (Input.GetKeyDown(KeyCode.K))
        {
            Hit(100);
        }
        if (mp < 0) mp = 0;

    }
    private void StopPlaying(bool stopPlaying)
    {
        playerCamera.stop = stopPlaying;
        unbeatable = stopPlaying;
        movement.enabled = !stopPlaying;
        playerAttackSkill.enabled = !stopPlaying;
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
        ui.hpText.text = hp.ToString() + "/" + maxHp.ToString(); // Hp value is marked by hpText.text.
    }

    public void MpRefresh() // Same
    {
        if ((int)(ui.mpBar.value - mp) != 0 || (int)(ui.mpBar.value) != mp)
        {
            if (ui.mpBar.value < mp)
                ui.mpBar.value += 10 * Time.deltaTime; // 10 = Rise Speed
            else if (ui.mpBar.value > mp)
                ui.mpBar.value -= 10 * Time.deltaTime; // 10 = Contractible Speed
        }
        ui.mpText.text = mp.ToString() + "/" + maxMp.ToString(); // Mp value is marked by mpText.text.
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void Hit(int damage)
    {
        if (unbeatable) return;
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
                animationController.OnDeath();
                CameraManager.fpsMode = false;
                playerCamera.bookVisibleFps.SetActive(false);

                spawn = playerCamera.GetComponent<StructureSpawn_Test>();
                itemInteraction = playerCamera.GetComponent<ItemInteractionManager>();
                spawn.enabled = false;
                itemInteraction.enabled = false;

                if (theNumberOfDeaths == 0 && playerAttackSkill.passiveSkill == PlayerAttackSkill.skill.Angel_2)
                {
                    StopPlaying(true);
                    playerCamera.gameObject.transform.position = playerCamera.dieCameraTransform.position;
                    animationController.OnRebirth();
                    theNumberOfDeaths++;

                    return;
                }

                else
                {
                    onDeath.Invoke();

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    return;
                }
            }
            animationController.OnDamaged();
            StartCoroutine(PlayerDamageRotator());
        }
    }

    public void Die()
    {
        Camera.main.gameObject.SetActive(false);
        ui.gameObject.SetActive(false);
        GameObject gameover = Instantiate(gameoverPrefab);
    }

    public void Rebirth()
    {
        hp = PlayerAttackSkill.passiveSkillData.rebirthHp;
        mp = PlayerAttackSkill.passiveSkillData.rebirthMp;

        StopPlaying(false);
        spawn = playerCamera.GetComponent<StructureSpawn_Test>();
        itemInteraction = playerCamera.GetComponent<ItemInteractionManager>();
        spawn.enabled = true;
        itemInteraction.enabled = true;

    }

    IEnumerator PlayerDamageRotator()
    {
        yield return new WaitForSeconds(damageCoolTime); // Wait as much as "damageCoolTime".
        hit = false;
    }
    private IEnumerator RevertMp()
    {
        yield return new WaitForSeconds(1f);

        if (!Player.instance.unbeatable)
        {
            Player.instance.mp += 2;
            if (Player.instance.mp > Player.maxMp)
                Player.instance.mp = Player.maxMp;
        }

        isRecoverMp = false;
    }
}
