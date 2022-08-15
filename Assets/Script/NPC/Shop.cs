using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROC;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject startShop;
    public GameObject npc;
    public GameObject npcCamera;

    public LayerMask layerExcept;

    [Header("Item Price")]
    [Space]
    public int[] price;

    public Transform shop;
    public List<Transform> shopitem = new List<Transform>();

    private void Awake()
    {
        for (int i = 0; i < shop.childCount; i++)
        { 
            shopitem.Add(shop.GetChild(i)); 
            if(PlayerSaveData.itemList.Count != 4)
                PlayerSaveData.itemList.Add("1");
        }
    }

    private void Update()
    {
        for(int i = 0; (i < shopitem.Count) && (i < price.Length); i++)
        {
            shopitem[i].GetComponentInChildren<TextMeshProUGUI>().text = " : " + price[i];
        }
    }


    public void Buy(int nthItem)
    {
        if (PlayerSaveData.gold >= price[nthItem]) 
        {
            PlayerSaveData.gold -= price[nthItem];
            if (nthItem == 0)
                CheckItem(nthItem, "Item-Turret.Lv1", "Turret Lv1");
            else if(nthItem == 1)
                CheckItem(nthItem, "Item-Turret.Lv2", "Turret Lv2");
            else if (nthItem == 2) 
                CheckItem(nthItem, "Item-Turret.Lv3", "Turret Lv3");
            else if (nthItem == 3) 
                CheckItem(nthItem, "Item-Turret.Lv4", "Turret Lv4");

        }
    }

    public void Exit()
    {
        ShopMove(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        npc.layer = 9; // 9 : itemLayer
        if(Physics.Raycast(ray, out hit,10f,~layerExcept) && hit.collider.CompareTag("Shop"))
        {
            if (Input.GetKeyDown(KeyCode.F) && startShop.activeSelf == false)
            {
                ShopMove(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        npc.layer = 0;
    }

    public void ShopMove(bool value)
    {

        for (int i = 0; i < Player.instance.ui.DeleteUI.Length; i++)
            Player.instance.ui.DeleteUI[i].SetActive(!value);

        Player.instance.ui.shopUI.SetActive(!value);
        Player.instance.ui.enabled = !value;
        Player.instance.playerCamera.enabled = !value;
        Player.instance.playerCamera.GetComponent<ItemInteractionManager>().enabled = !value;
        Player.instance.playerCamera.GetComponent<StructureSpawn_Test>().enabled = !value;
        Player.instance.movement.enabled = !value;
        Player.instance.playerAttackSkill.enabled = !value;
        Player.instance.animationController.animator.Rebind();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = value;

        Player.instance.unbeatable = value;
        startShop.SetActive(value);
        if(value)
            Player.instance.shopCamera = npcCamera;

        else
            Player.instance.shopCamera = null;

    }

    private void CheckItem(int nthItem, string item, string itemName)
    {
        if (shopitem[nthItem].name == ("Item-Turret.Lv" + (nthItem+1).ToString()))
        {
            PlayerSaveData.itemList[nthItem] = itemName;
        }
    }
}
