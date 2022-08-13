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
        }
    }

    private void Update()
    {
        for(int i = 0; i < shopitem.Count; i++)
        {
            shopitem[i].GetComponentInChildren<TextMeshProUGUI>().text = " : " + price[i];
        }
    }


    public void Buy(int nthItem)
    {
        if (PlayerSaveData.gold >= price[nthItem]) 
        {
            PlayerSaveData.gold -= price[nthItem];
            if(shopitem[nthItem].name == "Item-Turret.Lv1")
            {
                foreach(string item in PlayerSaveData.itemList)
                {
                    if (item == "Turret Lv1") return;
                }
                PlayerSaveData.itemList.Add("Turret Lv1");
            }
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
    }
}
