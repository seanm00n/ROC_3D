using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ROC;
public class DataCheck : MonoBehaviour
{
    public bool gold = false;
    public bool bone = false;
    public bool petPlayer = false;
    public bool priceTurret = false;

    public int price = 0;

    [Header("Price")]
    private static List<DataCheck> allPrice = new List<DataCheck>(); 

    [Header("ability")]
    public int hp = 0;
    public int mp = 0;
    public bool pet = false;
    public int getMoreGold = 0;
    public int getMoreBone = 0;

    [Header("UI")]
    public TextMeshProUGUI priceView;
    public TextMeshProUGUI myValue; // using text.
    public Text myValueLegacy; // using text.

    public GameObject AlreadyBuy;
    public GameObject Buybutton;

    private PlayerSaveData playerSaveData;
    public StructureSpawn_Test structure;

    private int maxHpValue = 400;
    private int maxMpValue = 60;

    private int maxGoldValue = 100;
    private int maxBoneValue = 20;

    private int maxPetDamage = 300;

    private static int petPrice = 0;

    [SerializeField]public int refundBoneValue;
    public int[] petDamage;
    private void Start()
    {
        try
        {
            playerSaveData = SaveManager.Load<PlayerSaveData>("PlayerData");
        }
        catch
        {
            playerSaveData = new PlayerSaveData();
        }
        if (Player.instance && Player.instance.ui)
        {
            Player.instance.ui.hpBar.maxValue = playerSaveData.maxHP;
            Player.instance.ui.mpBar.maxValue = playerSaveData.maxMP;

            Player.instance.ui.hpBar.value = playerSaveData.maxHP;
            Player.instance.ui.mpBar.value = playerSaveData.maxMP;
        }
        if (!myValue)
            myValue = GetComponent<TextMeshProUGUI>();
        if (!myValueLegacy)
            myValueLegacy = GetComponent<Text>();
        if (priceView) priceView.text = price.ToString();

        if (price != 0)
        {
            if (allPrice.Count == 0) allPrice.Add(this);
            else
            {
                foreach (DataCheck item in allPrice)
                {
                    if (item != this)
                    {
                        allPrice.Add(this);
                        break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!structure && Player.instance && Player.instance.playerCamera) structure = Player.instance.playerCamera.GetComponent<StructureSpawn_Test>();
        try
        {
            playerSaveData = SaveManager.Load<PlayerSaveData>("PlayerData");
        }
        catch
        {
            playerSaveData = new PlayerSaveData();
        }
        if (gold)
            myValue.text = ": " + PlayerSaveData.gold;
        if (bone)
            myValue.text = ": " + playerSaveData.bone;
        if (priceTurret && structure && 0 <= structure.selectNumber && 4 > structure.selectNumber && structure.turretInstallPrice.Length > 0)
        {
            int value = structure.turretInstallPrice[structure.selectNumber];
            myValueLegacy.text = "설치 비용 : " + value; 
        }
        
        Player.maxHp = playerSaveData.maxHP;
        Player.maxMp = playerSaveData.maxMP;
        SkillAttack.petDamage = playerSaveData.petDamage;

        if (AlreadyBuy && Buybutton)
        {
            #region
            if ((hp != 0 && playerSaveData.maxHP >= maxHpValue) || (mp != 0 && playerSaveData.maxMP >= maxMpValue) || (getMoreGold != 0 && playerSaveData.getMoreGold >= maxGoldValue)
              || (getMoreBone != 0 && playerSaveData.getMoreBone >= maxBoneValue) || ((pet && (playerSaveData.pet || playerSaveData.petSpecial)) && (playerSaveData.petDamage >= maxPetDamage)))
            {
                AlreadyBuy.SetActive(true);
                Buybutton.SetActive(false);
            }
            else
            {
                AlreadyBuy.SetActive(false);
                Buybutton.SetActive(true);
            }
            #endregion
        }
        if (!playerSaveData.pet && !playerSaveData.petSpecial && petPlayer)
        {
            Destroy(gameObject);
        }

        // Level Calculation /////////////
        int hpLv = ((playerSaveData.maxHP - new PlayerSaveData().maxHP) / 20);
        int mpLv = ((playerSaveData.maxMP - new PlayerSaveData().maxMP) / 3);
        int goldLv = ((playerSaveData.getMoreGold - new PlayerSaveData().getMoreGold) / 20);
        int boneLv = ((playerSaveData.getMoreBone - new PlayerSaveData().getMoreBone) / 4);
        int petLv = (int)(playerSaveData.petDamage / 60f);

        if (hp != 0) myValue.text = "   LV. " + hpLv;
        if (mp != 0) myValue.text = "   LV. " + mpLv;
        if (getMoreGold != 0) myValue.text = "   LV. " + goldLv;
        if (getMoreBone != 0) myValue.text = "   LV. " + boneLv;
        if (pet) 
        {
            if (playerSaveData.petDamage == maxPetDamage)
            {
                petLv = 3;
                myValue.text = "   LV. " + petLv;

                price = 150;
                priceView.text = price.ToString();
            }
            else
            {
                price = 50 * (petLv + 1);
                petLv = (int)(playerSaveData.petDamage / 60f);
                myValue.text = "   LV. " + petLv;
                
                priceView.text = price.ToString();
                
            }
        }

        ///////////// Refund Value Calculation ///////////////////
        int hpPrice = 0;
        int mpPrice = 0;
        int goldPrice = 0;
        int bonePrice = 0;

        foreach (DataCheck item in allPrice)
        {
            if (item.hp != 0)
            {
                hpPrice = hpLv * item.price;
            }
            else if (item.mp != 0)
            {
                mpPrice = mpLv * item.price;
            }
            else if (item.getMoreGold != 0)
            {
                goldPrice = goldLv * item.price;
            }
            else if (item.getMoreBone != 0)
            {
                bonePrice = boneLv * item.price;
            }
            else if (item.pet == true && petDamage.Length > 0)
            {
                if(playerSaveData.petDamage == petDamage[0]) petPrice = 50;
                else if(playerSaveData.petDamage == petDamage[1]) petPrice = 50 + 100;
                else if (playerSaveData.petDamage == petDamage[2]) petPrice = 50 + 100 + 150;
            }
        }
        refundBoneValue = hpPrice + mpPrice + goldPrice + bonePrice + petPrice;
    }

    public void Buy()
    {
        if(price <= playerSaveData.bone) 
        {
            playerSaveData.bone -= price;
            if (hp != 0) 
            {
                if(playerSaveData.maxHP == 0)
                    playerSaveData.maxHP = Player.maxHp + hp;
                else 
                    playerSaveData.maxHP += hp;
            }
            if (mp != 0)
            {
                if (playerSaveData.maxMP == 0)
                    playerSaveData.maxMP = Player.maxMp + mp;
                else 
                    playerSaveData.maxMP += mp;
            }
            if (getMoreGold != 0)
            {
                    playerSaveData.getMoreGold += getMoreGold;
            }
            if (getMoreBone != 0)
            {
                    playerSaveData.getMoreBone += getMoreBone;
            }
            if (pet) 
            {
                if(!playerSaveData.pet)
                    playerSaveData.pet = true;

                if (playerSaveData.petDamage == 0) { playerSaveData.petDamage = petDamage[0];}
                else if (playerSaveData.petDamage == petDamage[0]) { playerSaveData.petDamage = petDamage[1]; }
                else if (playerSaveData.petDamage == petDamage[1]) { playerSaveData.petDamage = petDamage[2]; }
            }

            SaveManager.Save("PlayerData", playerSaveData);
        }
    }

    public void Refund()
    {
        playerSaveData.bone += refundBoneValue;
        int amount = playerSaveData.bone;
        playerSaveData = new PlayerSaveData();
        playerSaveData.bone = amount;
        SaveManager.Save("PlayerData", playerSaveData);
    }

}
