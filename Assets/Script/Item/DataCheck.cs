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

    [Header("ability")]
    public int hp = 0;
    public int mp = 0;
    public bool pet = false;
    public bool getMoreGold = false;
    public bool getMoreBone = false;

    [Header("UI")]
    public TextMeshProUGUI priceView;
    private TextMeshProUGUI myValue;

    public GameObject AlreadyBuy;
    public GameObject Buybutton;

    private PlayerSaveData playerSaveData;

    private void Start()
    {
        myValue = GetComponent<TextMeshProUGUI>();
        if (priceView) priceView.text = price.ToString();

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

        if (playerSaveData.maxHP != 0 && Player.maxHp <= 100) Player.maxHp += playerSaveData.maxHP;
        if (playerSaveData.maxHP != 0 && Player.maxHp <= 20) Player.maxHp += playerSaveData.maxMP;

        if (AlreadyBuy && AlreadyBuy.activeSelf == false)
        if (Buybutton && ((hp != 0 && playerSaveData.maxHP != 0) || (mp != 0 && playerSaveData.maxMP != 0) || (getMoreGold && playerSaveData.getMoreGold)
            || (getMoreBone && playerSaveData.getMoreBone) || (pet && (playerSaveData.pet || playerSaveData.petSpecial))))
        {
            AlreadyBuy.SetActive(true);
            Buybutton.SetActive(false);
        }
        if(!playerSaveData.pet && !playerSaveData.petSpecial && petPlayer)
        {
            Destroy(gameObject);
        }
    }

    public void Buy()
    {
        if(price <= playerSaveData.bone) 
        {
            playerSaveData.bone -= price;
            if (hp != 0) playerSaveData.maxHP = 150;
            if (mp != 0) playerSaveData.maxMP = 30;
            if (getMoreGold) playerSaveData.getMoreGold = true;
            if (getMoreBone) playerSaveData.getMoreBone = true;
            if (pet) playerSaveData.pet = true;

            SaveManager.Save("PlayerData", playerSaveData);
        }
    }

}
