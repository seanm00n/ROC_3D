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

    public int price = 0;

    [Header("Price")]
    List<DataCheck> allPrice = new List<DataCheck>(); 

    [Header("ability")]
    public int hp = 0;
    public int mp = 0;
    public bool pet = false;
    public int getMoreGold = 0;
    public int getMoreBone = 0;

    [Header("UI")]
    public TextMeshProUGUI priceView;
    public TextMeshProUGUI myValue; // using text.

    public GameObject AlreadyBuy;
    public GameObject Buybutton;

    private PlayerSaveData playerSaveData;

    private int maxHpValue = 400;
    private int maxMpValue = 60;

    private int maxGoldValue = 100;
    private int maxBoneValue = 20;

    private int maxPetDamage = 300;

    private int refundBoneValue;

    private void Start()
    {
        if(!myValue)
            myValue = GetComponent<TextMeshProUGUI>();
        if (priceView) priceView.text = price.ToString();

        if (price != 0)
            foreach (DataCheck item in allPrice)
            {
                if(item != this)
                allPrice.Add(this);
            }
    }

    // Update is called once per frame
    private void Update()
    {
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

        Player.maxHp = playerSaveData.maxHP;
        Player.maxHp = playerSaveData.maxMP;
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
        int petLv = 0;

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

                price = 200;
                priceView.text = price.ToString();
            }
            else
            {
                petLv = (playerSaveData.petDamage / 60);
                myValue.text = "   LV. " + petLv;

                int changedPrice = (petLv + 1) * price;
                priceView.text = changedPrice.ToString();
            }
        }

        ///////////// Refund Value Calculation ///////////////////
        int hpPrice = 0;
        int mpPrice = 0;
        int goldPrice = 0;
        int bonePrice = 0;
        int petPrice = 0;

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
            else if (item.pet)
            {
                petPrice = petLv * item.price;
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

                if (playerSaveData.petDamage == 0) { playerSaveData.petDamage = 60;}
                else if (playerSaveData.petDamage == 60) { playerSaveData.petDamage = 150;}
                else if (playerSaveData.petDamage == 150) { playerSaveData.petDamage = 300;}
            }

            SaveManager.Save("PlayerData", playerSaveData);
        }
    }

    public void Refund()
    {
        playerSaveData = new PlayerSaveData();
        playerSaveData.bone += refundBoneValue; 
        SaveManager.Save("PlayerData", playerSaveData);
    }

}
