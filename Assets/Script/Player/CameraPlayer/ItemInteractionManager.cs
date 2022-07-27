using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionManager : MonoBehaviour
{
    [Header("Except certain layer when you find item")]
    public LayerMask exceptLayer;

    void Update()
    {
        // Find aimed element 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        
        if (Physics.Raycast(ray, out var hit, 10f, ~exceptLayer))
        {
            if (hit.collider.gameObject.layer == 9) // 9: item layer
            {
                // Cast into testbox and call Use
                TestBox boxItem;
                if (boxItem = hit.collider.gameObject.GetComponent<TestBox>())
                {
                    if (Input.GetKeyDown(KeyCode.F)) // Use Item.
                    {
                        boxItem.Use();
                    }
                }
            }
        }
    }
}
