using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionManager : MonoBehaviour
{
    [Header("Except certain layer when you find item")]
    public LayerMask exceptLayer;
    public static GameObject shopUI;

    private void Start()
    {
        shopUI = Player.instance.ui.shopUI;    
    }

    void Update()
    {
        // Find aimed element 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        
        if (Physics.Raycast(ray, out var hit, 10f, ~exceptLayer))
        {
            if (hit.collider.gameObject.layer == 9) // 9: item layer
            {
                if(Time.timeScale != 0)
                    shopUI.SetActive(true);
                else
                    shopUI.SetActive(false);
                // Cast into item.
                TestBox boxItem;
                if (boxItem = hit.collider.gameObject.GetComponent<TestBox>()) // if item is boxitem.
                {
                    if (Input.GetKeyDown(KeyCode.F)) // Use Item.
                    {
                        boxItem.Use();
                    }
                }
            }
            else
            {
                shopUI.SetActive(false);
            }
        }
        else
        {
            shopUI.SetActive(false);
        }
    }
}
